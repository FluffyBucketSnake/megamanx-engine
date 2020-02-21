using System;
using System.Collections.Generic;
using System.Linq;
using MegamanX.World;
using Microsoft.Xna.Framework;

namespace MegamanX.Physics
{
    public class PhysicWorld : IPhysicSensorParent
    {
        public GameTime CurrentTime { get; private set; }

        public Vector2 Gravity = new Vector2(0, 0.0009f);

        public PhysicBodyCollection Bodies { get; }

        public PhysicSensorCollection Sensors { get; }

        public TileMap Tiles;

        private readonly Dictionary<PhysicBody, Vector2> dirtyBodies = new Dictionary<PhysicBody, Vector2>();

        public PhysicWorld()
        {
            Bodies = new PhysicBodyCollection(this);
            Sensors = new PhysicSensorCollection(this);
        }

        public void Translate(PhysicBody body, Vector2 translation)
        {
            if (translation.X != 0 || translation.Y != 0)
            {
                MarkAsDirty(body, translation);
            }
        }

        public bool CheckSolid(Rectangle area)
        {
            return Tiles.AnySolid(area) 
            || Bodies.Any(e => e.WorldBounds.Intersects(area) 
                && e.Type == CollisionTypes.Solid);
        }

        public bool CheckSolid(Rectangle area, uint maskBits, uint categoryBits)
        {
            return Tiles.AnySolid(area) 
            || Bodies.Any(e => (maskBits & e.CategoryBits) != 0
                && (e.MaskBits & categoryBits) != 0
                && e.WorldBounds.Intersects(area) 
                && e.Type == CollisionTypes.Solid);
        }

        public void Update(GameTime gameTime)
        {
            CurrentTime = gameTime;
            var currentFrameBodies = Bodies.ToList();

            //Update each body.
            foreach (var body in currentFrameBodies)
            {
                //Check if the body moved. If it did, mark as dirty and update delta position. 
                //If not, just update its sensors.
                Vector2 deltaPosition = body.Speed * gameTime.ElapsedGameTime.Milliseconds;
                if (deltaPosition.X != 0 || deltaPosition.Y != 0)
                {
                    MarkAsDirty(body, deltaPosition);
                }
                else
                {
                    UpdateBodySensors(body);
                }

                //Apply gravity if not on ground.
                body.Speed += Gravity * body.GravityScale * 
                gameTime.ElapsedGameTime.Milliseconds;
            }

            //Update dirty bodies.
            foreach (var pair in dirtyBodies)
            {
                PhysicBody body = pair.Key;
                Vector2 translation = pair.Value;
                Vector2 penetration = Vector2.Zero;

                //Body to tile collision.
                bool collided = false;

                if (body.IsTangible)
                {
                    collided = HandleTileCollisionY(body, translation.Y, out penetration.Y);
                }
                body.Position.Y += translation.Y - penetration.Y;
                if (body.IsTangible)
                {
                    collided |= HandleTileCollisionX(body, translation.X, out penetration.X);
                }
                body.Position.X += translation.X - penetration.X;

                //Tilemap collision response.
                if (collided)
                {
                    TileMapCollisionInfo info = new TileMapCollisionInfo();
                    info.GameTime = CurrentTime;
                    info.Penetration = penetration;
                    body.TileMapCollision(info);
                }

                //Body-to-body collision testing.
                foreach (var other in currentFrameBodies)
                {
                    collided = (body.MaskBits & other.CategoryBits) != 0 && (other.MaskBits & body.CategoryBits) != 0 &&
                        body != other && body.WorldBounds.Intersects(other.WorldBounds, out penetration);
                    if (collided)
                    {
                        if (Math.Abs(penetration.X) > Math.Abs(penetration.Y))
                        {
                            penetration.X = 0;
                        }
                        else
                        {
                            penetration.Y = 0;
                        }

                        if (body.IsTangible && other.Type == CollisionTypes.Solid)
                        {
                            body.Position -= penetration;
                            if (penetration.Y * body.Speed.Y > 0)
                            {
                                body.Speed.Y = 0;
                            }
                            else if (penetration.X * body.Speed.X > 0)
                            {
                                body.Speed.X = 0;
                            }

                            BodyCollisionInfo infoOther = new BodyCollisionInfo();
                            infoOther.GameTime = CurrentTime;
                            infoOther.Penetration = -penetration;
                            infoOther.CollidingBody = body;
                            other.BodyCollision(infoOther);
                        }

                        BodyCollisionInfo info = new BodyCollisionInfo();
                        info.GameTime = CurrentTime;
                        info.Penetration = penetration;
                        info.CollidingBody = other;
                        body.BodyCollision(info);
                    }
                }

                //Update sensors.
                UpdateBodySensors(body);
            }
            dirtyBodies.Clear();

            //Update global sensors.
            foreach (var sensor in Sensors)
            {
                sensor.State = CheckSolid(sensor.Area, sensor.MaskBits, sensor.CategoryBits);
            }
        }

        private void UpdateBodySensors(PhysicBody body)
        {
            foreach (var sensor in body.Sensors)
            {
                Rectangle worldArea = sensor.Area;
                worldArea.Offset(body.Position);
                worldArea.Offset(body.Bounds.Location);

                sensor.State = CheckSolid(worldArea, sensor.MaskBits, sensor.CategoryBits);
            }
        }

        private bool HandleTileCollisionX(PhysicBody body, float deltaX, out float penetration)
        {
            bool collided = false;
            penetration = 0;

            if (deltaX > 0)
            {
                int wallX;
                if (Tiles.QueryWallRight(body.WorldBounds, out wallX))
                {
                    int rightWall = (int)Tiles.GetWorldX(wallX);
                    int bodyRight = (int)body.Position.X + body.Bounds.Right;
                    if (rightWall < deltaX + bodyRight)
                    {
                        collided = true;
                        penetration = deltaX + bodyRight - rightWall;
                        if (body.Speed.X > 0)
                        {
                            body.Speed.X = 0;
                        }
                    }
                }
            }
            else if (deltaX < 0)
            {
                int wallX;
                if (Tiles.QueryWallLeft(body.WorldBounds, out wallX))
                {
                    int leftWall = (int)Tiles.GetWorldX(wallX) + Tile.Width;
                    int bodyLeft = (int)body.Position.X + body.Bounds.Left;
                    if (leftWall > deltaX + bodyLeft)
                    {
                        collided = true;
                        penetration = deltaX + bodyLeft - leftWall;
                        if (body.Speed.X > 0)
                        {
                            body.Speed.X = 0;
                        }
                    }
                }
            }

            return collided;
        }

        private bool HandleTileCollisionY(PhysicBody body, float deltaY, out float penetration)
        {
            bool collided = false;
            penetration = 0;

            if (deltaY > 0)
            {
                int floorY;
                if (Tiles.QueryFloor(body.WorldBounds, out floorY))
                {
                    int bottom = (int)Tiles.GetWorldY(floorY);
                    int bodyBottom = (int)(body.Position.Y + body.Bounds.Bottom);
                    if (bottom <= deltaY + bodyBottom)
                    {
                        collided = true;
                        penetration = deltaY + bodyBottom - bottom;
                        if (body.Speed.Y > 0)
                        {
                            body.Speed.Y = 0;
                        }
                    }
                }
            }
            else if (deltaY < 0)
            {
                int ceilingY;
                if (Tiles.QueryCeiling(body.WorldBounds, out ceilingY))
                {
                    int top = (int)Tiles.GetWorldY(ceilingY) + Tile.Height;
                    int bodyTop = (int)body.Position.Y + body.Bounds.Top;
                    if (top > deltaY + bodyTop)
                    {
                        collided = true;
                        penetration = deltaY + bodyTop - top;
                        if (body.Speed.Y < 0)
                        {
                            body.Speed.Y = 0;
                        }
                    }
                }
            }

            return collided;
        }

        private void MarkAsDirty(PhysicBody body, Vector2 deltaPosition)
        {
            if (dirtyBodies.ContainsKey(body))
            {
                dirtyBodies[body] += deltaPosition;
                if (deltaPosition.X == 0 && deltaPosition.Y == 0)
                {
                    dirtyBodies.Remove(body);
                }
            }
            else
            {
                dirtyBodies.Add(body, deltaPosition);
            }
        }
    }
}
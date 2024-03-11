using System;
using System.Collections.Generic;
using System.Linq;
using MegamanX.World;
using Microsoft.Xna.Framework;
using static System.Math;

namespace MegamanX.Physics
{
    public class PhysicWorld : IPhysicSensorParent
    {
        public GameTime CurrentTime { get; private set; }

        public Vector2 Gravity = new Vector2(0, 0.0009f);

        public PhysicBodyCollection Bodies { get; }

        public PhysicSensorCollection Sensors { get; }

        public TileMap Tiles;

        private readonly Dictionary<PhysicBody, Vector2> dirtyBodies = [];

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
            List<PhysicBody> currentFrameBodies = Bodies.ToList();

            //Update each body.
            foreach (PhysicBody? body in currentFrameBodies)
            {
                //Check if the body moved. If it did, mark as dirty and update delta position. 
                //If not, just update its sensors.
                Vector2 deltaPosition = body.Velocity * gameTime.ElapsedGameTime.Milliseconds;
                if (deltaPosition.X != 0 || deltaPosition.Y != 0)
                {
                    MarkAsDirty(body, deltaPosition);
                }
                else
                {
                    UpdateBodySensors(body);
                }

                //Apply gravity if not on ground.
                body.Velocity += Gravity * body.GravityScale *
                gameTime.ElapsedGameTime.Milliseconds;
            }

            //Update dirty bodies.
            foreach (KeyValuePair<PhysicBody, Vector2> pair in dirtyBodies)
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
                body.Position += new Vector2(0, translation.Y - penetration.Y);
                if (body.IsTangible)
                {
                    collided |= HandleTileCollisionX(body, translation.X, out penetration.X);
                }
                body.Position += new Vector2(translation.X - penetration.X, 0);

                //Tilemap collision response.
                if (collided)
                {
                    TileMapCollisionInfo info = new(CurrentTime, penetration);
                    body.TileMapCollision(info);
                }

                //Body-to-body collision testing.
                foreach (var other in currentFrameBodies)
                {
                    collided = (body.MaskBits & other.CategoryBits) != 0 && (other.MaskBits & body.CategoryBits) != 0 &&
                        body != other && body.WorldBounds.Intersects(other.WorldBounds, out penetration);
                    if (collided)
                    {
                        if (Abs(penetration.X) > Abs(penetration.Y))
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
                            if (penetration.Y * body.Velocity.Y > 0)
                            {
                                body.Velocity = new Vector2(body.Velocity.X, 0);
                            }
                            else if (penetration.X * body.Velocity.X > 0)
                            {
                                body.Velocity = new Vector2(0, body.Velocity.Y);
                            }

                            BodyCollisionInfo infoOther = new(CurrentTime, -penetration, body);
                            other.BodyCollision(infoOther);
                        }

                        BodyCollisionInfo info = new(CurrentTime, penetration, other);
                        body.BodyCollision(info);
                    }
                }

                //Update sensors.
                UpdateBodySensors(body);
            }
            dirtyBodies.Clear();

            //Update global sensors.
            foreach (PhysicSensor sensor in Sensors)
            {
                sensor.State = CheckSolid(sensor.Area, sensor.MaskBits, sensor.CategoryBits);
            }
        }

        private void UpdateBodySensors(PhysicBody body)
        {
            foreach (PhysicSensor sensor in body.Sensors)
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
                if (Tiles.QueryWallRight(body.WorldBounds, out int wallX))
                {
                    int rightWall = (int)Tiles.GetWorldX(wallX);
                    int bodyRight = (int)body.Position.X + body.Bounds.Right;
                    if (rightWall < deltaX + bodyRight)
                    {
                        collided = true;
                        penetration = deltaX + bodyRight - rightWall;
                        if (body.Velocity.X > 0)
                        {
                            body.Velocity = new Vector2(0, body.Velocity.Y);
                        }
                    }
                }
            }
            else if (deltaX < 0)
            {
                if (Tiles.QueryWallLeft(body.WorldBounds, out int wallX))
                {
                    int leftWall = (int)Tiles.GetWorldX(wallX) + Tile.Width;
                    int bodyLeft = (int)body.Position.X + body.Bounds.Left;
                    if (leftWall > deltaX + bodyLeft)
                    {
                        collided = true;
                        penetration = deltaX + bodyLeft - leftWall;
                        if (body.Velocity.X > 0)
                        {
                            body.Velocity = new Vector2(0, body.Velocity.Y);
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
                if (Tiles.QueryFloor(body.WorldBounds, out int floorY))
                {
                    int bottom = (int)Tiles.GetWorldY(floorY);
                    int bodyBottom = (int)(body.Position.Y + body.Bounds.Bottom);
                    if (bottom <= deltaY + bodyBottom)
                    {
                        collided = true;
                        penetration = deltaY + bodyBottom - bottom;
                        if (body.Velocity.Y > 0)
                        {
                            body.Velocity = new Vector2(body.Velocity.X, 0);
                        }
                    }
                }
            }
            else if (deltaY < 0)
            {
                if (Tiles.QueryCeiling(body.WorldBounds, out int ceilingY))
                {
                    int top = (int)Tiles.GetWorldY(ceilingY) + Tile.Height;
                    int bodyTop = (int)body.Position.Y + body.Bounds.Top;
                    if (top > deltaY + bodyTop)
                    {
                        collided = true;
                        penetration = deltaY + bodyTop - top;
                        if (body.Velocity.Y < 0)
                        {
                            body.Velocity = new Vector2(body.Velocity.X, 0);
                        }
                    }
                }
            }

            return collided;
        }

        private void MarkAsDirty(PhysicBody body, Vector2 deltaPosition)
        {
            if (!dirtyBodies.TryAdd(body, deltaPosition))
            {
                dirtyBodies[body] += deltaPosition;
                if (deltaPosition.X == 0 && deltaPosition.Y == 0)
                {
                    _ = dirtyBodies.Remove(body);
                }
            }
        }
    }
}

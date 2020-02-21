using MegamanX.Physics;
using MegamanX.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.GameObjects.Debug
{
    public class PhysicDebugObject : GameObject
    {
        public PhysicBody Body;

        public PhysicDebugObject()
        {
            Body = new PhysicBody(new Rectangle(0,0,16,16), Position);
            Body.Sensors.Add(new PhysicSensor(new Rectangle(-3,0,3,16)));   //Left
            Body.Sensors.Add(new PhysicSensor(new Rectangle(0,-3,16,3)));   //Top
            Body.Sensors.Add(new PhysicSensor(new Rectangle(16,0,3,16)));   //Right
            Body.Sensors.Add(new PhysicSensor(new Rectangle(0,16,16,3)));   //Bottom
        }

        protected override void OnPositionChange(PositionChangedArgs e)
        {
            Body.Position = e.NewPosition;
        }

        protected override void OnCreation(object sender)
        {
            base.OnCreation(sender);
            Map.World.Bodies.Add(Body);
        }

        protected override void OnDestruction(object sender)
        {
            base.OnDestruction(sender);
            Map.World.Bodies.Remove(Body);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {
            Position = Body.Position;

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                Body.GravityScale = -1f;
            }
            else
            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                Body.GravityScale = 1f;
            }

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                Body.Speed.X = -0.3f;
            }
            else
            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                Body.Speed.X = 0.3f;
            }
        }
    }
}
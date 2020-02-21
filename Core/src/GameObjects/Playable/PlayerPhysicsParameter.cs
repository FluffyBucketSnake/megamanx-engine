using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MegamanX.GameObjects.Playable
{
    public struct PlayerPhysicsParameters
    {
        public float WalkingSpeed;

        public float DashingSpeed;

        public float JumpSpeed;

        public float WallslidingSpeed;

        public Vector2 LeftKnockbackSpeed;

        public Vector2 CenterKnockbackSpeed;

        public Vector2 RightKnockbackSpeed;

        public static PlayerPhysicsParameters Default => new PlayerPhysicsParameters() 
        { 
            WalkingSpeed =  0.088125f,
            DashingSpeed = 0.207421875f,
            JumpSpeed = 0.319453125f,
            WallslidingSpeed = 0.12f,
            LeftKnockbackSpeed = new Vector2(0.03234375f,-0.09f),
            CenterKnockbackSpeed = new Vector2(0,-0.09f),
            RightKnockbackSpeed = new Vector2(-0.03234375f,-0.09f)
        };
    }

    public class JSONPlayerPhysicsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(PlayerPhysicsParameters));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = PlayerPhysicsParameters.Default;
            while(reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                {
                    break;
                }

                var propertyName = (string)reader.Value;
                if (!reader.Read())
                {
                    continue;
                }

                switch(propertyName)
                {
                    case "WalkingSpeed": 
                    result.WalkingSpeed =  serializer.Deserialize<float>(reader); 
                    break;
                    case "JumpSpeed": 
                    result.JumpSpeed = serializer.Deserialize<float>(reader); 
                    break;
                    case "DashingSpeed": 
                    result.DashingSpeed = serializer.Deserialize<float>(reader);
                    break;
                    case "WallslidingSpeed": 
                    result.WallslidingSpeed = serializer.Deserialize<float>(reader); 
                    break;
                    case "LeftKnockbackSpeed": 
                    result.LeftKnockbackSpeed =  serializer.Deserialize<Vector2>(reader); 
                    break;
                    case "CenterKnockbackSpeed": 
                    result.CenterKnockbackSpeed = serializer.Deserialize<Vector2>(reader); 
                    break;
                    case "RightKnockbackSpeed": 
                    result.RightKnockbackSpeed = serializer.Deserialize<Vector2>(reader); 
                    break;
                }
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var physics = (PlayerPhysicsParameters)value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(physics.WalkingSpeed));
            serializer.Serialize(writer,physics.WalkingSpeed);

            writer.WritePropertyName(nameof(physics.JumpSpeed));
            serializer.Serialize(writer,physics.JumpSpeed);

            writer.WritePropertyName(nameof(physics.DashingSpeed));
            serializer.Serialize(writer,physics.DashingSpeed);

            writer.WritePropertyName(nameof(physics.WallslidingSpeed));
            serializer.Serialize(writer,physics.WallslidingSpeed);

            writer.WritePropertyName(nameof(physics.LeftKnockbackSpeed));
            serializer.Serialize(writer,physics.LeftKnockbackSpeed);

            writer.WritePropertyName(nameof(physics.CenterKnockbackSpeed));
            serializer.Serialize(writer,physics.CenterKnockbackSpeed);

            writer.WritePropertyName(nameof(physics.RightKnockbackSpeed));;
            serializer.Serialize(writer,physics.RightKnockbackSpeed);
            writer.WriteEndObject();
        }
    }
}
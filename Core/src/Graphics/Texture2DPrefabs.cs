using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Graphics
{
    public static class Texture2DPrefabs
    {
        public static Texture2D CreateWhitePixel(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException(nameof(graphicsDevice));
            }
            //Attempt to create a white pixel texture.
            var result = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            result.SetData(new Color[] { Color.White });
            //Return result.
            return result;
        }
    }
}
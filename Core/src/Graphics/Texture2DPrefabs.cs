using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Graphics
{
    public static class Texture2DPrefabs
    {
        public static Texture2D CreateWhitePixel(GraphicsDevice graphicsDevice)
        {
            Texture2D result = new(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            result.SetData(new Color[] { Color.White });
            return result;
        }
    }
}

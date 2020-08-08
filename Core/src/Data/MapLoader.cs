using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MegamanX.Input.Keyboard;
using MegamanX.GameObjects;
using MegamanX.GameObjects.Playable;
using MegamanX.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace MegamanX.Data
{
    public static class MapLoader
    {
        public static GameWorld LoadFromTMX(string filename, ContentManager content, IServiceProvider services)
        {
            filename = Path.GetFullPath(filename);
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"Couldn't find the '{filename}' file.");
            }

            GameWorld map = new GameWorld();

            //Open document.
            var xDocument = XDocument.Load(filename);
            var xMap = xDocument.Element("map");

            //Read properties.
            ReadProperties(map, xMap, content);

            //Read tilesets.
            Dictionary<int, Tile> tileset = ReadTileset(xMap, content);

            //Read objects.
            foreach (var xObjectGroup in xMap.Elements("objectgroup"))
            {
                foreach (var xObject in xObjectGroup.Elements("object"))
                {
                    map.Objects.Add(BuildGameObject(xObject, services));
                }
            }

            //Read layers.
            TileMap tileMap = (ReadLayers(xMap, tileset)).First();
            map.World.Tiles = tileMap;

            return map;
        }

        private static void ReadProperties(GameWorld map, XElement xMap, ContentManager content)
        {
            int width = XParser.ParseInt(xMap, "width") * Tile.Width;
            int height = XParser.ParseInt(xMap, "height") * Tile.Height;

            var properties = xMap.Element("properties");
            if (properties != null)
            {
                var musicProperty = properties.Elements().FirstOrDefault(e => XParser.GetString(e, "name") == "music");
                if (musicProperty != null)
                {
                    map.Music = content.Load<Song>(XParser.GetString(musicProperty, "value"));
                }
            }
        }

        private static Dictionary<int, Tile> ReadTileset(XElement xMap, ContentManager content)
        {
            Dictionary<int, Tile> tileset = new Dictionary<int, Tile>();

            //The '0' tile is empty.
            tileset.Add(0, Tile.Empty);

            //Parse each tileset.
            foreach (var xTileset in xMap.Elements("tileset"))
            {
                //Get tileset information.
                int tileCount = XParser.ParseInt(xTileset, "tilecount");
                int firstGID = XParser.ParseInt(xTileset, "firstgid");
                int columns = XParser.ParseInt(xTileset, "columns");

                //Get main image.
                var xImage = xTileset.Element("image");
                Texture2D image = (xImage != null) ? content.Load<Texture2D>(XParser.GetString(xImage, "source")) : null;

                if (image != null)
                {
                    foreach (var xTile in xTileset.Elements("tile"))
                    {
                        int id = XParser.ParseInt(xTile, "id");

                        Tile tile = new Tile();
                        tile.Texture = image;
                        tile.Type = ParseTileType(xTile);
                        tile.UV = new Point(id % columns, id / columns);

                        tileset.Add(id + firstGID, tile);
                    }
                }
                else
                {
                    foreach (var xTile in xTileset.Elements("tile"))
                    {
                        int id = XParser.ParseInt(xTile, "id");

                        var xTileImage = xTile.Element("image");
                        Texture2D tileImage = content.Load<Texture2D>(XParser.GetString(xTileImage, "source"));

                        Tile tile = new Tile();
                        tile.Texture = tileImage;
                        tile.Type = ParseTileType(xTile);
                        tile.UV = Point.Zero;

                        tileset.Add(id + firstGID, tile);
                    }
                }
            }

            return tileset;
        }

        private static TileType ParseTileType(XElement xTile)
        {
            TileType result;

            var typeAttribute = xTile.Attribute("type");
            if (typeAttribute == null || String.IsNullOrWhiteSpace(typeAttribute.Value) ||
            !Enum.TryParse(typeAttribute.Value, out result))
            {
                return TileType.Empty;
            }

            return result;
        }

        private static GameObject BuildGameObject(XElement sourceElement, IServiceProvider services)
        {
            string typeName = XParser.GetString(sourceElement, "type");
            GameObject gameObject;

            switch (typeName)
            {
                case "MegamanX.GameObjects.Playable.Player":
                    Player player = new Player((IKeyboardDevice)services.GetService(typeof(IKeyboardDevice)));
                    gameObject = player;
                    break;

                case "MegamanX.GameObjects.GameObjectSpawner":
                    GameObjectSpawner spawner = new GameObjectSpawner();
                    string spawnTypeName = XParser.GetString(sourceElement, "spawntype");
                    Type spawnType = Type.GetType(spawnTypeName);
                    if (!typeof(GameObject).IsAssignableFrom(spawnType))
                    {
                        throw new TypeLoadException($"'{spawnTypeName}' is not a valid spawnable GameObject.");
                    }
                    spawner.SpawnType = spawnType;
                    gameObject = spawner;
                    break;

                default:
                    var type = Type.GetType(typeName);
                    if (!typeof(GameObject).IsAssignableFrom(type))
                    {
                        throw new TypeLoadException($"'{typeName}' is not a valid GameObject.");
                    }
                    gameObject = (GameObject)Activator.CreateInstance(type);
                    break;
            }

            float x = XParser.ParseFloat(sourceElement, "x");
            float y = XParser.ParseFloat(sourceElement, "y");
            gameObject.Position = new Vector2(x, y);

            return gameObject;
        }

        private static IEnumerable<TileMap> ReadLayers(XElement xMap, Dictionary<int, Tile> tileset)
        {
            List<TileMap> tileMaps = new List<TileMap>();
            foreach (var xLayer in xMap.Elements("layer"))
            {
                TileMap tileMap = new TileMap(XParser.ParseInt(xLayer, "width"), XParser.ParseInt(xLayer, "height"));

                var xData = xLayer.Element("data");
                if (xData != null)
                {
                    string csvData = xData.Value;
                    int[] tileData = csvData.Split(',').Select(s => int.Parse(s)).ToArray();

                    for (int y = 0; y < tileMap.GridHeight; y++)
                    {
                        for (int x = 0; x < tileMap.GridWidth; x++)
                        {
                            var tileID = tileData[(y * tileMap.GridWidth) + x];
                            var tile = tileset[tileID];
                            tileMap[x, y] = tile;
                        }
                    }
                }

                tileMaps.Add(tileMap);
            }

            return tileMaps;
        }
    }
}
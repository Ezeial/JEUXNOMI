using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace YIKES.Class
{
    enum TileCollision
    {
        Passable = 0,
        Impassable = 1,
        Platform = 2,
    }

    struct Tile
    {
        public Texture2D Texture;
        public TileCollision Collision;

        public const int Width = 40;
        public const int Height = 32;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(Texture2D texture, TileCollision collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }

    class TilesManager
    {
        private ContentManager Content;
        public Vector2 TilesSize;
        public Tile[,] Tiles;
        
        public TilesManager(ContentManager content)
        {
            Content = content;
        }

        public void LoadAllTiles(Stream fileStream)
        {
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new System.Exception(System.String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            Tiles = new Tile[width, lines.Count];
            TilesSize = new Vector2(width, lines.Count);

            for (int y = 0; y < TilesSize.Y; ++y)
            {
                for (int x = 0; x < TilesSize.X; ++x)
                {
                    char tile_type = lines[y][x];
                    Tiles[x, y] = LoadTile(tile_type);
                }
            }
        }

        public Tile LoadTile(char tile_type)
        {
            Texture2D block_texture = Content.Load<Texture2D>("Tile/BlockA0");

            switch (tile_type)
            {
                case ('.'):
                    return new Tile(null, TileCollision.Passable);
                case ('#'):
                    return new Tile(block_texture, TileCollision.Impassable);
                default:
                    throw new System.Exception("Invalid specifier in map found.");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < TilesSize.Y; ++y)
            {
                for (int x = 0; x < TilesSize.X; ++x)
                {
                    if (Tiles[x, y].Texture != null)
                        spriteBatch.Draw(Tiles[x, y].Texture, new Vector2(x, y) * Tile.Size, Color.White);
                }
            }
        }

        public Rectangle GetBounds(Vector2 Indexes)
        {
            return new Rectangle((Indexes * Tile.Size).ToPoint(), Tile.Size.ToPoint());
        }
    }
}

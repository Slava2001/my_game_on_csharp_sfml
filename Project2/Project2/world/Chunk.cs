using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class Chunk: Transformable, Drawable
    {
        public const int chunk_size = 5;

        Tile[][] Tiles;

        Random rnd;

        World world;

        SFML.System.Vector2i chunk_poz;

        public Chunk(Random rnd ,World world ,SFML.System.Vector2i chunk_poz)
        {
            this.chunk_poz = chunk_poz;
            this.world = world;
            this.rnd = rnd;

            Tiles = new Tile[chunk_size][];
            for (int y = 0; y < chunk_size; y++)
            {
                Tiles[y] = new Tile[chunk_size];
                for (int x= 0; x < chunk_size; x++)
                {
                    SetTile(TileType.AIR, y, x);
                }
            }

        }

        public void Update()
        {
            for (int x = 0; x < chunk_size; x++)
                for (int y = 0; y < chunk_size; y++)
                    if (Tiles[y][x] != null) Tiles[y][x].Update();

           Debug.Add(chunk_poz.X * chunk_size * Tile.tile_size, chunk_poz.Y * chunk_size * Tile.tile_size, chunk_size * Tile.tile_size, chunk_size * Tile.tile_size, Color.Yellow);
        }      

        public void SetTile(TileType type,int y, int x)
        {
            Tiles[y][x] = new Tile(type,rnd,world,chunk_poz);
            Tiles[y][x].Position = new SFML.System.Vector2f(x * Tile.tile_size, y * Tile.tile_size);
        }

        public TileType GetTileType(int y, int x)
        {
            return Tiles[y][x]!=null? Tiles[y][x].type:TileType.AIR;
        }

        public Tile GetTile(int y, int x)
        {
            return Tiles[y][x];
        }

        public string Save()
        {
            string chunck="(";

            for (int y = 0; y < chunk_size; y++)
                for (int x = 0; x < chunk_size; x++)
                    chunck += ((char)Tiles[y][x].type).ToString();
            return chunck+")";
        }

        public void Load(string chunk)
        {
            for (int i = 0; i < chunk_size * chunk_size; i++)
            {
                SetTile((TileType)chunk[i+1], (i) / chunk_size, (i) % chunk_size);
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            for (int x = 0; x < chunk_size; x++)
                for (int y = 0; y < chunk_size; y++)
                    if (Tiles[y][x] != null) target.Draw(Tiles[y][x],states);
        }
    }
}

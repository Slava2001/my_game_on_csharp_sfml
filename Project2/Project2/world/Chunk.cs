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
        public const int chunk_size = 16;

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
                {
                    chunck += ((char)Tiles[y][x].type).ToString();
                    if ((Tiles[y][x].type==TileType.CHEAST)|| (Tiles[y][x].type == TileType.FURNACE))
                    {
                        chunck += "{";
                        for (int i = 0; i < Tiles[y][x].inventar_types.Length; i++)
                        {
                            chunck += ((char)Tiles[y][x].inventar_types[i]).ToString();
                            chunck += ((char)Tiles[y][x].inventar_count[i]).ToString();
                        }
                        chunck += "}";
                    }

                }
           return chunck+")";
        }

        public void Load(string chunk)
        {
            int strind = 1;
            for (int i = 0; i < chunk_size * chunk_size; i++, strind++)
            {
                SetTile((TileType)chunk[strind], (i) / chunk_size, (i) % chunk_size);

                if ((Tiles[(i) / chunk_size][(i) % chunk_size].type == TileType.CHEAST)|| (Tiles[(i) / chunk_size][(i) % chunk_size].type == TileType.FURNACE))
                {
                   
                    strind++;
                    strind++;
                    String str = "";
                    for (;chunk[strind]!='}';strind++)
                    {
                        str += chunk[strind];
                    }
                    load_tile_inventory(str,i);
                }
            }

            
        }
        void load_tile_inventory(string str,int i)
        {
            for (int t = 0; t < str.Length; t += 2)
            {

                Tiles[(i) / chunk_size][(i) % chunk_size].inventar_types[t / 2] = (TileType)str[t];
                Tiles[(i) / chunk_size][(i) % chunk_size].inventar_count[t / 2] = (int)str[t + 1];
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

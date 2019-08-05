using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class World : Transformable,Drawable
    {
        public string WorldName;

        public const int world_size_Y =20;
        public const int world_size_X = 100;

        Chunk[][] chunks;

        Random rnd;

        SFML.System.Vector2i plpoz_chunks;

        TextReader datafileR;

      

        public World(string wn,int seed)
        {
            WorldName = wn;
           
            chunks = new Chunk[world_size_Y][];
            for (int y = 0; y < world_size_Y; y++)
            
                chunks[y] = new Chunk[world_size_X];


            try
            {
                datafileR = new StreamReader("maps/" + WorldName + "/seed_date.txt");
                rnd = new Random(Convert.ToInt32(datafileR.ReadLine()));
                datafileR.Close();
            }
            catch
            {
                rnd = new Random(seed);             
            }


            try
            {
                datafileR = new StreamReader("maps/" + WorldName + "/map_date.txt");
              
                Load();
            }
            catch
            {
                Directory.CreateDirectory("maps/" + WorldName);
                TextWriter datafileseedW = new StreamWriter("maps/" + WorldName + "/seed_date.txt");
                datafileseedW.WriteLine(seed.ToString());
                datafileseedW.Close();
            }



            generation();

        }

        int[] HeightMap;
        int[][] map;
        void generation()
        {
            HeightMap = new int[world_size_X + 1];
            HeightMap[0]= rnd.Next(-5, 5);
            for (int i = 1; i <= world_size_X; i++)
                HeightMap[i] = HeightMap[i-1]+rnd.Next(-5, 5);

           

            map = new int[world_size_X+1][];
            for (int i = 0; i <= world_size_X; i++)
            {
                map[i] = new int[world_size_Y + 1];
                for (int t = 0; t <= world_size_Y; t++)
                    map[i][t] = rnd.Next(0, 255);
            }
        }

        public void Update(SFML.System.Vector2i plpoz)
        {
            this.plpoz_chunks = plpoz;
            for (int x = (plpoz_chunks.X - 1 - 5 / 2) > 0 ? (plpoz_chunks.X - 1 - 5 / 2) : 0; x < ((plpoz_chunks.X + 2 + 5 / 2) < world_size_X ? (plpoz_chunks.X + 2 + 5 / 2) : world_size_X); x++)
                for (int y = (plpoz_chunks.Y - 1 - 5 / 2) > 0 ? (plpoz_chunks.Y - 1 - 5 / 2) : 0; y < ((plpoz_chunks.Y + 3 + 5 / 2) < world_size_Y ? (plpoz_chunks.Y + 3 + 5 / 2) : world_size_Y); y++)
                     if (chunks[y][x] != null) chunks[y][x].Update();

          
        }

        public void SetTile_world(TileType type, int cy,int cx)
        {
            chunks[cy / Chunk.chunk_size][cx / Chunk.chunk_size].SetTile(type, cy % Chunk.chunk_size, cx % Chunk.chunk_size);

        }

        public TileType GetTileType_world(int y, int x)
        {
            if (x>0 && x < world_size_X * Chunk.chunk_size && y > 0 && y < world_size_Y * Chunk.chunk_size)
            {
                return chunks[y / Chunk.chunk_size][x / Chunk.chunk_size] != null ?
                    chunks[y / Chunk.chunk_size][x / Chunk.chunk_size].GetTileType(y % Chunk.chunk_size, x % Chunk.chunk_size) : TileType.AIR;
            }
            else
            {
                return TileType.AIR;
            }
        }

        public Tile GetTile_world(int y,int x)
        {
            if (x >= 0 && x < world_size_X * Chunk.chunk_size && y >= 0 && y < world_size_Y * Chunk.chunk_size&& chunks[y / Chunk.chunk_size][x / Chunk.chunk_size]!=null)
            {
                return chunks[y / Chunk.chunk_size][x / Chunk.chunk_size].GetTile(y % Chunk.chunk_size, x % Chunk.chunk_size);
            }
            else
            {
                return null;
            }
        }

        public void SetChunk(int y, int x)
        {

            chunks[y][x] = new Chunk(rnd,this,new SFML.System.Vector2i (x,y));
            chunks[y][x].Position = new SFML.System.Vector2f(x * Chunk.chunk_size*Tile.tile_size, y * Chunk.chunk_size * Tile.tile_size);

        }

        public void Save()
        {
            string world="";

            for (int y = 0; y < world_size_Y; y++)
                for (int x = 0; x < world_size_X; x++)
                    if (chunks[y][x] != null)
                        world += chunks[y][x].Save();
                    else
                        world+= "(N)";


            TextWriter datafileW = new StreamWriter("maps/" + WorldName + "/map_date.txt");
            // написать строку текста в файл
            datafileW.WriteLine(world);

            // закрыть поток
            datafileW.Close();

          

        }

        public void Load()
        {
          
            string world = datafileR.ReadToEnd();
            
            datafileR.Close();
            string chunk = "";

            int t = 0;

         

            for (int i = 0; i < world.Length; i++)
            {

              
                 if (world[i] == ')')
                 {

                     if (chunk != "(N")
                     {
                         SetChunk(t / world_size_X , t % world_size_X );
                         chunks[t / world_size_X][ t % world_size_X].Load(chunk);
                     }

                    chunk = "";
                     t++;
                 }
                 else
                 {
                     chunk += world[i];
                 }
            }

           
        }

        //**************************************************************************************************************************
        void chunks_generator(int y, int x)
        {
            if (x >= 0 && x < world_size_X && y >= 0 && y < world_size_Y)
            {
                SetChunk(y, x);
                for (int cx = 0; cx < Chunk.chunk_size; cx++)
                    for (int cy = 0; cy < Chunk.chunk_size; cy++)
                    {
                        if ((cy + (y * Chunk.chunk_size)) < Interpol(cx+x*Chunk.chunk_size, HeightMap, world_size_X))
                        {
                          SetTile_world(TileType.AIR, cy +y* Chunk.chunk_size, cx+x * Chunk.chunk_size);
                        }
                        else
                         {
                            if (func(cx + x * Chunk.chunk_size, cy + y * Chunk.chunk_size) == 0)

                                if ((cy + (y * Chunk.chunk_size)) == Interpol(cx + x * Chunk.chunk_size, HeightMap, world_size_X))
                                {
                                    SetTile_world(TileType.GRASS, cy + y * Chunk.chunk_size, cx + x * Chunk.chunk_size);
                                }
                                else
                                {
                                    if((cy + (y * Chunk.chunk_size)) > 10+Interpol(cx + x * Chunk.chunk_size, HeightMap, world_size_X))
                                    SetTile_world(TileType.STOUN, cy + y * Chunk.chunk_size, cx + x * Chunk.chunk_size);
                                    else
                                        SetTile_world(TileType.DIRT, cy + y * Chunk.chunk_size, cx + x * Chunk.chunk_size);
                                }

                            else
                                SetTile_world(TileType.AIR, cy + y * Chunk.chunk_size, cx + x * Chunk.chunk_size);
                         }

                    }
            }
        }
        byte func(int cx,int cy)
        {

            int x_ind = cx / Chunk.chunk_size;
            int y_ind = cy / Chunk.chunk_size;

            double z1 = 0;
            double z2 = 0;
            double ansv = 0;


            z1 = inter(x_ind*Chunk.chunk_size,
                       cx,
                       (x_ind+1) * Chunk.chunk_size,
                      
                       map[x_ind][y_ind],
                       map[x_ind + 1][y_ind]
                      );

            z2 = inter(x_ind * Chunk.chunk_size,
                       cx,
                      (x_ind + 1) * Chunk.chunk_size,

                       map[x_ind][y_ind+1],
                       map[x_ind + 1][y_ind+1]
                      );
                      

            ansv = inter(y_ind * Chunk.chunk_size,
                       cy,
                       (y_ind + 1) * Chunk.chunk_size,
                         z1,
                         z2
                         );

            if (ansv < 150)
                ansv = 0;
            return (byte)ansv;
        }
        double inter(double x1, double x2, double x3, double y1, double y3)
        {
            return ((x2 - x1) * (y3 - y1) / (x3 - x1)) + y1;
        }
     
        int Interpol(double x, int[] yValues, int size)
        {
           return (int)((x - (x-(x%Chunk.chunk_size))) * (yValues[(int)x/Chunk.chunk_size+1] - yValues[(int)x / Chunk.chunk_size]) / (Chunk.chunk_size) + yValues[(int)x / Chunk.chunk_size]);
        }
           //**************************************************************************************************************************


           /*    void chunks_generator(int y, int x)
               {
                   ///////генератор чанков
                   if (x >= 0 && x < world_size_X && y >= 0 && y < world_size_Y) {
                       SetChunk(y, x);

                        if (y == 8)
                       {
                           for (int cx = x * Chunk.chunk_size; cx < (x + 1) * Chunk.chunk_size; cx++)
                           {

                               for (int cy = y * Chunk.chunk_size; cy < Chunk.chunk_size + y * Chunk.chunk_size; cy++)
                                   SetTile_world(rnd.Next(0, 10) == 0 ? TileType.LEAF : TileType.AIR, cy, cx);
                           }
                       }
                       else if (y == 9)
                       {
                           for (int cx = x * Chunk.chunk_size; cx < (x + 1) * Chunk.chunk_size; cx++)
                           {

                               for (int cy = y * Chunk.chunk_size; cy < Chunk.chunk_size + y * Chunk.chunk_size; cy++)
                                   SetTile_world(rnd.Next(0, 10) == 0 ? TileType.WOOD : TileType.AIR, cy, cx);
                           }
                       }
                       else if (y == 10)
                       {
                           for (int cx = x * Chunk.chunk_size; cx < (x + 1) * Chunk.chunk_size; cx++)
                           {

                               int a = rnd.Next(-2, 2);

                               SetTile_world(TileType.GRASS, y * Chunk.chunk_size + a, cx);

                               for (int cy = y * Chunk.chunk_size + a +1; cy < Chunk.chunk_size + y * Chunk.chunk_size; cy++)
                               {
                                   SetTile_world(TileType.DIRT, cy, cx);
                               }
                           }
                       }
                       else if (y == 11)
                       {
                           for (int cx = x * Chunk.chunk_size; cx < (x + 1) * Chunk.chunk_size; cx++)
                           {

                               for (int cy = y * Chunk.chunk_size; cy < Chunk.chunk_size + y * Chunk.chunk_size; cy++)
                                   SetTile_world(rnd.Next(0,10)==0?TileType.DIRT:TileType.STOUN, cy, cx);
                           }
                       }
                       else if (y == 12)
                       {
                           for (int cx = x * Chunk.chunk_size; cx < (x + 1) * Chunk.chunk_size; cx++)
                           {

                               for (int cy = y * Chunk.chunk_size; cy < Chunk.chunk_size + y * Chunk.chunk_size; cy++)
                                   SetTile_world(rnd.Next(0, 10) == 0 ? TileType.COAL : TileType.STOUN, cy, cx);
                           }
                       }
                       else if (y == 13)
                       {
                           for (int cx = x * Chunk.chunk_size; cx < (x + 1) * Chunk.chunk_size; cx++)
                           {

                               for (int cy = y * Chunk.chunk_size; cy < Chunk.chunk_size + y * Chunk.chunk_size; cy++)
                                   SetTile_world(rnd.Next(0, 10) == 0 ? TileType.IRON : TileType.STOUN, cy, cx);
                           }
                       }
                       else if (y == 14)
                       {
                           for (int cx = x * Chunk.chunk_size; cx < (x + 1) * Chunk.chunk_size; cx++)
                           {

                               for (int cy = y * Chunk.chunk_size; cy < Chunk.chunk_size + y * Chunk.chunk_size; cy++)
                                   SetTile_world(rnd.Next(0, 10) == 0 ? TileType.GOLD : TileType.STOUN, cy, cx);
                           }
                       }
                       else if (y == 15)
                       {
                           for (int cx = x * Chunk.chunk_size; cx < (x + 1) * Chunk.chunk_size; cx++)
                           {

                               for (int cy = y * Chunk.chunk_size; cy < Chunk.chunk_size + y * Chunk.chunk_size; cy++)
                                   SetTile_world(rnd.Next(0, 10) == 0 ? TileType.DIAMONT : TileType.STOUN, cy, cx);
                           }
                       }
                       else if (y > 15)
                       {
                           for (int cx = x * Chunk.chunk_size; cx < (x + 1) * Chunk.chunk_size; cx++)
                           {

                               for (int cy = y * Chunk.chunk_size; cy < Chunk.chunk_size + y * Chunk.chunk_size; cy++)
                                   SetTile_world(TileType.STOUN, cy, cx);
                           }
                       }
                   }
               }*/

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

                SFML.System.Vector2i view_size_chuncs = new SFML.System.Vector2i((int)Core.game_view.Size.X / Tile.tile_size / Chunk.chunk_size, (int)Core.game_view.Size.Y / Tile.tile_size / Chunk.chunk_size);

                for (int x = (plpoz_chunks.X - 1 - view_size_chuncs.X / 2) > 0 ? (plpoz_chunks.X - 1 - view_size_chuncs.X / 2) : 0; x < ((plpoz_chunks.X + 2 + view_size_chuncs.X / 2) < world_size_X ? (plpoz_chunks.X + 2 + view_size_chuncs.X / 2) : world_size_X); x++)
                    for (int y = (plpoz_chunks.Y - 1 - view_size_chuncs.Y / 2) > 0 ? (plpoz_chunks.Y - 1 - view_size_chuncs.Y / 2) : 0; y < ((plpoz_chunks.Y + 3 + view_size_chuncs.Y / 2) < world_size_Y ? (plpoz_chunks.Y + 3 + view_size_chuncs.Y / 2) : world_size_Y); y++)
                    {
                    if (chunks[y][x] == null) chunks_generator(y, x);
                        target.Draw(chunks[y][x], states);
                    }

          
            
        }
    }
}


   
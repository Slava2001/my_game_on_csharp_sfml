using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    enum TileType
    {
        AIR = 0,
        GRASS,
        DIRT,
        STOUN,
        PLATE,
        WOOD,
        LEAF,
        COAL,
        GOLD,
        IRON,
        DIAMONT,
        CRAFTTABEL,
        FURNACE,
        CHEAST
    }

    class Tile : Transformable, Drawable
    {
        public const int tile_size = 16;

        public TileType type = TileType.DIRT;

        World world;

        RectangleShape tile_rectangle;

        IntRect get_rect(int x, int y, bool flip)
        {
            if (!flip) { return new IntRect(x * tile_size, y * tile_size, tile_size, tile_size); }
            else { return new IntRect((x + 1) * tile_size, y * tile_size, -tile_size, tile_size); }
        }
        Random rnd;

        SFML.System.Vector2i chunk_poz;

        public TileSettings.TileSettngsStruct settings;

       public TileType[] inventar_types;
       public int[] inventar_count;

        public Tile(TileType type, Random rnd, World world, SFML.System.Vector2i chunk_poz)
        {

           

            this.chunk_poz = chunk_poz;
            this.world = world;
            this.rnd = rnd;

            this.type = type;
            tile_rectangle = new RectangleShape(new SFML.System.Vector2f(tile_size, tile_size));

         


            switch (type)
            {
                case TileType.AIR:
                    {
                        tile_rectangle = new RectangleShape(new SFML.System.Vector2f(tile_size, tile_size));
                        tile_rectangle.FillColor = Color.Transparent;
                        settings = TileSettings.tilesettings[(int)TileType.AIR];
                        break;
                    }
                case TileType.DIRT:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(rnd.Next(0, 3), 1, rnd.Next(0, 2) == 0);
                        settings = TileSettings.tilesettings[(int)TileType.DIRT];
                        break;
                    }
                case TileType.STOUN:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(rnd.Next(0, 3), 2, rnd.Next(0, 2) == 0);
                        settings = TileSettings.tilesettings[(int)TileType.STOUN];
                        break;
                    }
                case TileType.GRASS:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(0, 0, false);
                        settings = TileSettings.tilesettings[(int)TileType.GRASS];
                        break;
                    }
                case TileType.PLATE:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(0,3, false);
                        settings = TileSettings.tilesettings[(int)TileType.PLATE];
                        break;
                    }
                case TileType.WOOD:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(1, 3, rnd.Next(0, 2) == 0);
                        settings = TileSettings.tilesettings[(int)TileType.WOOD];
                        break;
                    }
                case TileType.LEAF:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(4, 3, rnd.Next(0, 2) == 0);
                        settings = TileSettings.tilesettings[(int)TileType.LEAF];
                        break;
                    }
                case TileType.COAL:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(rnd.Next(0, 3), 4, rnd.Next(0, 2) == 0);
                        settings = TileSettings.tilesettings[(int)TileType.COAL];
                        break;
                    }
                case TileType.GOLD:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(rnd.Next(0, 3), 5, rnd.Next(0, 2) == 0);
                        settings = TileSettings.tilesettings[(int)TileType.GOLD];
                        break;
                    }
                case TileType.IRON:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(rnd.Next(0, 3), 6, rnd.Next(0, 2) == 0);
                        settings = TileSettings.tilesettings[(int)TileType.IRON];
                        break;
                    }
                case TileType.DIAMONT:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(rnd.Next(0, 3), 7, rnd.Next(0, 2) == 0);
                        settings = TileSettings.tilesettings[(int)TileType.DIAMONT];
                        break;
                    }
                case TileType.CRAFTTABEL:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(3, 0, false);
                        settings = TileSettings.tilesettings[(int)TileType.CRAFTTABEL];
                        inventar_types = new TileType[17];
                        inventar_count = new int[17];
                        break;
                    }
                case TileType.FURNACE:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(3,1,false);
                        settings = TileSettings.tilesettings[(int)TileType.FURNACE];
                        inventar_types = new TileType[3];
                        inventar_count = new int[3];
                        break;
                    }
                case TileType.CHEAST:
                    {
                        tile_rectangle.Texture = content.tile;
                        tile_rectangle.TextureRect = get_rect(3, 4, false);
                        settings = TileSettings.tilesettings[(int)TileType.CHEAST];
                        inventar_types = new TileType[32];
                        inventar_count = new int[32];
                        break;
                    }
                    /*case TileType.:
                        {
                            tile_rectangle.Texture = content.;
                            tile_rectangle.TextureRect = get_rect(, );
                             settings = TileSettings.tilesettings[(int)TileType.];
                            break;
                        }*/
            }


        }

        public void Update()
        {
            if (type == TileType.GRASS)
            {
                if (
                    world.GetTile_world(
                                        (int)(chunk_poz.Y * Chunk.chunk_size - 1 + this.Position.Y / Tile.tile_size),
                                        (int)(chunk_poz.X * Chunk.chunk_size + this.Position.X / Tile.tile_size)) != null &&
                    !world.GetTile_world(
                                        (int)(chunk_poz.Y * Chunk.chunk_size - 1 + this.Position.Y / Tile.tile_size),
                                        (int)(chunk_poz.X * Chunk.chunk_size + this.Position.X / Tile.tile_size)).settings.CanWallk)

                {
                    world.SetTile_world(TileType.DIRT,
                                        (int)(chunk_poz.Y * Chunk.chunk_size + this.Position.Y / Tile.tile_size),
                                        (int)(chunk_poz.X * Chunk.chunk_size + this.Position.X / Tile.tile_size));
                }

                bool lt = true, rt = true;

                if (
                    world.GetTile_world(
                                        (int)(chunk_poz.Y * Chunk.chunk_size + this.Position.Y / Tile.tile_size),
                                        (int)(chunk_poz.X * Chunk.chunk_size - 1 + this.Position.X / Tile.tile_size)) != null &&
                    world.GetTile_world(
                                        (int)(chunk_poz.Y * Chunk.chunk_size + this.Position.Y / Tile.tile_size),
                                        (int)(chunk_poz.X * Chunk.chunk_size - 1 + this.Position.X / Tile.tile_size)).settings.CanWallk)

                {
                    lt = false;
                }
                if (
                    world.GetTile_world(
                                        (int)(chunk_poz.Y * Chunk.chunk_size + this.Position.Y / Tile.tile_size),
                                        (int)(chunk_poz.X * Chunk.chunk_size + 1 + this.Position.X / Tile.tile_size)) != null &&
                    world.GetTile_world(
                                        (int)(chunk_poz.Y * Chunk.chunk_size + this.Position.Y / Tile.tile_size),
                                        (int)(chunk_poz.X * Chunk.chunk_size + 1 + this.Position.X / Tile.tile_size)).settings.CanWallk)

                {
                    rt = false;
                }

                if (lt && rt) { tile_rectangle.TextureRect = get_rect(0, 0, false); }
                else
                if (lt && !rt) { tile_rectangle.TextureRect = get_rect(1, 0, false); }
                else
                if (!lt && rt) { tile_rectangle.TextureRect = get_rect(1, 0, true); }
                else
                if (!lt && !rt) { tile_rectangle.TextureRect = get_rect(2, 0, false); }




            }
            else if (type == TileType.FURNACE)
            {
             //   Furnace();
             if(inventar_types[1]!=TileType.COAL)
                    tile_rectangle.TextureRect = get_rect(3, 1, false);
             else
                    tile_rectangle.TextureRect = get_rect(3, 2, false);
            }
        }

      
 

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(tile_rectangle, states);
        }



    }
}


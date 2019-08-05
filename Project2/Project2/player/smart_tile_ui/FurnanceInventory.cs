using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class FurnanceInventory : Transformable, Drawable
    {
      
        public const int invent_size = 3;

        public TileType[] inventar_cell_type = new TileType[invent_size];
        public int[] inventar_cell_count = new int[invent_size];
      

        public bool visibal = false;

        public float inventory_ui_size = 2f;



        RectangleShape inventory_rectangle;
        RectangleShape inventory_icon_rectangle;
        Text inventory_icon_index;
        Font font = content.font;

        IntRect get_rect(int x, int y)
        {
            return new IntRect(x * Tile.tile_size, y * Tile.tile_size, Tile.tile_size, Tile.tile_size);
        }
        public IntRect get_icon_rect(TileType type)
        {
            switch (type)
            {
                case TileType.DIRT:
                    {
                        return get_rect(0, 0);

                    }
                case TileType.STOUN:
                    {
                        return get_rect(1, 0);

                    }
                case TileType.GRASS:
                    {
                        return get_rect(0, 0);

                    }
                case TileType.PLATE:
                    {
                        return get_rect(2, 0);

                    }
                case TileType.WOOD:
                    {
                        return get_rect(3, 0);

                    }
                case TileType.DIAMONT:
                    {
                        return get_rect(4, 0);

                    }
                case TileType.IRON:
                    {
                        return get_rect(5, 0);

                    }
                case TileType.GOLD:
                    {
                        return get_rect(6, 0);

                    }
                case TileType.COAL:
                    {
                        return get_rect(7, 0);

                    }
                case TileType.CRAFTTABEL:
                    {
                        return get_rect(8, 0);

                    }
                case TileType.FURNACE:
                    {
                        return get_rect(9, 0);

                    }
            }
            return get_rect(0, 0);

        }
        public FurnanceInventory()
        {


            inventory_rectangle = new RectangleShape(new SFML.System.Vector2f(Tile.tile_size * inventory_ui_size, Tile.tile_size * inventory_ui_size));
            inventory_rectangle.Texture = content.inventory;
            inventory_rectangle.TextureRect = get_rect(0, 0);

            inventory_icon_rectangle = new RectangleShape(new SFML.System.Vector2f(Tile.tile_size * inventory_ui_size, Tile.tile_size * inventory_ui_size));
            inventory_icon_rectangle.Texture = content.icon;
            inventory_icon_rectangle.TextureRect = get_rect(0, 0);


        }
        public int add(TileType type, int CountInStack, int cell, int count)
        {

            if (type == inventar_cell_type[cell])
            {
                if (CountInStack >= inventar_cell_count[cell] + count)
                {
                    inventar_cell_count[cell] += count;
                    save();
                    return 0;
                }
                else
                {
                    count -= (CountInStack - inventar_cell_count[cell]);
                    inventar_cell_count[cell] = CountInStack;
                    save();
                    return count;
                }
            }
            else if (inventar_cell_type[cell] == TileType.AIR)
            {
                inventar_cell_count[cell] = count;
                inventar_cell_type[cell] = type;
                save();
                return 0;
            }
            save();
            return -1;
        }
        Tile mytile;
        public void load(Tile tile)
        {
            mytile = tile;
            for (int i = 0; i < invent_size; i++)
            {
                inventar_cell_type[i] = tile.inventar_types[i];
                inventar_cell_count[i] = tile.inventar_count[i];
            }
        }
        public void save()
        {
            for (int i = 0; i < invent_size; i++)
            {
                mytile.inventar_types[i] =inventar_cell_type[i] ;
                mytile.inventar_count[i]=inventar_cell_count[i];
            }
        }
        public void Draw(RenderTarget target, RenderStates states)
        {

            states.Transform *= Transform;

            float ui_size = Tile.tile_size * inventory_ui_size * Core.game_view.Size.X / 1000;
            float full_inventory_poz_x = Core.game_view.Size.X / 2 - Inventory.small_invent_size  * ui_size;
            float full_inventory_poz_y = Core.game_view.Size.Y / 2 - invent_size / Inventory.small_invent_size * ui_size - ui_size - 1;

            inventory_rectangle.Size = new SFML.System.Vector2f(ui_size, ui_size);
            inventory_icon_rectangle.Size = new SFML.System.Vector2f(ui_size, ui_size);
        

            ////////////////////////////////////////////////////////////////////////////////////////////
            inventory_rectangle.Position = new Vector2f(full_inventory_poz_x, full_inventory_poz_y);
            inventory_icon_rectangle.Position = new Vector2f(full_inventory_poz_x, full_inventory_poz_y);
            
            inventory_icon_index = new Text(inventar_cell_count[0].ToString(), font);

            inventory_icon_index.Color = Color.Black;
            inventory_icon_index.Scale = new SFML.System.Vector2f(ui_size / 40, ui_size / 40);

            inventory_icon_index.Position = new Vector2f(full_inventory_poz_x + ui_size / 8, full_inventory_poz_y);

            inventory_icon_rectangle.TextureRect = get_icon_rect(inventar_cell_type[0]);



                        target.Draw(inventory_rectangle, states);

                        if (inventar_cell_type[0] != TileType.AIR)
                            target.Draw(inventory_icon_rectangle, states);
                        if (inventar_cell_count[0] != 0)
                            target.Draw(inventory_icon_index, states);

            ////////////////////////////////////////////////////////////////////////////////////////////
            inventory_rectangle.Position = new Vector2f(full_inventory_poz_x, (float)(full_inventory_poz_y + ui_size * 1.2));
            inventory_icon_rectangle.Position = new Vector2f(full_inventory_poz_x, (float)(full_inventory_poz_y+ui_size*1.2));

            inventory_icon_index = new Text(inventar_cell_count[1].ToString(), font);

            inventory_icon_index.Color = Color.Black;
            inventory_icon_index.Scale = new SFML.System.Vector2f(ui_size / 40, ui_size / 40);

            inventory_icon_index.Position = new Vector2f(full_inventory_poz_x + ui_size / 8, (float)(full_inventory_poz_y + ui_size * 1.2));
          

            inventory_icon_rectangle.TextureRect = get_icon_rect(inventar_cell_type[1]);



            target.Draw(inventory_rectangle, states);

            if (inventar_cell_type[1] != TileType.AIR)
                target.Draw(inventory_icon_rectangle, states);
            if (inventar_cell_count[1] != 0)
                target.Draw(inventory_icon_index, states);


            ////////////////////////////////////////////////////////////////////////////////////////////
            inventory_rectangle.Position = new Vector2f((float)(full_inventory_poz_x + ui_size * 1.6), (float)(full_inventory_poz_y + ui_size * 0.6));
            inventory_icon_rectangle.Position = new Vector2f((float)(full_inventory_poz_x + ui_size * 1.6), (float)(full_inventory_poz_y + ui_size * 0.6));

            inventory_icon_index = new Text(inventar_cell_count[2].ToString(), font);

            inventory_icon_index.Color = Color.Black;
            inventory_icon_index.Scale = new SFML.System.Vector2f(ui_size / 40, ui_size / 40);

            inventory_icon_index.Position = new Vector2f((float)(full_inventory_poz_x + ui_size * 1.6 + ui_size / 8), (float)(full_inventory_poz_y + ui_size * 0.6));

            inventory_icon_rectangle.TextureRect = get_icon_rect(inventar_cell_type[2]);



            target.Draw(inventory_rectangle, states);

            if (inventar_cell_type[2] != TileType.AIR)
                target.Draw(inventory_icon_rectangle, states);
            if (inventar_cell_count[2] != 0)
                target.Draw(inventory_icon_index, states);





        }
       
    }
}

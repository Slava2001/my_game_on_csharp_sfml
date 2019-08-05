using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Project2
{
    class Inventory : Transformable, Drawable
    {
        public const int small_invent_size = 8;
        public const int invent_size = small_invent_size*5;

       public TileType[] inventar_cell_type = new TileType[invent_size];
       public int[] inventar_cell_count = new int[invent_size];
        public int now_cell_ch = 0;

        public bool full_invent_ui_visibal = false;

        public float inventory_ui_size=2f;

        

        RectangleShape inventory_rectangle;
        RectangleShape inventory_icon_rectangle;
        RectangleShape inventory_cursor_rectangle;
        Text inventory_icon_index;

        Font font = content.font;

        public Inventory()
        {

          

            
            inventory_rectangle = new RectangleShape(new SFML.System.Vector2f(Tile.tile_size * inventory_ui_size, Tile.tile_size * inventory_ui_size));
            inventory_rectangle.Texture = content.inventory;
            inventory_rectangle.TextureRect = get_rect(0, 0);

            inventory_icon_rectangle = new RectangleShape(new SFML.System.Vector2f(Tile.tile_size * inventory_ui_size, Tile.tile_size * inventory_ui_size));
            inventory_icon_rectangle.Texture = content.icon;
            inventory_icon_rectangle.TextureRect = get_rect(0, 0);

            inventory_cursor_rectangle = new RectangleShape(new SFML.System.Vector2f(Tile.tile_size * inventory_ui_size, Tile.tile_size * inventory_ui_size));
            inventory_cursor_rectangle.Texture = content.inventory;
            inventory_cursor_rectangle.TextureRect = get_rect(1, 0);





        }
        public bool add(TileType type, int CountInStack,int Count)
        {
            bool find = false;

            for (int i = 0; i < invent_size; i++)
                if (inventar_cell_type[i] == type && inventar_cell_count[i]+Count < CountInStack)
                {
                    inventar_cell_count[i]+=Count;
                    find = true;
                    break;
                }
            if (!find)
            {
                for (int i = 0; i < invent_size; i++)
                    if (inventar_cell_count[i] == 0)
                    {
                        inventar_cell_count[i] += Count;
                        inventar_cell_type[i] = type;

                        find = true;
                        break;
                    }
            }
            else if (!find)
            {
                for (int i = 0; i < invent_size; i++)
                    if (inventar_cell_type[i] == type)
                    {
                        inventar_cell_count[i] =CountInStack;
                        inventar_cell_type[i] = type;

                        find = true;
                        break;
                    }
            }
            //здесь: что делать если инвентарь полон
            return find;
        }
        public bool add(TileType type,int CountInStack)
        {
            bool find = false;

           

        
            for (int i = 0; i < invent_size; i++)
                if (inventar_cell_type[i] == type && inventar_cell_count[i]< CountInStack)
                {
                    inventar_cell_count[i]++;
                    find = true;
                    break;
                }
            if (!find)
            {
                for (int i = 0; i < invent_size; i++)
                    if (inventar_cell_count[i] == 0)
                    {
                        inventar_cell_count[i]++;
                        inventar_cell_type[i] = type;
              
                        find = true;                      
                        break;
                    }
            }
            //здесь: что делать если инвентарь полон
            return find;
        }
        public int add(TileType type, int CountInStack,int cell,int count)
        {
           
           if (type == inventar_cell_type[cell])
            {
                if (CountInStack >= inventar_cell_count[cell] + count)
                {
                    inventar_cell_count[cell] += count;
                return 0;
                }
                else
                {
                    count -= (CountInStack - inventar_cell_count[cell]);
                    inventar_cell_count[cell] = CountInStack;
                    return count;
                }
            }
            else if (inventar_cell_type[cell]==TileType.AIR)
            {
                inventar_cell_count[cell] = count;
                inventar_cell_type[cell] = type;
                return 0;
            }
                return -1;     
        }
        public string Save()
        {
            string inventory = "";
            inventory += '{';

            for (int i = 0; i < invent_size; i++)
            {
                inventory += (char)inventar_cell_type[i];
                inventory += (char)inventar_cell_count[i];
            }
            inventory += '}';
            return inventory;
        }
        public void Load(string inv)
        {

            for (int i = 1; i < invent_size*2; i+=2)
            {
                inventar_cell_type[i/2] = (TileType)inv[i];
                inventar_cell_count[i/2] = (int)inv[i+1];
            }
        }
        public TileType get()
        {
            if (inventar_cell_count[invent_size - (small_invent_size - now_cell_ch)] != 0)
            {
                inventar_cell_count[invent_size - (small_invent_size - now_cell_ch)]--;

                if (inventar_cell_count[invent_size - (small_invent_size - now_cell_ch)] == 0)
                {
                    TileType type = inventar_cell_type[invent_size - (small_invent_size - now_cell_ch)];
                    inventar_cell_type[invent_size - (small_invent_size - now_cell_ch)] = TileType.AIR;
                    return type;
                }
                else
                {
                    return inventar_cell_type[invent_size - (small_invent_size - now_cell_ch)];
                }
                
            }
            else 
            {
                return TileType.AIR;
            }
        }
        public void cell_ch(float dir)
        {
            if (dir > 0)
             {
                 now_cell_ch++;
             }
             else
             {
                 now_cell_ch--;
             }


             if (now_cell_ch >= small_invent_size)
             {
                 now_cell_ch = 0;
             }
             if (now_cell_ch <0)
             {
                 now_cell_ch = small_invent_size - 1;
             }
        }
        IntRect get_rect(int x, int y )
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
                case TileType.CHEAST:
                    {
                        return get_rect(10, 0);

                    }
            }
            return get_rect(0, 0);

        }
        public void Draw(RenderTarget target, RenderStates states)
        {
           
            states.Transform *= Transform;

           float ui_size= Tile.tile_size * inventory_ui_size *Core.game_view.Size.X / 1000;
           float full_inventory_poz_x =Core.game_view.Size.X / 2 - small_invent_size / 2 * ui_size;

            inventory_rectangle.Size =new SFML.System.Vector2f( ui_size,  ui_size);
            inventory_icon_rectangle.Size = new SFML.System.Vector2f( ui_size, ui_size);
            inventory_cursor_rectangle.Size = new SFML.System.Vector2f( ui_size,  ui_size);

            for (int i = 0; i < small_invent_size; i++)
            {
                inventory_rectangle.Position = new Vector2f(full_inventory_poz_x + i*ui_size, Core.game_view.Size.Y -  ui_size - ui_size / 10 * 2);
                inventory_icon_rectangle.Position = new Vector2f(full_inventory_poz_x + i * ui_size, Core.game_view.Size.Y -  ui_size - ui_size / 10 * 2);
                inventory_cursor_rectangle.Position = new Vector2f(full_inventory_poz_x + now_cell_ch * ui_size, Core.game_view.Size.Y - ui_size -ui_size/10*2);

                inventory_icon_index =new Text(inventar_cell_count[invent_size - (small_invent_size - i)].ToString(), font);
                inventory_icon_index.Color = Color.Black;
                inventory_icon_index.Scale = new SFML.System.Vector2f(ui_size/40, ui_size / 40);
                
                inventory_icon_index.Position=new Vector2f(i * ui_size + full_inventory_poz_x +ui_size/8, Core.game_view.Size.Y - ui_size - ui_size / 10 * 2);


                inventory_icon_rectangle.TextureRect = get_icon_rect(inventar_cell_type[invent_size - (small_invent_size - i)]);


                target.Draw(inventory_rectangle, states);
                if (inventar_cell_type[invent_size - (small_invent_size - i)] != TileType.AIR)
                target.Draw(inventory_icon_rectangle, states);
                if(inventar_cell_count[invent_size - (small_invent_size - i)] !=0)
                target.Draw(inventory_icon_index, states);
                target.Draw(inventory_cursor_rectangle, states);
                
            }

            if (full_invent_ui_visibal)
            {

                var rec = new RectangleShape(new SFML.System.Vector2f(Core.game_view.Size.X, Core.game_view.Size.Y));
                rec.Position = new SFML.System.Vector2f(0, 0);
                rec.Texture = content.inventory;
                rec.TextureRect = get_rect(0, 1);
                target.Draw(rec, states);


                float full_inventory_poz_y =Core.game_view.Size.Y / 2 - invent_size/small_invent_size / 2 * ui_size- ui_size-1;
               
                for (int y = 0; y < invent_size/small_invent_size; y++)
                {
                    if (y == (invent_size / small_invent_size - 1)) full_inventory_poz_y += inventory_ui_size;// small_in_ofset

                    for (int x = 0; x < small_invent_size; x++)
                    {

                        

                        inventory_rectangle.Position = new Vector2f(x*ui_size+ full_inventory_poz_x, y*ui_size+ full_inventory_poz_y);

                        inventory_icon_rectangle.Position = new Vector2f(x * ui_size + full_inventory_poz_x, y * ui_size + full_inventory_poz_y);



                        inventory_icon_index = new Text(inventar_cell_count[y * small_invent_size + x].ToString(), font);
                        inventory_icon_index.Color = Color.Black;
                        inventory_icon_index.Scale = new SFML.System.Vector2f(ui_size / 40, ui_size / 40);

                        inventory_icon_index.Position = new Vector2f(x * ui_size + full_inventory_poz_x + ui_size / 8, full_inventory_poz_y+ ui_size*y);


                        inventory_icon_rectangle.TextureRect = get_icon_rect(inventar_cell_type[small_invent_size*y+x]);



                        target.Draw(inventory_rectangle, states);
                        if (inventar_cell_type[y * small_invent_size + x] != TileType.AIR)
                            target.Draw(inventory_icon_rectangle, states);
                        if (inventar_cell_count[y * small_invent_size + x] != 0)
                            target.Draw(inventory_icon_index, states);
                        target.Draw(inventory_cursor_rectangle, states);
                    }
                }
            }
        }
    }
}

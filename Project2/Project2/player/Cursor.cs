using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class Cursor : Transformable, Drawable
    {
        Player player;
        World world;

        Vector2f cursor_poz;
        Vector2f cursor_poz_last;
        RectangleShape cursor_rectangle;

      
        Tile now_tile;

        Inventory inventory;

        RectangleShape cursor_icon_rectangle;
        Text cursor_icon_index;
        TileType cursor_icon_type=TileType.AIR;
        int cursor_icon_count = 0;

        public Cursor(Player player,World world)
        {
            cursor_icon_rectangle = new RectangleShape(cursor_poz);
            cursor_icon_rectangle.Texture = content.icon;

            Core.window.SetMouseCursorVisible(true);
            this.world = world;
            this.player = player;
            cursor_rectangle = new RectangleShape(new SFML.System.Vector2f(Tile.tile_size,Tile.tile_size));
            cursor_rectangle.FillColor = Color.Transparent;
            cursor_rectangle.OutlineColor = Color.Blue;
            cursor_rectangle.OutlineThickness = 1;
            inventory = new Inventory();
        }
        float BreakState = 1;
        bool now_keyE_isprs = false;
        bool now_mouse_isprs = false;
        public void Update()
        {
            float deltatime = Core.deltatime;

            inventory.Position = Core.game_view.Center - new Vector2f(Core.game_view.Size.X / 2, Core.game_view.Size.Y / 2);

            cursor_poz = (Vector2f)Mouse.GetPosition(Core.window);//очень
            cursor_poz.X *= Core.game_view.Size.X / Core.window.Size.X;//сложная
            cursor_poz.Y *= Core.game_view.Size.Y / Core.window.Size.Y;//магия

            cursor_poz += Core.game_view.Center - Core.game_view.Size / 2;

            if (cursor_icon_count < 1) { cursor_icon_count = 0;cursor_icon_type = TileType.AIR; }

            


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (!inventory.full_invent_ui_visibal)
            {

                cursor_poz.X -= cursor_poz.X % (float)Tile.tile_size;
                cursor_poz.Y -= cursor_poz.Y % (float)Tile.tile_size;

                cursor_poz /= (float)Tile.tile_size;

                cursor_rectangle.Position = cursor_poz * (float)Tile.tile_size;

                now_tile = world.GetTile_world((int)cursor_poz.Y, (int)cursor_poz.X);

                int x = (int)(player.player_poz.X / Tile.tile_size);
                x -= (int)cursor_poz.X;
                int y = (int)(player.player_poz.Y / Tile.tile_size);
                y -= (int)cursor_poz.Y;

                Debug.Add(0, 3, "смешение курсора x: " + x.ToString());
                Debug.Add(0, 4, "смешение курсора y: " + y.ToString());
                Debug.Add(0, 5, "смешение курсора R: " + (x * x + y * y).ToString());

                if ((x * x + y * y) <81 && !Collision())
                {

                    if (now_tile != null)
                    {

                        if (now_tile.settings.CanBreak)
                        {
                            Debug.Add(cursor_poz.X * Tile.tile_size, cursor_poz.Y * Tile.tile_size, Tile.tile_size, Tile.tile_size, Color.Green);

                            if (Mouse.IsButtonPressed(Mouse.Button.Left))
                            {
                                BreakState += deltatime;
                                if (cursor_poz != cursor_poz_last)
                                {
                                    BreakState = 1;
                                    cursor_poz_last = cursor_poz;
                                }


                                if (BreakState > now_tile.settings.TimeBreak)
                                {
                                    BreakState = 1;


                                    //////////////////////////////обработка сломанных блоков
                                    if (TileSettings.tilesettings[(int)now_tile.type].drop_type == TileType.AIR)
                                    {
                                        world.SetTile_world(TileType.AIR, (int)cursor_poz.Y, (int)cursor_poz.X);
                                    }
                                    else if (inventory.add(TileSettings.tilesettings[(int)now_tile.type].drop_type, now_tile.settings.CountInStack))
                                    {
                                        world.SetTile_world(TileType.AIR, (int)cursor_poz.Y, (int)cursor_poz.X);
                                    }


                                }
                                cursor_rectangle.OutlineThickness = -(BreakState / world.GetTile_world((int)cursor_poz.Y, (int)cursor_poz.X).settings.TimeBreak) * (Tile.tile_size / 2); //магия которая заменяется функцией map из arduino языка

                            }
                            else
                            {
                                BreakState = 1;
                                cursor_rectangle.OutlineThickness = -3;
                            }

                        }               
                        else if (now_tile.settings.CanBuild)
                        {
                            if (Mouse.IsButtonPressed(Mouse.Button.Right))
                            {
                                TileType type;
                                type = inventory.get();
                                if (type != TileType.AIR)
                                {
                                    world.SetTile_world(type, (int)cursor_poz.Y, (int)cursor_poz.X);
                                }
                            }
                            BreakState = 1;
                            cursor_rectangle.OutlineThickness = -3;
                        }
                        else
                        {
                            Debug.Add(cursor_poz.X * Tile.tile_size, cursor_poz.Y * Tile.tile_size, Tile.tile_size, Tile.tile_size, Color.Red);
                        }




                    }
                    else
                    {
                        Debug.Add(cursor_poz.X * Tile.tile_size, cursor_poz.Y * Tile.tile_size, Tile.tile_size, Tile.tile_size, Color.Cyan);
                    }

                }
                else
                {
                    cursor_rectangle.OutlineThickness = -1;
                }

            }
            else
            {
                cursor_icon_rectangle.Position = cursor_poz;

                if (Mouse.IsButtonPressed(Mouse.Button.Left) )
                {
                    if (!now_mouse_isprs)
                    {
                        now_mouse_isprs = true;
                        int inv_ret = 0;
                        int poz_cell = Inventory_cell_collision();
                        if (poz_cell != -1)
                        {


                            inv_ret = inventory.add(cursor_icon_type, TileSettings.tilesettings[(int)TileType.DIRT].CountInStack, poz_cell, cursor_icon_count);

                            if (inv_ret == 0)
                            {
                                cursor_icon_type = TileType.AIR;
                                cursor_icon_count = 0;
                            }
                            else if (inv_ret == -1)
                            {
                                TileType type = inventory.inventar_cell_type[poz_cell];
                                int count = inventory.inventar_cell_count[poz_cell];

                                inventory.inventar_cell_type[poz_cell] = cursor_icon_type;
                                inventory.inventar_cell_count[poz_cell] = cursor_icon_count;

                                cursor_icon_type = type;
                                cursor_icon_count = count;
                            }
                            else
                            {
                                cursor_icon_count = inv_ret;
                            }

                        }
                    }
                }
                else if (Mouse.IsButtonPressed(Mouse.Button.Right))
                {
                    if (!now_mouse_isprs)
                    {
                        now_mouse_isprs = true;
                        int poz_cell = Inventory_cell_collision();
                        if (poz_cell != -1)
                        {
                            if (cursor_icon_type == TileType.AIR && inventory.inventar_cell_count[poz_cell] > 1)
                            {
                                cursor_icon_type = inventory.inventar_cell_type[poz_cell];
                                cursor_icon_count = inventory.inventar_cell_count[poz_cell] / 2;

                                inventory.inventar_cell_count[poz_cell] = inventory.inventar_cell_count[poz_cell] / 2 + inventory.inventar_cell_count[poz_cell] % 2;
                            }else
                            if(inventory.inventar_cell_count[poz_cell]<TileSettings.tilesettings[(int)inventory.inventar_cell_type[poz_cell]].CountInStack&& cursor_icon_type != TileType.AIR && (inventory.inventar_cell_type[poz_cell]==cursor_icon_type || inventory.inventar_cell_type[poz_cell]==TileType.AIR))
                            {
                                if(inventory.inventar_cell_type[poz_cell]==TileType.AIR)
                                inventory.inventar_cell_type[poz_cell] = cursor_icon_type;
                                inventory.inventar_cell_count[poz_cell]++;
                                    cursor_icon_count--;
                            

                            }
                        }
                    }
                }
                else { now_mouse_isprs = false; }
            }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (cell_ch_dir != 0)
                {
                    inventory.cell_ch(cell_ch_dir);
                    cell_ch_dir = 0;
                }

            cursor_icon_rectangle.TextureRect = inventory.get_icon_rect(cursor_icon_type);

                if (Keyboard.IsKeyPressed(Keyboard.Key.E))
                {
                    if (now_keyE_isprs)
                    {
                        inventory.full_invent_ui_visibal = !inventory.full_invent_ui_visibal;
                        Core.window.SetMouseCursorVisible(inventory.full_invent_ui_visibal);
                    if (!inventory.full_invent_ui_visibal)
                    {
                        inventory.add(cursor_icon_type, TileSettings.tilesettings[(int)cursor_icon_type].CountInStack, cursor_icon_count);
                        cursor_icon_count = 0;
                    }
                        now_keyE_isprs = false;
                    }
                }
                else
                {
                    now_keyE_isprs = true;
                }

            
        }
        bool Collision()
        {
            for (int y = (int)(player.player_poz.Y / Tile.tile_size); y < (player.player_poz.Y + player.player_y) / Tile.tile_size; y++)
                for (int x = (int)(player.player_poz.X / Tile.tile_size); x < (player.player_poz.X + player.player_x) / Tile.tile_size; x++)
                {
                    if (cursor_poz==new SFML.System.Vector2f(x,y))
                    {
                        return true;                     
                    }            
                }
            return false;
        }
        int Inventory_cell_collision()
        {
            float ui_size = Tile.tile_size *inventory.inventory_ui_size * Core.game_view.Size.X / 1000;
            float full_inventory_poz_x = Core.game_view.Size.X / 2 - Inventory.small_invent_size / 2 * ui_size;
            float full_inventory_poz_y = Core.game_view.Size.Y / 2 - Inventory.invent_size / Inventory.small_invent_size / 2 * ui_size - ui_size - 1;

            

           
            Debug.Add(full_inventory_poz_x, full_inventory_poz_y, Inventory.small_invent_size*ui_size, (Inventory.invent_size /Inventory.small_invent_size-1) * ui_size, Color.Yellow);
            Debug.Add(full_inventory_poz_x, full_inventory_poz_y+(Inventory.invent_size / Inventory.small_invent_size-1) * ui_size+inventory.inventory_ui_size, Inventory.small_invent_size * ui_size, ui_size, Color.Yellow);



            cursor_poz = (Vector2f)Mouse.GetPosition(Core.window);
            cursor_poz.X *= Core.game_view.Size.X / Core.window.Size.X;
            cursor_poz.Y *= Core.game_view.Size.Y / Core.window.Size.Y;
            Debug.Add(cursor_poz.X, cursor_poz.Y, 2, 2, Color.Cyan);

            cursor_poz.X -= full_inventory_poz_x;
            cursor_poz.Y -= full_inventory_poz_y;


            if (cursor_poz.X < 0||cursor_poz.X>Inventory.small_invent_size*ui_size)
                return -1;


            if ((cursor_poz.Y <= (Inventory.invent_size / Inventory.small_invent_size - 1)*ui_size)&& (cursor_poz.Y >= 0))
            {
                cursor_poz = (Vector2f)((Vector2i)(cursor_poz / ui_size));

                Debug.Add(0, 10, "in ceeel x: " + cursor_poz.X.ToString());
                Debug.Add(0, 11, "in ceeel y: " + cursor_poz.Y.ToString());
               
                return (int)(cursor_poz.Y * Inventory.small_invent_size+cursor_poz.X);
            }
            else if((cursor_poz.Y >= (Inventory.invent_size / Inventory.small_invent_size - 1) * ui_size+ inventory.inventory_ui_size)&&(cursor_poz.Y< (Inventory.invent_size / Inventory.small_invent_size - 1) * ui_size + inventory.inventory_ui_size+ui_size))
            {
                cursor_poz.Y -= (Inventory.invent_size / Inventory.small_invent_size - 1) * ui_size + inventory.inventory_ui_size;

                cursor_poz = (Vector2f)((Vector2i)(cursor_poz / ui_size));

                Debug.Add(0, 10, "in cel x: " + cursor_poz.X.ToString());
                Debug.Add(0, 11, "in cel y: " + (cursor_poz.Y+Inventory.invent_size/Inventory.small_invent_size-1).ToString());
               
                return (int)((cursor_poz.Y+Inventory.invent_size / Inventory.small_invent_size - 1) *Inventory.small_invent_size + cursor_poz.X);
            }


        

            return -1;
        }
        public static float cell_ch_dir = 0;
        public static void win_scroll(object sender, EventArgs e)
        {
            SFML.Window.MouseWheelScrollEventArgs mouseEvent = (SFML.Window.MouseWheelScrollEventArgs)e;

            
            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
               
                Core.game_view = new View(new FloatRect(0, 0, Core.game_view.Size.X+ (Core.game_view.Size.X / 100)*mouseEvent.Delta, Core.game_view.Size.Y + (Core.game_view.Size.Y/100)*mouseEvent.Delta ));
            }
            else
            {
                cell_ch_dir = -mouseEvent.Delta;
            }
               
        }
        public string Save()
        {
            return inventory.Save();
        }
        public void Load(string inv)
        {
            inventory.Load(inv);
        }
        Font font = content.font;
        IntRect get_rect(int x, int y)
        {
            return new IntRect(x * Tile.tile_size, y * Tile.tile_size, Tile.tile_size, Tile.tile_size);
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(cursor_rectangle, states);
            target.Draw(inventory);

            if (inventory.full_invent_ui_visibal&&cursor_icon_type!=TileType.AIR)
            {
                Core.window.SetMouseCursorVisible(false);
                float ui_size = Tile.tile_size * inventory.inventory_ui_size * Core.game_view.Size.X / 1000;

                cursor_icon_rectangle.Position = new Vector2f(cursor_icon_rectangle.Position.X-ui_size/2, cursor_icon_rectangle.Position.Y - ui_size / 2);


                cursor_icon_rectangle.Size = new Vector2f(ui_size, ui_size);

                target.Draw(cursor_icon_rectangle);

                if (cursor_icon_count != 0)
                {


                    cursor_icon_index = new Text(cursor_icon_count.ToString(), font);
                    cursor_icon_index.Color = Color.Black;
                    cursor_icon_index.Scale = new SFML.System.Vector2f(ui_size / 40, ui_size / 40);

                    cursor_icon_index.Position = new Vector2f(cursor_icon_rectangle.Position.X + ui_size / 8, cursor_icon_rectangle.Position.Y);
                    target.Draw(cursor_icon_index);
                }
            }else Core.window.SetMouseCursorVisible(true);

        }
    }
}

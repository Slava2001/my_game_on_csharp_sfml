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
    class Item : Transformable, Drawable
    {
        public int item_y = Tile.tile_size;
        public int item_x = Tile.tile_size;

        Vector2f item_acl = new Vector2f(0.5f, 0.1f);

        float item_spd_jmp = 4f;
        float item_max_spd = 3f;




        public Vector2f item_poz;

        Vector2f item_spd;

        bool item_onGround = false;


        RectangleShape item_rec;

        World world;
        IntRect get_rect(int x, int y)
        {
            return new IntRect(x * Tile.tile_size, y * Tile.tile_size, Tile.tile_size, Tile.tile_size);
        }
        public Item(World world,float x,float y,float acs_x,float acs_y)
        {
            this.world = world;

         

            
            item_poz.Y = x;
            item_poz.X = x;
            item_rec = new RectangleShape(new SFML.System.Vector2f(item_x, item_y));
            item_rec.Position = new SFML.System.Vector2f(item_poz.X, item_poz.Y);
            item_rec.Texture = content.icon;
            item_rec.TextureRect = get_rect(0, 0);



        }

        public void Update()
        {
            physic_update();
        }

        void physic_update()
        {



            float deltatime = Core.deltatime < 2 ? Core.deltatime : 2;


            if (item_onGround && Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                item_onGround = false;
                item_spd.Y -= item_spd_jmp;

            }
            else
            {
                item_spd.Y += item_acl.Y * deltatime;
              
            }
            Debug.Add(0, 10, "onGround " + item_onGround.ToString());

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                if (item_spd.X > -item_max_spd)
                    item_spd.X -= item_acl.X * deltatime;

              
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                if (item_spd.X < item_max_spd)
                    item_spd.X += item_acl.X * deltatime;

               
            }
            else
            {
                if (item_spd.X > 0.1)
                {
                    item_spd.X -= item_acl.X * deltatime;
                }
                else if (item_spd.X < -0.1)
                {
                    item_spd.X += item_acl.X * deltatime;
                }
                else
                {
                    item_spd.X = 0;
                }
            }


            item_poz.Y += item_spd.Y * deltatime;
            Collision(1);
            item_poz.X += item_spd.X * deltatime;
            Collision(0);



            item_rec.Position = item_poz;
            Core.game_view.Center = item_poz + new Vector2f(item_x / 2, item_y / 2);
        }

        void Collision(int dir)
        {
            for (int y = (int)(item_poz.Y / Tile.tile_size); y < (item_poz.Y + item_y) / Tile.tile_size; y++)
                for (int x = (int)(item_poz.X / Tile.tile_size); x < (item_poz.X + item_x) / Tile.tile_size; x++)
                {
                    if (world.GetTile_world(y, x) != null && !world.GetTile_world(y, x).settings.CanWallk)
                    {
                        Debug.Add(x * Tile.tile_size, y * Tile.tile_size, Tile.tile_size, Tile.tile_size, Color.Red);
                        if (dir == 0) //collision X
                        {
                            if (item_spd.X > 0)
                            {
                                item_poz.X = (x * Tile.tile_size) - item_x;
                            }
                            else
                            {
                                item_poz.X = (x * Tile.tile_size) + Tile.tile_size;
                            }
                            item_spd.X = 0;
                        }
                        else          //collision Y
                        {
                            if (item_spd.Y > 0)
                            {
                                item_poz.Y = (y * Tile.tile_size) - item_y;
                                item_onGround = true;
                            }
                            else if (item_spd.Y < 0)
                            {
                                item_poz.Y = (y * Tile.tile_size) + Tile.tile_size;
                            }
                            item_spd.Y = 0;
                        }
                    }
                    else Debug.Add(x * Tile.tile_size, y * Tile.tile_size, Tile.tile_size, Tile.tile_size, Color.Green);
                }
        }

   




        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(item_rec);
        }

    }

}

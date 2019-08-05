using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class Player: Transformable,Drawable
    {
        public int gamemode = 1;

        public int player_y = 44;
        public int player_x = 16;

        Vector2f player_acl = new Vector2f(0.5f, 0.1f);

        float player_spd_jmp = 4f;
        float player_max_spd = 3f;




       public Vector2f player_poz;

        Vector2f player_spd;

        bool player_onGround = false;


        RectangleShape player_rec;

        World world;
        Cursor cursor;

        animator anim;

        TextReader datafileR;
        string wn;
        string pn;
        public Player(World world,string wn,string pn)
        {
            this.world = world;
            this.wn = wn;
            this.pn = pn;
            cursor = new Cursor(this,world);
            anim = new animator(5, 16, 44);


            try
            {
                datafileR = new StreamReader("maps/" + wn + "/players/"+pn+"_"+"player_date.txt");
                Load();
            }
            catch
            {
                Directory.CreateDirectory("maps/" + wn+ "/players");
                spawn(0, -50);
            }

        }

        public void Update()
        {
            physic_update();
            cursor.Update();
            Debug.Add(0, 0, "игрок x: " + player_poz.X / Tile.tile_size);
            Debug.Add(0, 1, "игрок y: " + player_poz.Y / Tile.tile_size);
        }

        void physic_update()
        {

            float deltatime = Core.deltatime < 2 ? Core.deltatime : 2;

            if (gamemode == 0)
            {


                if (player_onGround && Keyboard.IsKeyPressed(Keyboard.Key.W))
                {
                    player_onGround = false;
                    player_spd.Y -= player_spd_jmp;

                }
                else
                {
                    player_spd.Y += player_acl.Y * deltatime;
                    if (!player_onGround)
                        player_rec.TextureRect = anim.Update(2);
                }
                Debug.Add(0, 10, "onGround " + player_onGround.ToString());

                if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                {
                    if (player_spd.X > -player_max_spd)
                        player_spd.X -= player_acl.X * deltatime;

                    player_rec.TextureRect = anim.Update(1);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    if (player_spd.X < player_max_spd)
                        player_spd.X += player_acl.X * deltatime;

                    player_rec.TextureRect = anim.Update(-1);
                }
                else
                {
                    if (player_spd.X > 0.1)
                    {
                        player_spd.X -= player_acl.X * deltatime;
                    }
                    else if (player_spd.X < -0.1)
                    {
                        player_spd.X += player_acl.X * deltatime;
                    }
                    else
                    {
                        player_spd.X = 0;
                    }
                    player_rec.TextureRect = anim.Update(0);
                }


                player_poz.Y += player_spd.Y * deltatime;
                player_onGround = false;
                Collision(1);
                player_poz.X += player_spd.X * deltatime;
                Collision(0);

            }
            else if (gamemode == 1)
            {


                if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                {
                    if (player_spd.Y > -player_max_spd)
                        player_spd.Y -= player_acl.X * deltatime;
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    if (player_spd.Y < player_max_spd)
                        player_spd.Y += player_acl.X * deltatime;
                }
                else
                {
                    player_spd.Y = 0;
                }
              
                if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                {
                    if (player_spd.X > -player_max_spd)
                        player_spd.X -= player_acl.X * deltatime;
                    player_rec.TextureRect = anim.GetFrame(1); 
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    if (player_spd.X < player_max_spd)
                        player_spd.X += player_acl.X * deltatime;
                    player_rec.TextureRect = anim.GetFrame(-1);
                }
                else
                {                 
                        player_spd.X = 0;
                }


                player_poz.Y += player_spd.Y * deltatime;
                player_poz.X += player_spd.X * deltatime;
               
            }

            player_rec.Position = player_poz;
            Core.game_view.Center = player_poz+ new Vector2f(player_x/2,player_y/2);
        }

        void Collision(int dir)
        {
            for (int y = (int)(player_poz.Y / Tile.tile_size); y < (player_poz.Y + player_y) / Tile.tile_size; y++)
                for (int x = (int)(player_poz.X / Tile.tile_size); x < (player_poz.X + player_x) / Tile.tile_size; x++)
                {
                    if (world.GetTile_world(y, x)!=null && !world.GetTile_world(y,x).settings.CanWallk)
                    {
                        Debug.Add(x * Tile.tile_size, y * Tile.tile_size, Tile.tile_size, Tile.tile_size, Color.Red);
                        if (dir == 0) //collision X
                        {
                            if (player_spd.X > 0)
                            {
                                player_poz.X =  (x * Tile.tile_size) - player_x;
                            }
                            else
                            {
                                player_poz.X =  (x * Tile.tile_size)+Tile.tile_size;
                            }
                            player_spd.X = 0;
                        }
                        else          //collision Y
                        {
     
                            if (player_spd.Y > 0)
                            {
                                player_poz.Y = (y * Tile.tile_size) - player_y;
                                player_onGround = true;
                            }
                            else if(player_spd.Y<0)
                            {
                                player_poz.Y =  (y * Tile.tile_size)+Tile.tile_size;
                            }
                            player_spd.Y = 0;
                        }
                    }else Debug.Add(x * Tile.tile_size, y * Tile.tile_size, Tile.tile_size, Tile.tile_size, Color.Green);
                }
        }





        public void spawn(float x,float y)
        {
            player_poz.Y = y;
            player_poz.X = x;
            player_rec = new RectangleShape(new SFML.System.Vector2f(player_x, player_y));
            player_rec.Position = new SFML.System.Vector2f(player_poz.X, player_poz.Y);
            player_rec.Texture = content.player;
            player_rec.TextureRect = new IntRect(0, 0, 16, 44);
        }



        public void Save()
        {
            string player_poz_str = "";
            player_poz_str += '(';
            player_poz_str += ((int)player_poz.X).ToString();
            player_poz_str += ';';
            player_poz_str += ((int)player_poz.Y).ToString();
            player_poz_str += ')';
            player_poz_str += cursor.Save();

            TextWriter datafileW = new StreamWriter("maps/" + wn + "/players/" + pn + "_" + "player_date.txt");
            // написать строку текста в файл
            datafileW.WriteLine(player_poz_str);

            // закрыть поток
            datafileW.Close();


        }

        public void Load()
        {
            string playerDt = datafileR.ReadToEnd();
            datafileR.Close();

            string buffer="";
            int i;

            for (i = 1; playerDt[i] != ';'; i++) buffer += playerDt[i];
            player_poz.X = Convert.ToInt32(buffer);
            buffer = "";
            i++;
            for (; playerDt[i] != ')'; i++) buffer += playerDt[i];
            player_poz.Y = Convert.ToInt32(buffer);
            buffer = "";
            spawn(player_poz.X, player_poz.Y);
            i++;
            cursor.Load(playerDt.Remove(0,i));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(player_rec);
            target.Draw(cursor);
        }
    }
}

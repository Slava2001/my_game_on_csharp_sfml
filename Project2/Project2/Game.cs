using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class game
    {

        World world;
        Player player;

        bool isprst = false;
        public static bool domenuopen = false;

        List<Button> minimenu = new List<Button>();

        public game(string WorldName,string PlayerName,int seed)
        {
            WaitingScreen();
            
            world = new World(WorldName,seed);
            player = new Player(world,WorldName,PlayerName);
            createbuton();


        }
      
       public void Update() 
        {

            if (!domenuopen)
            {
                world.Update((SFML.System.Vector2i)player.player_poz / Tile.tile_size / Chunk.chunk_size);
                player.Update();
            }
            else
            {
                foreach (Button bt in minimenu)
                    bt.Udpate();
            }

            



            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                if (!isprst)
                {
                    domenuopen = !domenuopen;
                    isprst = true;
                }
            }
            else
            {
                isprst = false;
            }


        }
       void WaitingScreen()
        {
            var rec = new SFML.Graphics.RectangleShape(new SFML.System.Vector2f(Core.game_view.Size.X, Core.game_view.Size.Y) * 2);
            rec.Position = Core.game_view.Center - Core.game_view.Size;
            rec.Texture = content.waitingScreeen;
      

            Core.window.Draw(rec);

            Core.window.Display();
        }

        public void Draw()
        {

            Core.window.Draw(world);
            Core.window.Draw(player);
            Debug.Draw(Core.window);
            if(domenuopen)
            foreach (Button bt in minimenu)
            {
                bt.move(Core.game_view.Center.X, Core.game_view.Center.Y);
                    bt.Udpate();
                    Core.window.Draw(bt);
            }
           

        }
 public void createbuton()
        {
            float sizeX = /*Core.menu_view.Size.X * (Core.game_view.Size.X / Core.menu_view.Size.X) + */player.Position.X;
            float sizeY = /*Core.menu_view.Size.Y * (Core.game_view.Size.Y / Core.menu_view.Size.Y) + */player.Position.Y;

            minimenu.Clear();

            Button bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2 - 25, 20, 8, "Resume", resume);
            minimenu.Add(bt);
            bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2, 20, 8, "Save", save);
            minimenu.Add(bt);
            bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2 + 25, 20, 8, "Save & Exit", exit);
            minimenu.Add(bt);

           

        }
        void resume()     
        {
            domenuopen = false;
        }
        void save()       
        {
        WaitingScreen();
        world.Save();
        player.Save();
    }
        void exit()       
        {
            WaitingScreen();
            world.Save();
            player.Save();
            Core.gameIsReady = false;
            Core.window.SetMouseCursorVisible(true);
            Core.game_view = Core.menu_view;
            Core.game_view.Center = new SFML.System.Vector2f(Core.menu_view.Size.X / 2, Core.menu_view.Size.Y / 2);
            Core.menu = new Menu();
        }
    }
}

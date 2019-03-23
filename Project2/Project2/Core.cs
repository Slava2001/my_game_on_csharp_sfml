using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project2
{
    class Core
    {


       
        public static RenderWindow window { get; set; }
        public static game game {  set; get; }

        public static Menu menu { set; get; }

        public static View menu_view { private set; get; }

        public static View game_view { set; get; }

        public static float deltatime { set; get; }

        public static bool gameIsReady = false;

       
        static void Main()
        {
            menu_view= new View(new FloatRect(0, 0, 800, 600));//new View(new FloatRect(0, 0, 400f, 300f));
            game_view = menu_view;

            window = new RenderWindow(new SFML.Window.VideoMode(800, 600), "GAME!!");
            window.SetView(game_view);
            window.SetVerticalSyncEnabled(true);
            window.SetActive();


            window.Closed += win_close;
            window.Resized += win_rezize;
            window.MouseWheelScrolled +=Cursor.win_scroll;
           


            content.load();//ВАЖНОООО
            TileSettings.load();

           
            menu = new Menu();


            Clock clk = new Clock();
            while (window.IsOpen)
            {
                deltatime = clk.Restart().AsSeconds()*65;//65- game speed
                Debug.Add(0, 15, "deltatime= " + deltatime.ToString());
                window.DispatchEvents();

                if (gameIsReady)
                    game.Update();
                else
                    menu.Update();

                window.Clear(Color.Black);
                window.SetView(game_view);
                ///draw////
                if (gameIsReady)
                    game.Draw();
                else
                    menu.Draw();







                //////////
                window.Display();
            }
        }
        private static void win_close(object sender, EventArgs e) { menu.saveworldlistdata(); window.Close(); }

        private static void win_rezize(object sender, SFML.Window.SizeEventArgs e) { if (gameIsReady) game_view = new View(new FloatRect(0, 0, e.Width / 2, e.Height / 2)); }

        

    }
}
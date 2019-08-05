using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Project2
{

    delegate void ButtonFunktion();
    delegate void ButtonFunktionStr(string s);
    class Menu 
    {
         List<Button>[] buttons_rec = new List<Button>[5];
        Textbox playernametextbox;
        Textbox seedtextbox;
        Textbox worldnametextbox;

        int menu_states = 0;
        //states
        //0 - main
        //1 - sg
        //2 - settings
        //3 - player name
        //4 - create world
        //
        



        string NowChangeWorld = "name";
        string PlayerName = "Slava";
        int seed;
        string worldlist ="";
        string worldlistindirectory;
        TextReader datafileR;
        int WorldListOffset = 0;

        public Menu()
        {
            

            datafileR = new StreamReader("world_list_data.txt");
            worldlist = datafileR.ReadLine();
            datafileR.Close();

            DirectoryInfo dir = new DirectoryInfo("maps/");
            foreach (var item in dir.GetDirectories())
            {
                if (worldlist.IndexOf(item.ToString()) == -1)
                    worldlist += item.ToString() + ';';

                worldlistindirectory += item.ToString() + ';';
            }
            

            float sizeX = Core.menu_view.Size.X * (Core.game_view.Size.X / Core.menu_view.Size.X);
            float sizeY = Core.menu_view.Size.Y * (Core.game_view.Size.Y / Core.menu_view.Size.Y);


            for (int i = 0; i < 5; i++)
                buttons_rec[i] = new List<Button>();


           


            Button bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2 - 25, 20,8, "Single game", singgame);
            buttons_rec[0].Add(bt);
                   bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2, 20,8, "Settings", setiings);
            buttons_rec[0].Add(bt);
                   bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2 +25, 20,8, "Exit", exit);
            buttons_rec[0].Add(bt);







            recreatebuttons();







                   bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2 - 25, 20,8, "Player name", changeplayername);
            buttons_rec[2].Add(bt);
                   bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2, 20,8, "Debag Mode", changedebagmode);
            buttons_rec[2].Add(bt);
                   bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2 + 25, 20,8, "Back", gomain);
            buttons_rec[2].Add(bt);



            playernametextbox = new Textbox(sizeX / 2 - 20 * 4, sizeY / 2 - 25, 20, 8, false);
            
                   bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2, 20, 8, "Back", gosettings);
            buttons_rec[3].Add(bt);


            seedtextbox = new Textbox(sizeX / 2 - 20 * 4, sizeY / 2 - 25, 20, 8, true);
            worldnametextbox= new Textbox(sizeX / 2 - 20 * 4, sizeY / 2 - 50, 20, 8, false);
            worldnametextbox.text = "name";
            bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2, 20, 8, "Play", newgamecreate);
            buttons_rec[4].Add(bt);
            bt = new Button(sizeX / 2 - 20 * 4, sizeY / 2+25, 20, 8, "Back", gosinglgame);
            buttons_rec[4].Add(bt);

          //  gamecreate();
        }

        void createWorldList()
        {


            buttons_rec[1].Clear();

            float sizeX = Core.menu_view.Size.X * (Core.game_view.Size.X / Core.menu_view.Size.X);
            float sizeY = Core.menu_view.Size.Y * (Core.game_view.Size.Y / Core.menu_view.Size.Y);
            Button bt;

                 bt = new Button(sizeX / 2 - 20 * 2 - 90, sizeY - 65, 20, 4, "Play", gamecreate);
            buttons_rec[1].Add(bt);
                 bt = new Button(sizeX / 2 - 20 * 2, sizeY - 65, 20, 4, "Delete", deleteworld);
            buttons_rec[1].Add(bt);
                 bt = new Button(sizeX / 2 - 20 * 2 + 90, sizeY - 65, 20, 4, "Back", gomain);
            buttons_rec[1].Add(bt);


              bt = new Button(sizeX / 2 - 20 * 2 - 90, sizeY - 40, 20, 4, "Create", createnewworld);
          buttons_rec[1].Add(bt);
              bt = new Button(sizeX / 2 - 20 * 2, sizeY - 40, 20, 4, "Move up", moveworldup);
          buttons_rec[1].Add(bt);
               bt = new Button(sizeX / 2 - 20 * 2 + 90, sizeY - 40, 20, 4, "Move dn", moveworldduwn);
          buttons_rec[1].Add(bt);
          
            int butcount = buttons_rec[1].Count;

            string buff = "";
            int line = 0;


            if (worldlistindirectory == null)
            {
                worldlist = "";
            }
            else
            for (int i = 0; i < worldlist.Length; i++)
            {
                if (worldlist[i] == ';')
                {
                    if(worldlistindirectory.IndexOf(buff)==-1)
                    {
                        NowChangeWorld = buff;
                        deleteworld();
                    }
                    else{ 
                        bt = new Button(0, line, 20, 8, buff, changeworld);
                        if (buff == NowChangeWorld)
                            bt.NowChange = true;
                        buttons_rec[1].Add(bt);
                        line += 25;
                        buff = "";
                    }
                }
                else
                    buff += worldlist[i];
            }
            if (buttons_rec[1].Count == butcount) return;

              buttons_rec[1][butcount].Position = new SFML.System.Vector2f(sizeX / 2 - 20 * 4, sizeY / 2 - 25 * 5);
           

            for (int i = butcount+1; i < buttons_rec[1].Count; i++)
            {
                buttons_rec[1][i].move(buttons_rec[1][butcount].Position.X, buttons_rec[1][butcount].Position.Y);
            }
        }

        public void Update()
        {
            bool find = false;
            foreach (Button bt in buttons_rec[menu_states])
            {
                if (menu_states == 1)
                {

                    
                    
                    if (bt.NowChange)
                    {
                        NowChangeWorld = bt.buttunName;
                        find = true;
                    }
                    for (int i = 0; i < 6; i++)
                        buttons_rec[1][i].Udpate();
                    int indezofset = (-WorldListOffset / 25);
                    if (indezofset < 0) indezofset = 0;
                    for (int i =  6+ indezofset; i < (16 + (-WorldListOffset / 25)) && i < buttons_rec[1].Count; i++)
                        buttons_rec[1][i].Udpate();

                }
                else
                bt.Udpate();
            }
            if (!find)
                NowChangeWorld = "";
            if (wantrecreatebuttons)
                recreatebuttons();
            if (menu_states == 3)
                playernametextbox.Udpate();
            if (menu_states == 4)
            { 
                seedtextbox.Udpate();
                worldnametextbox.Udpate();
            }




            Debug.Add(0, 5, NowChangeWorld.ToString());
            Debug.Add(0, 6,"menu states "+menu_states.ToString());


            Debug.Add(0, 8,  "X" + Core.game_view.Center.X.ToString());
            Debug.Add(0, 9,  "Y" + Core.game_view.Center.Y.ToString());
            Debug.Add(0, 10, "W" + Core.game_view.Size.X.ToString());
            Debug.Add(0, 11, "H" + Core.game_view.Size.Y.ToString());
            Debug.Add(0, 12, "worlds: " + worldlist);

           
        }

        public void Draw()
        {
            if (menu_states == 1)
            {
                for(int i=0;i<6;i++)
                    Core.window.Draw(buttons_rec[1][i]);

                int indezofset = (-WorldListOffset / 25);
                if (indezofset < 0) indezofset = 0;
                for (int i = 6 + indezofset; i < (16 + (-WorldListOffset / 25)) && i < buttons_rec[1].Count; i++)
                Core.window.Draw(buttons_rec[1][i]);

            }
            else
            foreach (Button bt in buttons_rec[menu_states])
                Core.window.Draw(bt);
           if(menu_states==3)
                Core.window.Draw(playernametextbox);
            if (menu_states == 4)
            {
                Core.window.Draw(seedtextbox);
                Core.window.Draw(worldnametextbox);
            }
            Debug.Draw(Core.window);
        }
        bool wantrecreatebuttons = false;

        void gamecreate()            
        {
            if (NowChangeWorld == "")
            return;

            if(playernametextbox.text != "")
                PlayerName = playernametextbox.text;


            menu_states = 0;
            Core.gameIsReady = true;
            Core.game = new game(NowChangeWorld, PlayerName, seed);
            Core.window.SetMouseCursorVisible(false);
        }
        void newgamecreate()         
        {
            long sd=0;
            if (seedtextbox.text != "")
              sd = Convert.ToInt64(seedtextbox.text);
            seed = (int)sd;
           
            if (worldnametextbox.text != "")
                if (worldlist.IndexOf(worldnametextbox.text + ';') == -1)
                    NowChangeWorld = worldnametextbox.text;
                else
                {
                    string name = "";

                    for (int i = 1; worldlist.IndexOf(name) != -1; i++)
                        name = worldnametextbox.text + i.ToString();

                    NowChangeWorld = name;

                }
            gamecreate();
        }
        void changeworld(string name)
        {
            NowChangeWorld = name;
        }
        void singgame()              
        {
            menu_states = 1;

        }
        void setiings()              
        {
            menu_states = 2;

        }
        void exit()                  
        {
            Core.window.Close();
        }
        void gomain()                
        {
            menu_states = 0;
        }
        void deleteworld()           
        {
            wantrecreatebuttons = true;
            if (NowChangeWorld == "")
                return;


            string S2 = NowChangeWorld + ';';

            worldlist = worldlist.Replace(S2, "");

            try { Directory.Delete("maps/" + NowChangeWorld.ToString(), true); } catch { }
            NowChangeWorld = "";
        }
        void recreatebuttons()       
        {
            wantrecreatebuttons = false;
            createWorldList();
            saveworldlistdata();
        }
        void changeplayername()      
        {
            menu_states = 3;
        }
        void createnewworld()        
        {
            menu_states = 4;
        }
        void moveworldup()           
        {
            if (NowChangeWorld == "")
                return;

            string S2 = NowChangeWorld + ';';

            int t = worldlist.IndexOf(S2);

            worldlist = worldlist.Replace(S2, "");

           t-=2;      
            for (; t>=0; t--)
            {
                if (worldlist[t] == ';')
                {
                    t++;
                    break;
                }
            }
            
            if (t < 0) t = 0;

            
            

            worldlist = worldlist.Insert(t,S2);



           wantrecreatebuttons = true;


        }
        void moveworldduwn()         
        {
            if (NowChangeWorld == "")
                return;
            string S2 = NowChangeWorld + ';';

            int t = worldlist.IndexOf(S2);

            worldlist = worldlist.Replace(S2, "");

            
            for (; t < worldlist.Length; t++)
            {
              
                if (worldlist[t] == ';')
                {
                    t++;
                    break;
                }
            }
           
         




            worldlist = worldlist.Insert(t, S2);



            wantrecreatebuttons = true;

        }
        void gosettings()            
        {
            menu_states = 2;
        }
        void gosinglgame()           
        {
            menu_states = 1;
        }
        void changedebagmode()
        {
            Debug.DebugMode = !Debug.DebugMode;
        }
        public void saveworldlistdata()     
        {
            TextWriter datafileW = new StreamWriter("world_list_data.txt");
            //datafileW.Write('#');
            
            datafileW.WriteLine(worldlist);
            
            datafileW.Close();
        }
        public void win_scroll(object sender, EventArgs e)
        {
            SFML.Window.MouseWheelScrollEventArgs mouseEvent = (SFML.Window.MouseWheelScrollEventArgs)e;

            WorldListOffset -= (int)mouseEvent.Delta;



            float sizeX = Core.menu_view.Size.X * (Core.game_view.Size.X / Core.menu_view.Size.X);
            float sizeY = Core.menu_view.Size.Y * (Core.game_view.Size.Y / Core.menu_view.Size.Y);
            int butcount = buttons_rec[1].Count-1;


            for(int i=6;i<buttons_rec[1].Count;i++)
            buttons_rec[1][i].Position = new SFML.System.Vector2f(sizeX / 2 - 20 * 4, sizeY / 2 - 25 * 5+WorldListOffset+25*(i-6));





            // mouseEvent.Delta
        }
    }
}



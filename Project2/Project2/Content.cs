using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class content
    {
        public static Texture tile;
        public static Texture player;
        public static Texture inventory;
        public static Texture icon;
        public static Texture waitingScreeen;
        public static Texture menu;
        public static Font font;

        public static void load()
        {
            tile = new Texture("content/tile.png");
            player = new Texture("content/player.png");
            inventory = new Texture("content/inventory.png");
            icon = new Texture("content/icon.png");
            waitingScreeen = new Texture("content/tile.png");
            menu = new Texture("content/menu.png");
            font = new Font("content/font.ttf");


        }

    }
}

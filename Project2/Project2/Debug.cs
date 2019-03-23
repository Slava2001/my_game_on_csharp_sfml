using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class Debug
    {

        public static bool DebugMode = true; 
     

        static List<Drawable> draw_rec = new List<Drawable>();


        public static void Add(float x, float y,Color col)
        {  
            var rec = new RectangleShape(new SFML.System.Vector2f(Tile .tile_size, Tile.tile_size));
            rec.Position = new SFML.System.Vector2f(x*Tile.tile_size, y* Tile.tile_size);
            rec.FillColor = Color.Transparent;
            rec.OutlineColor = col;
            rec.OutlineThickness = -1;
            draw_rec.Add(rec);
        }
        public static void Add(float x, float y, float w, float h, Color col)
        {
            var rec = new RectangleShape(new SFML.System.Vector2f(w, h));
                rec.Position = new SFML.System.Vector2f(x, y);
                rec.FillColor = Color.Transparent;
                rec.OutlineColor = col;
                rec.OutlineThickness = -1;
           draw_rec.Add(rec);
        }
        static Font font = content.font;

        public static void Add(float x, float y,string txt)
        {
            var rec = new Text(txt,font);
            rec.Position = new SFML.System.Vector2f(x,y*30*Core.game_view.Size.Y / 1200) +(SFML.System.Vector2f)Core.game_view.Center-Core.game_view.Size/2;
            rec.Scale = new SFML.System.Vector2f(Core.game_view.Size.X / 2000, Core.game_view.Size.Y / 1200);



            draw_rec.Add(rec);


        }

        public static void Draw(RenderTarget target)
        {
            if(DebugMode)
            foreach (var rec in draw_rec)
            {
                target.Draw(rec);
            }
            draw_rec.Clear();
        }
    }
}

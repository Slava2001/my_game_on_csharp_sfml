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

    class Button : Transformable, Drawable
    {
        RectangleShape button_rec;
        Text but_name;
        public bool NowChange = false;
        public string buttunName;
        bool mouseIsprst = false;
        bool lastmouseIsprst = true;
        Font font = content.font;
        delegate void function();

        ButtonFunktion funktion;
        ButtonFunktionStr funktionStr;

        bool DoImWorldName = false;

        Vector2f null_pozition;

        public Button(float x, float y, int size, int aspect_ratio, string text, ButtonFunktion funk)
        {
            null_pozition = new Vector2f(x, y);


            int h = size;
            int w = size * aspect_ratio;

            funktion = funk;


            button_rec = new RectangleShape(new Vector2f(1, 1));
            button_rec.Texture = content.menu;
            button_rec.TextureRect = new IntRect(0, 0, 620, 80);
            button_rec.Size = new Vector2f(w, h);
            this.Position = new SFML.System.Vector2f(x, y);

            but_name = new Text(text, font, 100);
            but_name.Scale = new SFML.System.Vector2f(0.01f * size, 0.01f * size);
            but_name.Position = new SFML.System.Vector2f((w - but_name.GetGlobalBounds().Width) / 2f, -but_name.GetGlobalBounds().Height / 4f);


        }
        public Button(float x, float y, int size, int aspect_ratio, string text, ButtonFunktionStr funk)
        {
            null_pozition = new Vector2f(x, y);

            DoImWorldName = true;
            int h = size;
            int w = size * aspect_ratio;

            funktionStr = funk;
            buttunName = text;

            button_rec = new RectangleShape(new Vector2f(1, 1));
            button_rec.Texture = content.menu;
            button_rec.TextureRect = new IntRect(0, 0, 620, 80);
            button_rec.Size = new Vector2f(w, h);
            this.Position = new SFML.System.Vector2f(x, y);

            but_name = new Text(text, font, 100);
            but_name.Scale = new SFML.System.Vector2f(0.01f * size, 0.01f * size);
            but_name.Position = new SFML.System.Vector2f((w - but_name.GetGlobalBounds().Width) / 2f, -but_name.GetGlobalBounds().Height / 4f);

            
        }
        Vector2f cursor_poz;
        public void Udpate()
        {
            cursor_poz = (Vector2f)Mouse.GetPosition(Core.window);//очень
            cursor_poz.X *= Core.game_view.Size.X / Core.window.Size.X;//сложная
            cursor_poz.Y *= Core.game_view.Size.Y / Core.window.Size.Y;//магия
            cursor_poz += Core.game_view.Center-Core.game_view.Size/2;


            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (!lastmouseIsprst)
                {
                    lastmouseIsprst = true;
                    mouseIsprst = true;
                }
                else
                {
                    mouseIsprst = false;
                }
            }
            else
            {
                mouseIsprst = false;
                lastmouseIsprst = false;
            }


            if (cursor_poz.X > this.Position.X && cursor_poz.X < (this.Position.X + button_rec.Size.X) && cursor_poz.Y > this.Position.Y && cursor_poz.Y < (this.Position.Y + button_rec.Size.Y))
            {
                button_rec.TextureRect = new IntRect(620, 0, 620, 80);
                if (mouseIsprst)
                {
                    if (DoImWorldName)
                    {
                        funktionStr(buttunName);
                        NowChange = true;
                    }
                    else
                        funktion();
                    //выхов функции
                }
            }
            else
            {
                
                if (!NowChange)
                    button_rec.TextureRect = new IntRect(0, 0, 620, 80);
                if (mouseIsprst) NowChange = false;
            }
            if (NowChange)
                button_rec.TextureRect = new IntRect(620, 0, 620, 80);



        }
        public void move(float x, float y)
        {
            this.Position =null_pozition+ new Vector2f(x,y);
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            target.Draw(button_rec,states);
            target.Draw(but_name, states);
        }
    }
}


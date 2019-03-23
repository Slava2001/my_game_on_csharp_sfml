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

    class Textbox : Transformable, Drawable
    {
        RectangleShape button_rec;
        Text textboxtext;
        public bool NowChange = false;
        public string text="";
        bool mouseIsprst = false;
        bool lastmouseIsprst = true;
        bool onlyNum = false;
        Font font = content.font;
        delegate void function();
        int size;
        int w;


        public Textbox(float x, float y, int size, int aspect_ratio, bool onlyNum)
        {
            this.onlyNum = onlyNum;
            this.size = size;
            int h = size;
            this.w = size * aspect_ratio;




            button_rec = new RectangleShape(new Vector2f(1, 1));
            button_rec.Texture = content.menu;
            button_rec.TextureRect = new IntRect(0, 80, 620, 80);
            button_rec.Size = new Vector2f(w, h);
            this.Position = new SFML.System.Vector2f(x, y);




        }
        Vector2f cursor_poz;
        public void Udpate()
        {
            cursor_poz = (Vector2f)Mouse.GetPosition(Core.window);//очень
            cursor_poz.X *= Core.game_view.Size.X / Core.window.Size.X;//сложная
            cursor_poz.Y *= Core.game_view.Size.Y / Core.window.Size.Y;//магия

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
                button_rec.TextureRect = new IntRect(620, 80, 620, 80);
                if (mouseIsprst)
                {
                    NowChange = true;
                }
            }
            else
            {

                if (!NowChange)
                    button_rec.TextureRect = new IntRect(0,80, 620, 80);
                if (mouseIsprst) NowChange = false;
            }
            if (NowChange)
                button_rec.TextureRect = new IntRect(620, 80, 620, 80);



            if (NowChange)
            {
                char cr= getkey();
                if (cr != lastkey)
                {
                    lastkey = cr;
                    if(cr!='*'&&cr!='&'&& text.Length < 10)
                    text += cr;
                    if (cr == '&'& text.Length>0)
                        text = text.Remove(text.Length - 1, 1);
                }


            }
        }
        char lastkey;

        char getkey()
        {
            Char key=' ';
            if (Keyboard.IsKeyPressed(Keyboard.Key.Num0))
                key = '0';
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num9))
                key = '9';
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num8))
                key = '8';
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num7))
                key = '7';
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num6))
                key = '6';
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num5))
                key = '5';
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num4))
                key = '4';
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num3))
                key = '3';
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
                key = '2';
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
                key = '1';
            else if (Keyboard.IsKeyPressed(Keyboard.Key.BackSpace))
                key = '&';
            else if(onlyNum)
                key = '*';
            else
            if (!onlyNum)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
                    key = 'q';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                    key = 'w';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.E))
                    key = 'e';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                    key = 'r';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.T))
                    key = 't';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Y))
                    key = 'y';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.U))
                    key = 'u';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.I))
                    key = 'i';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.O))
                    key = 'o';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.P))
                    key = 'p';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                    key = 'a';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                    key = 's';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                    key = 'd';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.F))
                    key = 'f';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.G))
                    key = 'g';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.H))
                    key = 'h';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.J))
                    key = 'j';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.K))
                    key = 'k';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.L))
                    key = 'l';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Z))
                    key = 'z';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.X))
                    key = 'x';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.C))
                    key = 'c';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.V))
                    key = 'v';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.B))
                    key = 'b';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.N))
                    key = 'n';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.M))
                    key = 'm';
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    key = ' ';
                else key = '*';
            }
           
           
            string keystr =""+ key; 

            

            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift)) keystr= keystr.ToUpper();
                            
            return keystr[0];
        }
        public void move(float x, float y)
        {
            this.Position = new Vector2f(this.Position.X + x, this.Position.Y + y);
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            textboxtext = new Text(text, font, 100);
            textboxtext.Scale = new SFML.System.Vector2f(0.01f * size, 0.01f * size);
            textboxtext.Position = new SFML.System.Vector2f((w - textboxtext.GetGlobalBounds().Width) / 2f, -textboxtext.GetGlobalBounds().Height / 4f);
            textboxtext.Color = Color.Black;
            target.Draw(button_rec, states);
            target.Draw(textboxtext, states);
        }
    }
}


using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class animator
    {
        float animationSpeed = 10f;

        
        int spriteCount;
        int w;
        int h;
        public animator(int spriteCount,int w,int h)
        {
            this.spriteCount = spriteCount;
            this.w = w;
            this.h = h;
            lastrec = new IntRect(0, 0, w, h);
        }
        //dir
        // 0 -stop
        // 1 -right
        //-1 -left
        // 2 -up
        //-2 -down
        int lastDir = 0;
        float time=0;
        IntRect lastrec;
        int state = 1;
        int look;
        public IntRect Update(int dir)
        {

            Debug.Add(0, 11, "last look:" + look.ToString());

            float deltatime = Core.deltatime;

            time += deltatime;

            if (lastDir != dir)
            {
                state = 1;
                lastDir = dir;
                time = 0;
                if (lastDir ==-1||lastDir==1)
                    look = lastDir;
                switch (lastDir >= 0 ? lastDir : -lastDir)
                {
                    case 0:
                        {
                            lastrec = new IntRect(0, 0, w, h);
                            break;
                        }
                    case 1:
                        {
                            lastrec = new IntRect(state * w, 0, w, h);
                            state++;
                            break;
                        }
                    case 2:
                        {
                            lastrec = new IntRect(w, 0, w, h);
                            break;
                        }
                }
                if (look < 0)
                {
                    lastrec.Width = -w;
                    lastrec.Left += w;
                }
            }
            if (time > animationSpeed)
            {
                time = 0;
                switch (lastDir >= 0 ? lastDir : -lastDir)
                {
                    case 0:
                        {
                            lastrec = new IntRect(0, 0, w, h);
                            break;
                        }
                    case 1:
                        {
                            lastrec = new IntRect(state * w, 0, w, h);
                            state++;
                            break;
                        }
                    case 2:
                        {
                            lastrec = new IntRect(w, 0, w, h);
                            break;
                        }
                }
                if (state > spriteCount - 1) state = 1;
                if (look < 0)
                {
                    lastrec.Width = -w;
                    lastrec.Left += w;
                }
            }

          
            


            return lastrec;
        }
        public IntRect GetFrame(int dir)
        {
            switch (dir)
            {
                case 1:
                    return new IntRect(0, 0, w, h);
                    
                case -1:
                    return new IntRect(w, 0, -w, h);
                    

            }
            return new IntRect(0,0,0,0);
        }
    }
}

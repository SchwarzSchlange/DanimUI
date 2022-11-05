using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace danim
{
    public class Root
    {
        public static Root CurrentRoot = null;
        public List<Page> Pages = new List<Page>();

        public Page CurrentPage = null;

        public Thread RenderThread;
        public int RenderDelay = 100;
        

        public string Title { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Root(string Title,int width, int height,int Renderdelay)
        {
            this.Title = Title;
            this.Width = width;
            this.Height = height;
            this.RenderDelay = Renderdelay;

            Console.Title = Title;
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            
            
            ConsoleWindow.QuickEditMode(false);
            ConsoleWindow.CloseResize();
            //ConsoleWindow.MakeBorderless(Title);

            Console.CursorVisible = false;

            //Color Change
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Clear();
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            //


            CurrentRoot = this;


            
            ConsoleWindow.ListenConsole();
            RenderThread = new Thread(new ThreadStart(RenderLoop));
            RenderThread.Start();


        }

        private void RenderLoop()
        {
            while(true)
            {

                Update();
                Thread.Sleep(RenderDelay);

            }
        }

        public Component CheckPosition(Position position)
        {
            foreach(var comp in CurrentPage.components)
            {

                foreach(var bufferPos in comp.BufferArea)
                {
                  
                    if (position.Equals(bufferPos))
                    {
                        
                        return comp;
                    }
                }
            }
            return null;
        }


        public Component LastAddedComponent = null;
        public Position Auto()
        {
            if(LastAddedComponent != null)
            {
                return new Position(LastAddedComponent.position.x, LastAddedComponent.position.y + 1);
            }
            else
            {
                return new Position(0, 0);
            }
            
        }
  

        public void Write(Position pos,string text)
        {
            try
            {
                Console.SetCursorPosition(pos.x, pos.y);
                Console.Write(text);
            }
            catch
            {
                return;
            }
         
        }
        private void Update()
        {
            if(CurrentPage == null) { return; }
            var x = new List<Component>();

            CurrentPage.components.ForEach((item) =>
            {
                x.Add(item);
            });

            foreach(Component comp in x)
            {
                Write(new Position(comp.position.x, comp.position.y), new string(' ', Root.CurrentRoot.Width));
                Console.ForegroundColor = comp.Color;
                if (comp.ComponentType == typeof(Label) || comp.ComponentType == typeof(ListLabel))
                {
                    Write(comp.position, comp.Text);
                }
                
                else if(comp.ComponentType == typeof(Seperator))
                {
                    Seperator seperator = (Seperator)comp;

                    Write(new Position(1,comp.position.y), new string('█', Root.CurrentRoot.Width-1));
                    
                }
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

    }
}

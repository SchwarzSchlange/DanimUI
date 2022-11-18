using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace danim
{
    public class Root
    {
        public static Root CurrentRoot = null;
        public List<Page> Pages = new List<Page>();

        public Page CurrentPage = null;

        private Thread RenderThread;
        public int RenderDelay = 100;
        

        public string Title { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Root(string Title,int width, int height,int Renderdelay)
        {
            CurrentRoot = this;
            this.Title = Title;
            this.Width = width;
            this.Height = height;
            this.RenderDelay = Renderdelay;

            Console.Title = Title;
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Console.SetWindowSize(width, height);
            //Console.SetBufferSize(width, height);
           
            
            ConsoleWindow.QuickEditMode(false);
            ConsoleWindow.CloseResize();
            //ConsoleWindow.MakeBorderless(Title);

            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            

            Console.CursorVisible = false;
            ConsoleWindow.ListenConsole();
            RenderThread = new Thread(new ThreadStart(RenderLoop));
            RenderThread.Start();




        }

        public bool canUpdate = true;
        private void RenderLoop()
        {
            while(true)
            {
                if(canUpdate)
                {
                    if (CurrentPage == null) { continue; }
                    Update(CurrentPage.components);
                    Thread.Sleep(RenderDelay);
                }
                else
                {
                    Console.Clear();
   
                }


            }
        }

        private int ActiveMessage = 0;
        public async Task<bool> ShowMessage(string title,string content) // MESAJ LAYER : => 30
        {
            bool isComplete = false;

            if(CurrentPage == null) { return false; }

            Box AlertBox = new Box("AlertBox", new Position(0, Height/4), new Size(Width, Height / 2),title);
            AlertBox.Layer = 30+ActiveMessage;
            
            var contentLabel = new Label("content", new Position(1, 2), content);
            var backbtn = new Button("backBtn", new Position(1,1), "x");

            AlertBox.Add(contentLabel,backbtn);
            backbtn.Right();


            backbtn.OnClickEvent +=  (object sender,ConsoleWindow.OnClickEventArgs args)=>{
                CurrentPage.Delete(AlertBox);
                ActiveMessage--;
                isComplete = true;
            };
            CurrentPage.Add(AlertBox);
            ActiveMessage++;

            while(true)
            {
                if (isComplete) { return true; }
                else
                {
                    await Task.Delay(1);
                    continue;
                }
            }

        }


        public Component CheckPosition(List<Component> components,Position position)
        {
            Component Found = null;
            foreach (var comp in components)
            {
                
                comp.BufferArea.ForEach(bufferPos => { if (position.Equals(bufferPos)) { Found = comp; } });


                if (comp.ComponentType == typeof(Box))
                {
                    var box = (Box)comp;
                    var Checked = CheckPosition(box.BoxComponents, position);
                    if (Checked != null) { return Checked; } else { continue; }
                }
  
            }

            return Found;
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
            try{Console.SetCursorPosition(pos.x, pos.y);Console.Write(text);}
            catch{}
        }


        public void ResetComponent(Component comp,bool isBox = false)
        {
            
            if (comp.ComponentType == typeof(Box))
            {

                Box box = (Box)comp;
                for (int i = 0; i <box.Size.Height-1; i++)
                {
                    Write(new Position(comp.position.x, comp.position.y + i), new string(' ', box.Size.Width));
                }


            }
            else
            {
                if (comp.RootBox != null)
                {
                    Write(new Position(comp.position.x, comp.position.y) + comp.RootBox.position, new string(' ',comp.Text.Length));
                }
                else
                {
             
                    Write(new Position(comp.position.x, comp.position.y), new string(' ', comp.Text.Length));
                }

            }

        }

        public void Update(List<Component> components)
        {
            var StaticList = new List<Component>();
            components.ForEach((item) =>
            {
                StaticList.Add(item);
            });
            StaticList.Sort((a,b) => a.Layer.CompareTo(b.Layer));


            foreach(Component comp in StaticList)
            {
                ResetComponent(comp);
           
                Console.ForegroundColor = comp.Color; // CHANGE COLOR


                if (!comp.Visible) { continue; } // VISIBLITY CHECK

                // RENDER COMPONENT
                if (comp.ComponentType == typeof(Label) || comp.ComponentType == typeof(ListLabel))
                {
                    if(comp.Text.Contains(Environment.NewLine))
                    {
                        string[] lines = comp.Text.Split(
                            new string[] { Environment.NewLine },
                            StringSplitOptions.None
                        );

                        if (comp.RootBox != null)
                        {
                            for(int i = 0;i < lines.Length;i++)
                            {
                                Write(comp.position + comp.RootBox.position + new Position(0,i), lines[i]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < lines.Length; i++)
                            {
                                Write(comp.position + new Position(0, i), lines[i]);
                            }
                            Write(comp.position, comp.Text);
                        }
                    }
                    else
                    {
                        if (comp.RootBox != null)
                        {
                            Write(comp.position + comp.RootBox.position, comp.Text);


                            if (comp.AlwaysUpdate)
                            {
                                comp.UpdateBufferArea(0, comp.RootBox.position);
                            }
                        }
                        else
                        {
                            Write(comp.position, comp.Text);


                        }
                    }



                    if (comp.AlwaysUpdate)
                    {
                        comp.UpdateBufferArea();
                    }
                }
                else if(comp.ComponentType == typeof(Seperator))
                {
                    Seperator seperator = (Seperator)comp;

                    if (comp.RootBox != null)
                    {
                        Write(new Position(0, comp.position.y)+comp.RootBox.position, new string('━', Root.CurrentRoot.Width));
                    }
                    else
                    {
                        Write(new Position(0, comp.position.y), new string('━', Root.CurrentRoot.Width));
                    }

                    
                    
                }
                else if(comp.ComponentType == typeof(Input))
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Input input = (Input)comp;

                    if (comp.RootBox != null)
                    {

                        Write(comp.position + comp.RootBox?.position + new Position(input.Text.Length, 0), new string(' ', input.Width));
                        Write(comp.position + comp.RootBox.position, comp.Text);


                        if (comp.AlwaysUpdate)
                        {
                            comp.UpdateBufferArea(input.Width-comp.Text.Length,comp.RootBox.position);
                        }
                    }
                    else
                    {

                        Write(comp.position + new Position(input.Text.Length, 0), new string(' ', input.Width));


                        Write(comp.position, comp.Text);


                        if (comp.AlwaysUpdate)
                        {
                            comp.UpdateBufferArea(input.Width - comp.Text.Length);
                        }
                    }

                }
                else if(comp.ComponentType == typeof(Button))
                {
                    Console.BackgroundColor = ConsoleColor.Blue;

                    if (comp.RootBox != null)
                    {
                        Write(comp.position+comp.RootBox.position, comp.Text);


                        if (comp.AlwaysUpdate)
                        {
                            comp.UpdateBufferArea(0, comp.RootBox.position);
                        }
                    }
                    else
                    {
                        Write(comp.position, comp.Text);


                        if (comp.AlwaysUpdate)
                        {
                            comp.UpdateBufferArea();
                        }
                    }
                   

                }
                else if(comp.ComponentType == typeof(Box))
                {

                    try
                    {
                        var box = (Box)comp;
                        Update(box.BoxComponents);
                        string UpperBorderText = "┏" + box.Title + new string('━', box.Size.Width - 2 - box.Title.Length) + "┓";
                        Write(box.position, UpperBorderText);
                        
                        
                        for (int i = 1; i <= box.Size.Height-2; i++)
                        {
                            Write(new Position(box.position.x, box.position.y + i), "┃");
                            Write(new Position(box.position.x + box.Size.Width - 1, box.position.y + i), "┃");
                        }
                        

                   

                        Write(box.position + new Position(0, box.Size.Height-2), "┗" + new string('━', box.Size.Width-2) + "┛");

               
                        //box.UpdateBufferArea(UpperBorderText.Length);
                    }
                    catch(Exception ex)
                    {
                        Console.Title = ex.Message;
                        continue;
                    }
                }
                else if(comp.ComponentType == typeof(CheckBox))
                {
                    if (comp.RootBox != null)
                    {
                        Write(comp.position + comp.RootBox.position, comp.Text);


                        if (comp.AlwaysUpdate)
                        {
                            comp.UpdateBufferArea(0, comp.RootBox.position);
                        }
                    }
                    else
                    {
                        Write(comp.position, comp.Text);


                        if (comp.AlwaysUpdate)
                        {
                            comp.UpdateBufferArea();
                        }
                    }
                }


                if (!CurrentPage.components.Contains(comp) && comp.RootBox == null) { ResetComponent(comp); }
                //RESET COLOR
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
        }

    }
}

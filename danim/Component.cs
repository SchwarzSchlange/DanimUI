using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace danim
{
    public  class Component
    {
        public Type ComponentType { get; private set; }
        public string Name { get; private set; }
        public string Text { get; set; }
        public Position position { get; set; }
        public ConsoleColor Color { get; set; }


        public List<Position> BufferArea = new List<Position>();

 

 
        public delegate void OnClickEventHandler(object sender, ConsoleWindow.OnClickEventArgs e);

        public event OnClickEventHandler OnClickEvent;


        public static dynamic Find<T>(string name)
        {
            Component find = Root.CurrentRoot.CurrentPage.components.Find(x => x.Name == name);

            if(find == null) { return default(T); }
            return (T)Convert.ChangeType(find, typeof(T));
            

        }

        public Component Center()
        {
            this.position = new Position(Root.CurrentRoot.Width / 2 - this.Text.Length/2, this.position.y);
            return this;
        }
        public Component Bottom()
        {
            this.position = new Position(position.x, Root.CurrentRoot.Height-1);
            return this;
        }




        public Component(string name, Position position, Type componentType,string text,ConsoleColor Color = ConsoleColor.White)
        {
            this.Name = name;
            this.position = position;
            this.Text = text;
            this.ComponentType = componentType;
            this.Color = Color;

            for(int i = 0; i <= text.Length;i++)
            {
                BufferArea.Add(new Position(position.x+i, position.y));
            }

            Root.CurrentRoot.LastAddedComponent = this;
        }

        public void Click(ConsoleWindow.OnClickEventArgs args)
        {
            OnClickEvent?.Invoke(this,args);
        }

        public void Delete()
        {
            Root.CurrentRoot.CurrentPage.components.Remove(this);
        }
    }
}

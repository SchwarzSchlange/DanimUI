using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace danim
{
    public  class Component
    {
        public Type ComponentType { get; private set; }
        public string Name { get; private set; }
        public string Text { get; set; }
        public Position position { get; set; }
        public ConsoleColor Color { get; set; }

        public bool AlwaysUpdate = false;



        public List<Position> BufferArea = new List<Position>();

 

 
        public delegate void OnClickEventHandler(object sender, ConsoleWindow.OnClickEventArgs e);

        public event OnClickEventHandler OnClickEvent;

        public delegate void OnUnFocus();

        public event OnUnFocus UnFocused;


        public static T Find<T>(string name)
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

        public Component Left()
        {
            this.position = new Position(0, position.y);
            return this;
        }

        public Component Right()
        {
            this.position = new Position(Root.CurrentRoot.Width-Text.Length, position.y);
            return this;
        }

        public Component Auto()
        {
            this.position = new Position(Root.CurrentRoot.LastAddedComponent.position.x, Root.CurrentRoot.LastAddedComponent.position.y + 1);
            return this;
        }

        public Component(string name, Position position, Type componentType, string text, bool AlwaysUpdate = false,ConsoleColor Color = ConsoleColor.White)
        {
            this.Name = name;
            this.position = position;
            this.Text = text;
            this.AlwaysUpdate = AlwaysUpdate;
            this.ComponentType = componentType;
            this.Color = Color;

            UpdateBufferArea();
            Root.CurrentRoot.LastAddedComponent = this;
        }
        public void UpdateBufferArea(int Extra = 0)
        {
            BufferArea.Clear();
            for (int i = 0; i <= Text.Length+Extra; i++)
            {
                BufferArea.Add(new Position(position.x + i, position.y));
            }
        }

      

        public void Click(ConsoleWindow.OnClickEventArgs args)
        {
            OnClickEvent?.Invoke(this,args);
        }

        public void Unfocus()
        {
            UnFocused?.Invoke();
        }

        public void Delete()
        {
            Root.CurrentRoot.CurrentPage.components.Remove(this);
        }
    }
}

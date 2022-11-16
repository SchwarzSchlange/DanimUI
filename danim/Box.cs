using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace danim
{
    public class Box : Component
    {
        public Size Size { get; set; }
        public string Title { get; set; }
        public List<Component> BoxComponents = new List<Component>();
        public Box(string name, Position position, Size size, string title) : base(name, position, typeof(Box), "", true, ConsoleColor.White)
        {

            Size = size;
            Title = title;
        }

        public void Add(params Component[] comp)
        {
            foreach(var elem in comp)
            {
                elem.Layer = this.Layer;
                elem.RootBox = this;
                if (elem.ComponentType == typeof(Label)) { elem.SetText(elem.Text,true); }

                BoxComponents.Add(elem);
            }

        }
        public T FindInBox<T>(string name)
        {

            Component find = BoxComponents.Find(x => x.Name == name);

            if (find == null) { return default(T); }
            return (T)Convert.ChangeType(find, typeof(T));

        }

        public void SetSize(Size newSize)
        {
            Root.CurrentRoot.ResetComponent(this,true);
            Size = newSize;

            var labels = BoxComponents.FindAll(comp => comp.ComponentType == typeof(Label));
            labels.ForEach(element => { element.SetText(element.Text, true); });

            Console.Clear();
        }

        public int MaxWidth()
        {
            return Size.Width - 2;
        }

        public void Remove(Component comp)
        {
            comp.RootBox = null;
            Root.CurrentRoot.ResetComponent(comp);
            BoxComponents.Remove(comp);
        }
    }
}

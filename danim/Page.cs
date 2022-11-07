using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace danim
{
    public class Page
    {
        
        public string PageName { get; set; }
        public List<Component> components = new List<Component>();


        public Page(string name)
        {
            PageName = name;
            Root.CurrentRoot.Pages.Add(this);

        }

        public static void Load(string Name)
        {
            Root.CurrentRoot.Pages.Find(x => x.PageName == Name)?.Load();
        }

        public static Page Get(string Name)
        {
            return Root.CurrentRoot.Pages.Find(x => x.PageName == Name);
        }

        public void Load()
        {
            Console.Clear();
            Root.CurrentRoot.CurrentPage = this;
            OnLoadEvent?.Invoke();
            Console.Clear();
        }

        public void Add(Component component)
        {
            components.Add(component);
        }

        public void Delete(Component component)
        {
            components.Remove(component);
        }

        public delegate void OnLoad();
        public event OnLoad OnLoadEvent;

    }
}

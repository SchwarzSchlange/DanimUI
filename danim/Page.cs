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

        public async void Load()
        {
         
            Root.CurrentRoot.canUpdate = false;
            Root.CurrentRoot.CurrentPage = this;
            OnLoadEvent?.Invoke();
            await Task.Delay(100);
            Root.CurrentRoot.canUpdate = true;
        }


        public void Add(params Component[] givencomponents)
        {
            foreach(var comp in givencomponents)
            {
                components.Add(comp);
            }
        }

        public void Delete(Component component)
        {
            components.Remove(component);
            if(component.ComponentType == typeof(Box)) { Root.CurrentRoot.ResetComponent(component,true); }
            else { Root.CurrentRoot.ResetComponent(component); }
            
        }

        public delegate void OnLoad();
        public event OnLoad OnLoadEvent;

    }
}

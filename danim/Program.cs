using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace danim
{
    public class Program
    {
        static void Main(string[] args)
        {
            Root window = new Root("Test Menu", 40,20,100);

            Page MainPage = new Page("File Content Shower");

            Box box = new Box("box", new Position(0, 0), new Size(window.Width,window.Height), "Unknown File");
            Input pathput = new Input("pathput", box.MaxWidth(), new Position(1, 1), true, "Enter File Path...");
            Button ofb = new Button("openfileButton", new Position(1, 2), "Open File");
            Seperator seperator = new Seperator(new Position(1,3));
            Label content = new Label("contentLabel", new Position(1, 4), "");
            box.Add(pathput,seperator,content,ofb);
            ofb.Center();
  


            
        
            ofb.OnClickEvent += (object sender, ConsoleWindow.OnClickEventArgs e) =>
            {
                if(pathput.Text == "") { window.ShowMessage("Path", "Please enter a path");return; }
                if (!File.Exists(pathput.Text)) { window.ShowMessage("Error", $"'{pathput.Text}' doestn't exitsts!");return; }
                box.Title = pathput.Text;
                content.SetText(File.ReadAllText(pathput.Text));
                
            };

  
            MainPage.Add(box,ofb);
            MainPage.Load();

   
        }
    }
}

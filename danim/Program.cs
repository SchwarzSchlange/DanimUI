using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace danim
{
    public class Program
    {
        static void Main(string[] args)
        {
            Root window = new Root("Application", 60,15,10);
            Page MainPage = new Page("main");

            Box Group1 = new Box("g1", new Position(0, 0), new Size(window.Width, window.Height), "Box");


            MainPage.Add(Group1);
            MainPage.Load();
        }
    }
}

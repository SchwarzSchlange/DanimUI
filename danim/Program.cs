using System;
using System.Collections.Generic;
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
            Root window = new Root("Window", 30,10,100);
            //Page Menu
            Page Menu = new Page("menu");
            
            Menu.Add(new Label("headr", new Position(0,0), "Welcome To Window",ConsoleColor.Green).Center());
            Menu.Add(new Seperator(window.Auto()));
            var mainSection = new ListLabel("s_mainmenu", window.Auto(), "Main Menu");
            var exitSection = new ListLabel("s_exit", window.Auto(), "Exit");
            Menu.Add(mainSection);
            Menu.Add(exitSection);

            Menu.Load();

            mainSection.OnClickEvent += (object obj, ConsoleWindow.OnClickEventArgs e) =>
            {
                Page.Load("main");
            };

            exitSection.OnClickEvent += (object obj, ConsoleWindow.OnClickEventArgs e) =>
            {
                Environment.Exit(0);
            };

            //Page Menu

            //Main Menu
            Page MainPage = new Page("main");

            MainPage.Add(new Label("headr", new Position(0, 0), "Main Menu"));
            MainPage.Add(new Seperator(new Position(0, 0)));

            var backSection = new ListLabel("s_back", new Position(0, 1), "Back");

            MainPage.Add(backSection);


            backSection.OnClickEvent += (object obj, ConsoleWindow.OnClickEventArgs e) =>
            {
                Menu.Load();
            };

            //Main Menu
        }

    }
}

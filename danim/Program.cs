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
            Root window = new Root("DanimUI", 50,10,100);

            Page LoginPage = new Page("login");
            LoginPage.Add(new Label("pageHeader", new Position(0, 0), "Login",false,ConsoleColor.DarkYellow).Center());
            LoginPage.Add(new Seperator(new Position(0,1)));
            LoginPage.Add(new Input("passcode", 49, new Position(1,2),true,"Enter the passcode..."));
            var loginBtn = new Button("enter", window.Auto(),"Enter").Center();
            loginBtn.OnClickEvent += LoginBtn_OnClickEvent;
            LoginPage.Add(loginBtn);
            LoginPage.Load();


        }

        private static void LoginBtn_OnClickEvent(object sender, ConsoleWindow.OnClickEventArgs e)
        {

            var inp = Component.Find<Input>("passcode");
            if(inp.Text == "123")
            {
                Root.CurrentRoot.ShowMessage(Page.Get("login"),"Login","Succesfully logined!");
            }
            else
            {
                Root.CurrentRoot.ShowMessage(Page.Get("login"), "Login", "The passcode is wrong try another one...");
            }
        }
    }
}

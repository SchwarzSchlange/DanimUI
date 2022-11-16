using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace danim
{
    public class Input : Component
    {
        public int Width { get; set; }
        public string PlaceHolder { get; set; }
        public int StartIndex = 1;
       

        public Input(string name, int width, Position position,bool UpdateBufferArea = true,string PlaceHolder = "") : base(name, position, typeof(Input), new string(' ',width),UpdateBufferArea, ConsoleColor.Gray)
        {
            this.Width = width;
            this.PlaceHolder = PlaceHolder;
            if(PlaceHolder != "") { SetText(PlaceHolder); }
            
            OnClickEvent += Input_OnClickEvent;
            UnFocused += Input_UnFocused;
        }

        private void Input_UnFocused()
        {
            if(Text.Trim() == "") { SetText(PlaceHolder); }
            this.Color = ConsoleColor.Gray;
        }

        private void Input_OnClickEvent(object sender, ConsoleWindow.OnClickEventArgs e)
        {
            if(Text == PlaceHolder) { SetText(""); }
            this.Color = ConsoleColor.White;

            
        }
    }
}

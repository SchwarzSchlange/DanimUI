using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace danim
{
    public class Button : Component
    {

        public Button(string name, Position position, string text,bool UpdateBufferArea = true, ConsoleColor Color = ConsoleColor.White) : base(name, position, typeof(Button), text,UpdateBufferArea, Color)
        {

        }
    }
}

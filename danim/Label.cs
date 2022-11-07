using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace danim
{
    public class Label : Component
    {

        public Label(string name,Position position, string text,bool UpdateBufferArea = false,ConsoleColor Color = ConsoleColor.White) : base(name, position,typeof(Label),text,UpdateBufferArea,Color)
        {

        }
    }
}

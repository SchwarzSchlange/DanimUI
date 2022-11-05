using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace danim
{
    public class ListLabel : Component
    {

        public ListLabel(string name,Position position, string text,ConsoleColor Color = ConsoleColor.White) : base(name, position,typeof(ListLabel), " • " + text,Color)
        {
            this.Text = $" • " +text;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace danim
{
    public class Seperator : Component
    {


        public Seperator(Position position) : base("sep", position,typeof(Seperator),"SEPERATOR"){}
    }
}

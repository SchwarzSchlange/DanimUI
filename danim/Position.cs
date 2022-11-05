using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace danim
{
    public class Position
    {
       
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Position other = obj as Position;
            if(other.x == this.x && other.y == this.y) { return true; }
            return base.Equals(obj);
        }
   

    }
}

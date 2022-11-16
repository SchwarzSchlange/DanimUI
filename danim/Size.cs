using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace danim
{
    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static Size operator +(Size a, Size b)
        {
            return new Size(a.Width+b.Width, a.Height+b.Height);
        }

        public static Size operator *(Size a, Size b)
        {
            return new Size(a.Width * b.Width, a.Height * b.Height);
        }

        public static Size operator -(Size a, Size b)
        {
            return new Size(a.Width - b.Width, a.Height - b.Height);
        }

        public static Size operator /(Size a, Size b)
        {
            return new Size(a.Width / b.Width, a.Height / b.Height);
        }


    }
}

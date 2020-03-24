using System;

namespace Liskov
{
    public class Rectangle
    {
        //public int Width { get; set; }
        //public int Height { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public Rectangle()
        {
            
        }
        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }
    
    // This is wrong !! since square is derived from Rectangle - we can set a Rectangle r = new Square which will cause bad value
    // Try implement and see the difference (the bad is in comments).
    // Now what you should do is to put virtual in the base class (Rectangle) and override in the derived class
    public class Square : Rectangle
    {
        //public new int Width
        //{
        //    set { base.Width = base.Height = value; }
        //}
        //public new int Height
        //{
        //    set { base.Width = base.Height = value; }
        //}
        public override int Width
        {
            set { base.Width = base.Height = value; }
        }
        public override int Height
        {
            set { base.Width = base.Height = value; }
        }
    }

    class Program
    {
        static public int Area(Rectangle r) => r.Width * r.Height;

        static void Main(string[] args)
        {
            Rectangle rc = new Rectangle(2, 3);
            Console.WriteLine($"{rc} has an area of {Area(rc)}");
            // Now there is a problem when we will want to create a square class that is derived from Rectangle
            // Read the above comments
            Square sq = new Square();
            sq.Width = 4;
            Console.WriteLine($"{sq} has area {Area(sq)}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Course_Work
{
    class Package
    {
        private Product product;
        private int code;
        private Point position;
        private float width;
        private float height;
        private float length;
        public Package(Product p, int w, int h, int l)
        {
            product = p;
            position.X = 0;
            position.Y = 0;
            width = w;
            height = h;
            length = l;
        }
        public Package()
        {
            position.X = 0;
            position.Y = 0;
            width = 1;
            height = 1;
            length = 1;
            product = new Product();
        }
        public Package(string[] fileString)
        {
            int temp = 0;
            product = new Product(fileString);
            if (int.TryParse(fileString[9], out temp))
                position.X = temp;
            else
                throw new PackageException("Ошибка в файле. Невозможно получить позицию пакета");
            if (int.TryParse(fileString[10], out temp))
                position.Y = temp;
            else
                throw new PackageException("Ошибка в файле. Невозможно получить позицию пакета");
            if (!float.TryParse(fileString[11], out width))
                throw new PackageException("Ширина пакета не указана");
            if (!float.TryParse(fileString[12], out height))
                throw new PackageException("Высота пакета не указана");
            if (!float.TryParse(fileString[13], out length))
                throw new PackageException("Длина пакета не указана");
        }
        public int Code { get { return code; } set { code = value; } }
        public Point Position { get { return position; } set { position = value; } }
        public float Width { get { return width; } set { width = value; } }
        public float Height { get { return height; } set { height = value; } }
        public float Length { get { return length; } set { length = value; } }
        public bool CheckPosition(float mx, float my, int mas)
        {
            mx /= mas;
            my /= mas;
            return position.X <= mx && position.X + width >= mx && position.Y <= my && position.Y + length >= my;
        }
        public string ToolString()
        {
            return "Продукт: "+product.Name+Environment.NewLine + "Занимаемая ширина на складе: " + width + Environment.NewLine + "Занимаемая высота на складе: " + height + Environment.NewLine + "Занимаемая длина на складе: " + length + Environment.NewLine;
        }
        public void NewPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }
        public Product Product { get { return product; } set { product = value; } }
        public void Draw(Graphics g)
        {
            g.DrawRectangle(new Pen(Color.Black), position.X, position.Y, width, length);
        }
        public void Draw(Graphics g, int n)
        {
            g.DrawRectangle(new Pen(Color.Black), position.X * n, position.Y * n, width * n, length * n);
        }
        public void DrawFill(Graphics g,int n)
        { 
            g.FillRectangle(new SolidBrush(Color.LawnGreen),position.X*n,position.Y,width*n,length*n);
        }
        public void DrawFill(Graphics g, int n, Color c)
        {
            g.FillRectangle(new SolidBrush(c), position.X * n, position.Y, width * n, length * n);
        }
        public override string ToString()
        {
            return product.ToString() + position.X + ";" + position.Y + ";" + width + ";" + height + ";" + length + ";";
        }
        public string StringToPrint()
        {
            return product.StringToPrint()+"Занимаемая ширина на складе: \t"+ width + Environment.NewLine + "Занимаемая высота на складе: \t" + height + Environment.NewLine+ "Занимаемая длина на складе: \t" + length + "\r\n\r\n\r\n"+ DateTime.Today.ToString("d")+"\r\n"+ "-----------------------------------------------"+Environment.NewLine;
        }
    }
    class PackageException : Exception
    {
        public PackageException(string message) : base(message)
        {
        }
    }
}

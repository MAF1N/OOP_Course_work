using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Course_Work
{
    class Room
    {
        private List<Package> packageList = new List<Package>(); //Список товаров на складе
        private bool[][] positions;//матрица свободных\занятых позиций
        private int roomWidth;
        private int roomLength;
        private int roomHeight;
        private int lastNumber;
        private Package lastDeleted;
        public Room()
        {
            roomWidth = 100;
            roomHeight = 100;
            roomLength = 100;
            lastNumber = 1;
            positions = new bool[100][];
            for (int i = 0; i < 100; i++)
            {
                positions[i] = new bool[100];
                for (int j = 0; j < 100; j++)
                {
                    positions[i][j] = false;
                }
            }
        }
        public Room(int w, int l, int h)
        {
            roomWidth = w;
            roomLength = l;
            roomHeight = h;
            lastNumber = 1;
            positions = new bool[w][];
            for (int i=0; i<w; i++)
            {
                positions[i] = new bool[l];
                for (int j=0; j<l; j++)
                {
                    positions[i][j] = false;
                }
            }
        }
        public int RoomWidth//Свойство ширины комнаты
        {
            get { return roomWidth; }
            set { roomWidth = value; }
        }
        public int RoomLength//Свойство длины комнаты
        {
            get { return roomLength; }
            set { roomLength = value; }
        }
        public int RoomHeight//Свойство высоты комнаты
        {
            get { return roomHeight; }
            set { roomHeight = value; }
        }
        public Package LastDeleted
        {
            get { return lastDeleted; }
        }
        public void AddPackage(Package p)//Добавление товара в список товаров и занесение в матрицу занятых позиций 
        {
            for (int i=0; i<roomWidth-p.Width; i++)
            {
                for (int j=0; j<roomLength-p.Length; j++)
                {
                  
                    if (i+p.Width > roomWidth && j + p.Length > roomLength)
                        throw new RoomException("Комната заполнена, попробуйте перейти в другую комнату");
                    if (isFree(i, j, (int)p.Width + 1, (int)p.Length + 1)) {
                        p.NewPosition(i, j);
                        p.Code = lastNumber;
                        packageList.Add(p);
                        lastNumber++;
                        ocupatePosition(i, j,(int) p.Width+1,(int)p.Length+1);
                        return;
                    }
                }
            }
        }
        private void ocupatePosition(int x, int y, int w, int l)//Метод для отметки занятых позиций начиная с (х,у) шириной w и длиной l
        {
            if (x + w > roomWidth || y + l > roomLength) throw new RoomException("В комнате не может быть размещен данный продукт");
            for (int i = x; i <=x+ w; i++)
            {
                for (int j = y; j <=y+l; j++)
                {
                    positions[i][j] = true;
                }
            }
        }
        private bool isFree(int x, int y, int w, int l)//Метод проверки свободности позиций
        {
            if (x + w > roomWidth || y + l > roomLength) throw new RoomException("В комнате не может быть размещен данный продукт");
            for (int i=x; i<=x+w; i++)
            {
                for (int j=y; j<=y+l; j++)
                {
                    if (positions[i][j] != false)
                        return false;
                }
            }
            return true;
        }
        public Package[] AllProducts()//Метод возращающий массив всех товаров на складе
        {
            return packageList.ToArray();
        }
        public IEnumerable<Package> SearchName(string s)//Метод поиска по имени, возвращает элементы содержащие часть строки в имени 
        {
            for (int i = 0; i < packageList.Count; i++)
            {
                if (packageList[i].Product.Name.Contains(s))
                    yield return packageList[i];
            }
        }
        public IEnumerable<Package> SearchDeliveryman(string s)//Поиск по Доставщику
        {
            for (int i = 0; i < packageList.Count; i++)
            {
                if (packageList[i].Product.ProductProvider.Deliveryman.Contains(s))
                    yield return packageList[i];
            }
        }
        public IEnumerable<Package> SearchCompany(string s)//Поиск по компании поставщику
        {
            for (int i = 0; i < packageList.Count; i++)
            {
                if (packageList[i].Product.ProductProvider.CompanyName.Contains(s))
                    yield return packageList[i];
            }
        }
        public IEnumerable<Package> SearchDate(string s)//Поиск по дате
        {
            for (int i = 0; i < packageList.Count; i++)
            {
                if (packageList[i].Product.CloseDate(s))
                    yield return packageList[i];
            }
        }
        public void View(Graphics g)//Графическое отображение хранилища без масштабирования
        {
            g.DrawRectangle(new Pen(Color.Black), 0, 0, roomWidth, roomLength);
            foreach(Package p in packageList)
            {
                p.Draw(g);
            }
        }
        public void View(Graphics g, int n)//Графическое отображение хранилища с масштабированием
        {
            g.Clear(Color.White);
            g.DrawRectangle(new Pen(Color.Black), 0, 0, roomWidth*n, roomLength*n);
            foreach (Package p in packageList)
            {
                p.Draw(g,n);
            }
        }
        private void ClearPositions(int x,int y,int w,int l)
        {
            if (x + w > roomWidth || y + l > roomLength) throw new RoomException("В комнате не может быть размещен данный продукт");
            for (int i = x; i <= x + w; i++)
            {
                for (int j = y; j <= y + l; j++)
                {
                    positions[i][j] = false;
                }
            }
        }
        public void Delepe(int c)//Удаление Товара c
        {
            if (c < 0) throw new RoomException("Невозможно удалить несуществующее");
            if (c < lastNumber)
            {
                foreach (Package p in packageList)
                    if (p.Code == c)
                    {
                        lastDeleted = p;
                        ClearPositions(p.Position.X, p.Position.Y, (int) p.Width+1, (int)p.Length+1);
                        packageList.Remove(p);
                        return;
                    }               
            }
            else
                throw new RoomException("Такого элемента нету на складе");
        }
        public override string ToString()//Метод для получения строки, которую будет записано в файл-базу
        {
            string rez = "";
            foreach(Package p in packageList)
            {
                rez += p.ToString() + "\r\n";
            }
            return rez;
        }
        public Package MouseOverTheElement(float mx, float my, int n)//Проверка позиции мыши над элементом pictureBox 
        {
            foreach(Package p in packageList)
            {
                if (p.CheckPosition(mx, my, n))
                    return p;
            }
            return null;
        }
        public Package LastAdded//Свойство возвращающее последний добавленный элемент
        {
            get {
                if (packageList.Count!=0) return packageList.Last();
                else throw new RoomException("База данных пуста, попробуйте добавить что-то");
            }
        }
        public string StringToPrintInverntary()//Получение строки для печати приездной накладной 
        {
            string rez="";
            foreach (Package p in packageList)
            {
                rez += p.StringToPrint();
            }
            return rez;
        }

    }
    internal class RoomException : Exception
    {
        public RoomException()
        {
        }

        public RoomException(string message) : base(message)
        {
        }

        public RoomException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RoomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

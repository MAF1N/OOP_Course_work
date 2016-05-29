using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Course_Work
{
    class Product
    {
        private string name;
        private int amount;
        private float cost;
        private string currency;
        private string measure;
        private Provider productProvider;
        private string dateOfIncome;
        private string endDate;
        public Product()
        {
            productProvider = new Provider();
        }
        public Product(string n, int a, int c, string cur, string m)
        {
                name = n;
                amount = a;
                cost = c;
                currency = cur;
                measure = m;
        }
        public Product(string[] fileLine)
        {
            productProvider = new Provider();
            if (fileLine[0] != "")
                name = fileLine[0];
            else
                throw new ProductException("В файле отсутствует поле названия продукта");
            if (!int.TryParse(fileLine[1], out amount))
                throw new ProductException("Ошибка в хранении поля количества");
            if (!float.TryParse(fileLine[2], out cost))
                throw new ProductException("Ошибка в хранении поля числовой части цены");
            if (fileLine[3] == "")
                throw new ProductException("В файле отсутствует поле валюты");
            else
                currency = fileLine[3];
            if (fileLine[4] == "")
                throw new ProductException("В файле отсутствует поле меры измерения");
            else
                measure = fileLine[4];
            if (fileLine[5] == "")
                throw new ProductException("В файле отсутствует имя поставщика");
            else
                productProvider.Deliveryman = fileLine[5];
            if (fileLine[6] == "")
                throw new ProductException("В файле отсутствует фирма поставщик");
            else
                productProvider.CompanyName = fileLine[6];
            if (fileLine[7] == "")
                throw new ProductException("В файле отсутствует дата завоза продукта "+name);
            else
                dateOfIncome = fileLine[7];
            if (fileLine[8] == "")
                throw new ProductException("В файле отсутствует дата вывода продукта "+name);
            else
                endDate= fileLine[8];
        }
        public string Name { get { return name; } set { name = value; } }
        public int Amount { get { return amount; }  set { amount = value; } }
        public float Cost { get { return cost; } set { cost = value; } }
        public string Currency { get { return currency; } set { currency = value; } }
        public string Measure { get { return measure; } set { measure = value; } }
        public Provider ProductProvider { get { return productProvider; } set { productProvider = value; } }
        public string DateOfIncome { get { return dateOfIncome; } set { dateOfIncome = value; } }
        public string EndDate { get { return endDate; } set { endDate = value; } }
        private int[] getIntDate(string s)
        {
            string[] st = s.Split('.');
            int[] rez= { int.Parse(st[0]), int.Parse(st[1]), int.Parse(st[2]) };
            return rez;
        }
        private bool dateIsLess(int[] checkingDate, int[] thisClassDate)
        {
            return checkingDate[2] < thisClassDate[2] || (checkingDate[2] == thisClassDate[2] && checkingDate[1] < thisClassDate[1]) || (checkingDate[2] == thisClassDate[2] && checkingDate[1] == thisClassDate[1] && checkingDate[0] <= thisClassDate[0]);
        }

        public string StringToPrint()
        {
            return "Название продукта:\t\t"+name + "\r\n Количество: \t\t\t" + amount + "\r\n Цена:\t\t\t\t" + cost + "\r\n Валюта:\t\t\t\t" + currency + "\r\n Единица измерения: \t\t" + measure + "\r\n" + productProvider.StringToPrint() + "\r\n Дата завоза товара:\t\t" + dateOfIncome + "\r\n Дата вывоза:\t\t\t" + endDate + "\r\n";
        }

        public bool CloseDate(string d)
        {
            int[] temporary = getIntDate(d);
            return dateIsLess(temporary, getIntDate(endDate)) && dateIsLess(getIntDate(dateOfIncome), temporary);        
        }
        public override string ToString()
        {
            return name + ";" + amount + ";" + cost + ";" + currency + ";" + measure + ";" + productProvider.ToString() + dateOfIncome + ";" + endDate + ";";
        }
    }
    class ProductException : Exception
    {
        public ProductException(string message) : base(message)
        {
        }
    }
}

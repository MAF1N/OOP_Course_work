using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Course_Work
{
    class Provider
    {
        private string deliverymanName;
        private string companyName;
        public Provider()
        {

        }
        public Provider(string d,string cName)
        {
            deliverymanName = d;
            companyName = cName;
        }
        public string CompanyName//Свойство названия компании
        {
            get { return companyName; }
            set { companyName = value; }
        }
        public string Deliveryman//Свойство имени поставщика
        {
            get { return deliverymanName; }
            set { deliverymanName = value; }
        }
        public override string ToString()
        {
            return deliverymanName + ";" + companyName + ";";
        }
        public string StringToPrint()
        {
            return "Поставщик:\t\t\t" + deliverymanName + "\r\nКомпания поставщик:\t" + companyName +"\r\n";
        }
    }
}

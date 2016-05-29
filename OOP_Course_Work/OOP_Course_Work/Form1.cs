using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Course_Work
{
    public partial class Form1 : Form
    {
        DataTable table =new DataTable();
        private Room current;
        public Form1()
        {
            InitializeComponent();
            startDatePicker.Format = DateTimePickerFormat.Custom;
            startDatePicker.CustomFormat = "dd.MM.yyyy";
            endDatePicker.Format = DateTimePickerFormat.Custom;
            endDatePicker.CustomFormat = "dd.MM.yyyy";
            searchDatePicker.Format = DateTimePickerFormat.Custom;
            searchDatePicker.CustomFormat = "dd.MM.yyyy";
            trackBar1.Maximum = 6;
            trackBar1.TickFrequency = 1;
            trackBar1.LargeChange = 3;
            trackBar1.SmallChange = 1;
            trackBar1.Value = 1;
            pictureBox1.BackColor = Color.White;
            current = new Room();
        }
        private Package PackageFromTextBoxes()//Получение информации о продукте из текстовых полей формы
        {
            float valo;
            int val;
            Package pr = new Package();
            //packageName
            if (packageNameBox.Text != null && packageNameBox.Text != "")
            {
                pr.Product.Name = packageNameBox.Text;
            }
            else
                throw new TextBoxesException("Вы не задали название продукта");
            //amount
            if (amountBox.Text != null && amountBox.Text != "")
                if (int.TryParse(amountBox.Text, out val))
                    pr.Product.Amount = val;
                else
                    throw new TextBoxesException("В поле количества введены не коректные данные.");
            else
                throw new TextBoxesException("Содержимое поля количества продуктов пусто. Введите количество продуктов.");
            //cost
            if (costBox.Text != null && costBox.Text != "")
            {
                if (float.TryParse(costBox.Text, out valo))
                    pr.Product.Cost = valo;
                else
                    throw new TextBoxesException("Введенные данные цены не являются числовыми");
            }
            else
                throw new TextBoxesException("Поле цена пустое. Введите цену продукта");
            //currency
            if (currencyBox.Text != null && currencyBox.Text != "")
                pr.Product.Currency = currencyBox.Text;
            else
                throw new TextBoxesException("Валюта не указана");
            //measure
            if (measureBox.Text != null && measureBox.Text != "")
                pr.Product.Measure = measureBox.Text;
            else
                throw new TextBoxesException("Единица измерения не указана");
            //provider`s info
            //deliveryman Name
            if (deliverymanNameBox.Text != null && deliverymanNameBox.Text != "")
                pr.Product.ProductProvider.Deliveryman = deliverymanNameBox.Text;
            else
                throw new TextBoxesException("Доставщик не указан");
            //companyName
            if (companyNameBox.Text != null && companyNameBox.Text != "")
                pr.Product.ProductProvider.CompanyName = companyNameBox.Text;
            else
                throw new TextBoxesException("Компания не указана");
            //date
            checkDate();   //Проверка даты в полях
            pr.Product.DateOfIncome = startDatePicker.Text;
            pr.Product.EndDate = endDatePicker.Text;
            //Width
            if (float.TryParse(packageWidthBox.Text, out valo))
                pr.Width = valo;
            else
                throw new TextBoxesException("Не числовое значение в ширине пакета");
            //Height
            if (float.TryParse(packageHeightBox.Text, out valo))
                pr.Height = valo;
            else
                throw new TextBoxesException("Не числовое значение в высоты пакета");
            //Length
            if (float.TryParse(packageLengthBox.Text, out valo))
                pr.Length = valo;
            else
                throw new TextBoxesException("Не числовое значение в длине пакета");
            return pr;
        }
        private void checkDate()//Проверка даты на форме
        {
            int val;
            string[] parser = startDatePicker.Text.Split('.');
            int[] date1 = new int[3];
            for (int i = 0; i < parser.Length; i++)
                if (Int32.TryParse(parser[i], out val))
                    date1[i] = val;
                else
                    throw new TextBoxesException("Не правильный формат даты завоза продукта");

            //EndDate
            parser = endDatePicker.Text.Split('.');
            int[] date2 = new int[3];
            for (int i = 0; i < parser.Length; i++)
                if (Int32.TryParse(parser[i], out val))
                    date2[i] = val;
                else
                    throw new TextBoxesException("Дата вывоза не правильного формата");
            if (date2[2] < date1[2] || (date1[2] == date2[2] && date1[1] > date2[1]) || (date1[2] == date2[2] && date1[1] == date2[1] && date1[0] > date2[0]))
                throw new TextBoxesException("Дата вывоза должна быть больше чем дата завоза");
        }
        private bool CheckDateFile(string s)//Метод выберает по дате какие данные загружать из файла, а какие нет
        {
            string[] currdate = DateTime.Today.ToString("d").Split('.');
            string[] checkingDate = s.Split('.');
            return (Convert.ToInt32(currdate[2]) < Convert.ToInt32(checkingDate[2])) || (Convert.ToInt32(currdate[2]) == Convert.ToInt32(checkingDate[2]) && Convert.ToInt32(currdate[1]) < Convert.ToInt32(checkingDate[1])) || (Convert.ToInt32(currdate[2]) == Convert.ToInt32(checkingDate[2]) && Convert.ToInt32(currdate[1]) == Convert.ToInt32(checkingDate[1]) && Convert.ToInt32(checkingDate[0]) >= Convert.ToInt32(currdate[0]));
        }
        private Room LoadStorageBase()//Загрузка данных из файла
        {
            Room room = new Room(current.RoomWidth,current.RoomLength,current.RoomHeight);
            using (TextReader reader = new StreamReader("DataBase.txt"))
            {

                string line;
                while ((line = reader.ReadLine()) != null&&line!="")
                {

                    string[] sparse = line.Split(';');
                    if (CheckDateFile(sparse[8]))
                    {
                        room.AddPackage(new Package(sparse));
                    }
                }
                current = room;
                return room;
            }
        }
        private void DisplayPackage(Package p)//Добавление продукта в таблицу для отображения
        {
            DataRow r = table.NewRow();
            r["Code"] = p.Code;
            r["Product"] = p.Product.Name;
            r["Amount"] = p.Product.Amount;
            r["Cost"] = p.Product.Cost;
            r["Currency"] = p.Product.Currency;
            r["Measure"] = p.Product.Measure;
            r["Deliveryman"] = p.Product.ProductProvider.Deliveryman;
            r["Company"] = p.Product.ProductProvider.CompanyName;
            r["Income Date"] = p.Product.DateOfIncome;
            r["End Date"] = p.Product.EndDate;
            table.Rows.Add(r);
        }
        private void ShowProducts(Room Storage1)//Отображение продуктов получаемых массивом
        {
            table.Rows.Clear();
            Package[] ps = Storage1.AllProducts();
            if (table.Columns.Count == 0)
            {
                table.Columns.Add("Code");
                table.Columns.Add("Product");
                table.Columns.Add("Amount");
                table.Columns.Add("Cost");
                table.Columns.Add("Currency");
                table.Columns.Add("Measure");
                table.Columns.Add("Deliveryman");
                table.Columns.Add("Company");
                table.Columns.Add("Income Date");
                table.Columns.Add("End Date");
            }
            for (int i = 0; i < ps.Length; i++)
            {
                DisplayPackage(ps[i]);
            }
            resultBoxDataTable.DataSource = table;
        }
        private void Save(Room s1)//Сохранить в файл данные
        {
            using (TextWriter tw = new StreamWriter("DataBase.txt"))
            {
                tw.Write(s1.ToString());
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void ClearAllTextBoxes()//Очистка всех текстовых полей, например после получения всей информации из них
        {
            packageNameBox.Clear();
            amountBox.Clear();
            costBox.Clear();
            currencyBox.Clear();
            measureBox.Clear();
            deliverymanNameBox.Clear();
            companyNameBox.Clear();
            endDatePicker.ResetText();
            startDatePicker.ResetText();
            packageHeightBox.Clear();
            packageLengthBox.Clear();
            packageWidthBox.Clear();

        }
        private void button1_Click(object sender, EventArgs e)//Добавление продукта в базу данных 
        {
            try
            {
                Package pr = PackageFromTextBoxes();
                current.AddPackage(pr);
                Save(current);
                ShowProducts(current);
                ClearAllTextBoxes();
            }
            catch (TextBoxesException ex)
            {
                MessageBox.Show(ex.Message, "TextBoxesFailure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DataException)
            {
                MessageBox.Show("Попробуйте изменить файл, или проверьте вручную данные в файле на соответсвие типов", "Данные в файле ошибочны", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (PackageException ex)
            {
                MessageBox.Show(ex.Message, "PackageFailure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ProductException ex)
            {
                DialogResult result = MessageBox.Show(ex.Message, "ProductFailure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void resultBoxDataTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void ShowProducts(IEnumerable<Package> p)//метод отображения товаров в Табличном виде в вкладке Search 
        {
            table.Rows.Clear();
            if (table.Columns.Count == 0)
            {
                table.Columns.Add("Code");
                table.Columns.Add("Product");
                table.Columns.Add("Amount");
                table.Columns.Add("Cost");
                table.Columns.Add("Currency");
                table.Columns.Add("Measure");
                table.Columns.Add("Deliveryman");
                table.Columns.Add("Company");
                table.Columns.Add("Income Date");
                table.Columns.Add("End Date");
            }
            foreach (var i in p)
            {
                DisplayPackage(i);
            }
            searchDataGrid.DataSource = table;
        }
        private void searchByNameBox_TextChanged(object sender, EventArgs e)//метод поиска товара по названию продукта
        {
            ShowProducts(current.SearchName(searchByNameBox.Text));   
        }
        private void searchDatePicker_ValueChanged(object sender, EventArgs e)//метод поиска по дате
        {
            ShowProducts(current.SearchDate(searchDatePicker.Text));
        }
        private void searchDeliveryBox_TextChanged(object sender, EventArgs e)//метод поиска по имени поставщика
        {
            ShowProducts(current.SearchDeliveryman(searchDeliveryBox.Text));
        }
        private void searchCompanyBox_TextChanged(object sender, EventArgs e)//метод поиска по названию компании-поставщика
        {
            ShowProducts(current.SearchCompany(searchCompanyBox.Text));
        }
        private void loadBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            current=LoadStorageBase();
            ShowProducts(current);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value == 1)
            {
                Graphics g = pictureBox1.CreateGraphics();
                current.View(g);
            }
        }
        private void resultBoxDataTable_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            current.View(g, trackBar1.Value);
        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                using (TextWriter tw = new StreamWriter("PrintLastIn.txt"))
                {
                    tw.Write(current.LastAdded.StringToPrint());
                }
                printDialog1.AllowSomePages = true;

                printDialog1.ShowHelp = true;

                printDialog1.Document = printDocument1;

                DialogResult result = printDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
            catch (RoomException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void printInventaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (TextWriter tw = new StreamWriter("Inventary.txt"))
            {
                    tw.Write(current.StringToPrintInverntary());
            }
            printDialog1.AllowSomePages = true;

            printDialog1.ShowHelp = true;

            printDialog1.Document = printDocument2;

            DialogResult result = printDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                printDocument2.Print();
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Package p = current.MouseOverTheElement(e.X, e.Y, trackBar1.Value);
            if (p!= null)
            {
                toolTip1.Show("", this);
                toolTip1.ToolTipTitle = "Product";
                toolTip1.ToolTipIcon = ToolTipIcon.Info;
                toolTip1.SetToolTip(pictureBox1, p.ToolString());
            }
            else
            {
                toolTip1.SetToolTip(pictureBox1, "");
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string text = current.StringToPrintInverntary();
            System.Drawing.Font printFont = new System.Drawing.Font("Times New Roman", 14, System.Drawing.FontStyle.Regular);

            // Draw the content.
            e.Graphics.DrawString(text, printFont, System.Drawing.Brushes.Black, 10, 10);
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            toolTip1.Hide(this);
        }
        private Room CreateStorage()// метод создания  хранилища с параметрами из текстбоксов
        {
            int w=0, h=0, l=0;
            if (roomWidthBox.Text != null && roomWidthBox.Text != "")
            {
                if (!int.TryParse(roomWidthBox.Text, out w))
                    throw new TextBoxesException("Поле ширины хранилища указано неверно");
            }
            else
                throw new TextBoxesException("Содержимое поля ширины хранилища отсутствует.");
            if (roomLengthBox.Text != null && roomLengthBox.Text != "")
            {
                if (!int.TryParse(roomLengthBox.Text, out l))
                    throw new TextBoxesException("Поле длины хранилища указано неверно");
            }
            else
                throw new TextBoxesException("Содержимое поля длины хранилища отсутствует.");
            if (roomHeightBox.Text != null && roomHeightBox.Text != "")
            {
                if (!int.TryParse(roomHeightBox.Text, out h))
                    throw new TextBoxesException("Поле высоты хранилища указано неверно");
            }
            else
                throw new TextBoxesException("Содержимое поля высоты хранилища отсутствует.");
            roomHeightBox.Clear();
            roomLengthBox.Clear();
            roomWidthBox.Clear();
            return new Room(w, l, h);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                current = CreateStorage();
                Graphics g = pictureBox2.CreateGraphics();
                current.View(g);
            }
            catch (TextBoxesException ex)
            {
                MessageBox.Show(ex.Message, "TextBoxesFailure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (PackageException ex)
            {
                MessageBox.Show(ex.Message, "PackageFailure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ProductException ex)
            {
                DialogResult result = MessageBox.Show(ex.Message, "ProductFailure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string text = current.LastAdded.StringToPrint();
            System.Drawing.Font printFont = new System.Drawing.Font("Times New Roman", 14, System.Drawing.FontStyle.Regular);

            // Draw the content.
            e.Graphics.DrawString(text, printFont, System.Drawing.Brushes.Black, 10, 10);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int code;
                if (int.TryParse(codeBox.Text, out code))
                    current.Delepe(code);
                else
                    throw new TextBoxesException("В поле Code должен быть указан только целочисленный код товара");
                Save(current);
                ShowProducts(current);
                using (TextWriter tw = new StreamWriter("PrintLastDeleted.txt"))
                {
                    tw.Write(current.LastDeleted.StringToPrint());
                }
                printDialog1.AllowSomePages = true;

                printDialog1.ShowHelp = true;

                printDialog1.Document = printDocument3;

                DialogResult result = printDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    printDocument3.Print();
                }
            }
            catch (TextBoxesException ex)
            {
                MessageBox.Show(ex.Message, "TextBoxFailure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (RoomException ex)
            {
                MessageBox.Show(ex.Message, "RoomFailure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void printDocument3_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string text = current.LastDeleted.StringToPrint();
            System.Drawing.Font printFont = new System.Drawing.Font("Times New Roman", 14, System.Drawing.FontStyle.Regular);

            // Draw the content.
            e.Graphics.DrawString(text, printFont, System.Drawing.Brushes.Black, 10, 10);
        }

    }
    class DataException : Exception
    {

    }
    class TextBoxesException : Exception
    {
        public TextBoxesException()
        {
        }

        public TextBoxesException(string message) : base(message)
        {
        }

        public TextBoxesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TextBoxesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
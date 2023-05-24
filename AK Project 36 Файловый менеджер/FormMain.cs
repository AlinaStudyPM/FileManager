using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.IO.Compression;
using HtmlAgilityPack;
using System.Net;


using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Text.RegularExpressions;

//Ошибка доступа к файлу
//Файл без расширения

//TODO: цвета иконок
//TODO: создать норм пользователей
//TODO: вырезать, разархивировать???

//Изменены ShowPath и NewFolder

//#F3A0A0

namespace AK_Project_36_Файловый_менеджер
{
    public partial class FormMain : Form
    {
        Label TitleLabel;
        ComboBox SearchLine;
        Button SearchButton;
        NumericUpDown NumberOfResults;
        ListBox ResultList;
        ListView ResultsView;
        Button SettingsButton;

        Font GlobalFont;
        User CurrentUser;
        WebClient ItsWebClient;


        Button ReturnButton;
        ListBox leftList;

        

        public FormMain()
        {
            CurrentUser = new User();

            InitializeComponent();
            Size = new Size(860, 600);
            Text = "Amazon Books";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = new Icon(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\ProgrammIcon.ico");
            CenterToScreen();

            BackColor = CurrentUser.BackgroundColor;
            GlobalFont = new Font(CurrentUser.FontFamily, (int)CurrentUser.FontSize);

            TitleLabel = new Label();
            TitleLabel.Location = new Point(10, 10);
            //TitleLabel.Text = "";

            SearchLine = new ComboBox();
            SearchLine.Location = new Point(60, 52);
            SearchLine.Size = new Size(490, 40);
            SearchLine.Font = new Font(CurrentUser.FontFamily, 15);
            string[] exampleSearches = { "C#", "C++", "Python", "Kotlin", "Java", "Ruby", "OCaml" };
            foreach (string item in exampleSearches)
            {
                SearchLine.Items.Add(item);
            }
            Controls.Add(SearchLine);

            NumberOfResults = new NumericUpDown();
            NumberOfResults.Location = new Point(560, 52);
            NumberOfResults.Font = new Font(CurrentUser.FontFamily, 15);
            NumberOfResults.Width = 60;
            NumberOfResults.Maximum = 100;
            NumberOfResults.Minimum = 1;
            Controls.Add(NumberOfResults);

            SearchButton = new Button();
            SearchButton.Location = new Point(630, 49);
            SearchButton.Size = new Size(120, 36);
            SearchButton.Font = new Font(CurrentUser.FontFamily, 14);
            SearchButton.Text = "Найти";
            SearchButton.BackColor = Color.White;
            SearchButton.ForeColor = Color.Salmon;
            SearchButton.Click += SearchButton_Click;
            Controls.Add(SearchButton);

            ResultsView = new ListView();
            ResultsView.Location = new Point(20, 100);
            ResultsView.Size = new Size(800, 430);
            ResultsView.Font = GlobalFont;
            ResultsView.View = View.Details;
            ResultsView.Sorting = SortOrder.Ascending;
            ColumnHeader column1 = new ColumnHeader();
            ColumnHeader column2 = new ColumnHeader();
            ColumnHeader column3 = new ColumnHeader();
            ColumnHeader column4 = new ColumnHeader();
            column1.Text = "Название";
            column2.Text = "Автор";
            column3.Text = "Рейтинг";
            column4.Text = "Цена";
            column1.Width = 350;
            column2.Width = 250;
            column3.Width = 100;
            column4.Width = 100;
            ResultsView.Columns.Add(column1);
            ResultsView.Columns.Add(column2);
            ResultsView.Columns.Add(column3);
            ResultsView.Columns.Add(column4);
            ResultsView.ItemActivate += ResultsView_ItemActivate;
            Controls.Add(ResultsView);

            /*ResultList = new ListBox();
            ResultList.Location = new Point(20, 100);
            ResultList.Size = new Size(800, 430);
            ResultList.Font = GlobalFont;
            ResultList.MouseDoubleClick += ResultList_MouseDoubleClick;*/
            //Controls.Add(ResultList);

            





            ReturnButton = new Button();
            //ReturnButton.Text = "<";
            ReturnButton.Location = new Point(15, 50);
            ReturnButton.Size = new Size(35, 35);
            ReturnButton.BackColor = Color.White;
            ReturnButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\ReturnButtonIcon.png");
            //ReturnButton.Click += ReturnButton_Click;
            Controls.Add(ReturnButton);

            leftList = new ListBox();
            leftList.Name = "Список файлов";
            leftList.Location = new Point(20, 100);
            leftList.Size = new Size(800, 430);
            leftList.Font = GlobalFont;
            //leftList.SelectionColor = Color.Pink;
            //leftList.MouseDoubleClick += LeftList_MouseDoubleClick;
            //Controls.Add(leftList);

            SettingsButton = new Button();
            //SettingsButton.Text = "S";
            SettingsButton.Location = new Point(780, 50);
            SettingsButton.Size = new Size(40, 40);
            SettingsButton.BackColor = Color.White;
            SettingsButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\SettingsButtonIcon.png");
            SettingsButton.Click += SettingsButton_Click;
            Controls.Add(SettingsButton);

            /*LogoutButton = new Button();
            //LogoutButton.Text = "->";
            LogoutButton.Location = new Point(780, 50);
            LogoutButton.Size = new Size(40, 40);
            LogoutButton.BackColor = Color.White;
            LogoutButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\LogOutButtonIcon.png");
            LogoutButton.Click += LogoutButton_Click;
            Controls.Add(LogoutButton);*/
        }

        public void ChangeAppearance(int fontSize, string fontFamily, Color backColor)
        {
            GlobalFont = new Font(fontFamily, fontSize);
            ResultList.Font = GlobalFont;
            this.BackColor = backColor;
        }


        //------------------Ф У Н К Ц И И   К Н О П О К----------------------------
        private void SearchButton_Click(object sender, EventArgs e)
        {
            ItsWebClient = new WebClient();
            ItsWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/111.0");
            ItsWebClient.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
            ItsWebClient.Headers.Add("Accept-Encoding", "br");
            ItsWebClient.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");

            int NumberPage = 1;
            string Request = SearchLine.Text.Replace(" ", "+");

            List<Book> books = new List<Book>();
            while (books.Count < NumberOfResults.Value) 
            {
                string searchUrl = $"https://www.amazon.com/s?k={Request}&i=stripbooks-intl-ship&page={NumberPage}&ref=nb_sb_noss";
                string htmlCode;
                try
                {
                    htmlCode = ItsWebClient.DownloadString(searchUrl);
                }
                catch
                {
                    break;
                }
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(htmlCode);
                string xpath = "//div[@data-component-type='s-search-result']";
                HtmlNodeCollection bookNodes = document.DocumentNode.SelectNodes(xpath);
                if (bookNodes != null)
                {
                    foreach (HtmlNode node in bookNodes)
                    {
                        if (books.Count < NumberOfResults.Value)
                        {
                            var titleNode = node.SelectSingleNode(".//span[@class='a-size-medium a-color-base a-text-normal']");
                            string title = titleNode?.InnerText.Trim();

                            var authorNode = node.SelectSingleNode(".//a[contains(@class, 'a-size-base a-link-normal s-underline-text s-underline-link-text s-link-style')]");
                            string author = authorNode?.InnerText.Trim();

                            var ratingNode = node.SelectSingleNode(".//span[contains(@class, 'a-icon-alt')]");
                            string rating = ratingNode?.InnerText.Trim().Substring(0,3);

                            var priceNode = node.SelectSingleNode(".//span[contains(@class, 'a-offscreen')]");
                            string price = priceNode?.InnerText.Trim();

                            var linkNode = node.SelectSingleNode(".//a[contains(@class, 'a-link-normal') and contains(@class, 's-underline-text') and contains(@class, 's-underline-link-text') and contains(@class, 's-link-style') and contains(@class, 'a-text-normal')]");
                            string link = linkNode?.GetAttributeValue("href", "");

                            /*var dateNode = node.SelectSingleNode(".//span[contains(@class, 'a-size-base a-color-secondary a-text-normal')]");
                            string date = dateNode?.InnerText.Trim();*/

                            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(link))
                            {
                                books.Add(new Book(title, author, rating, price, "https://www.amazon.com" + link));
                            }
                        }
                    }
                ShowResult(books);
                }
                else
                {
                    MessageBox.Show("Ничего не найдено. Попробуйте переформулировать свой запрос");
                }
                NumberPage++;
            }
        }

        public string ConvertRequest(string request)
        {
            request.Replace(" ", "+");
            request.Replace("+", "%2B");
            request.Replace("#", "%23");
            return request;
        }

        public void ShowResult(List<Book> books)
        {
            ResultsView.Items.Clear();
            foreach (Book book in books)
            {
                ListViewItem bookItem = new ListViewItem(new[] { book.Title, book.Author, book.Rating, book.Price });
                bookItem.Tag = book.Link;
                ResultsView.Items.Add(bookItem);
            }
                /*ResultList.Items.Clear();
                foreach (Book book in books)
                {
                    ResultList.Items.Add(book);
                }*/
            }

        private void ResultsView_ItemActivate(object sender, EventArgs e)
        {
            string bookLink = ResultsView.SelectedItems[0].Tag.ToString();
            Process.Start(bookLink);
        }
        private void ResultList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Book currentBook = ResultList.SelectedItem as Book;
            Process.Start(currentBook.Link);
        }
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings(this, CurrentUser);
            formSettings.ShowDialog();
        }

        /*private void ReturnButton_Click(object sender, EventArgs e)
        {
            if (Path.GetDirectoryName(path) != null)
            {
                path = Path.GetDirectoryName(path);
            }
            ShowFiles(path);
        }*/



    }
}

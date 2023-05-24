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
        TextBox TextPath;
        ComboBox SearchLine;
        Button SearchButton;
        ListBox ResultList;
        Button SettingsButton;

        Font GlobalFont;
        User CurrentUser;
        WebClient ItsWebClient;



        Button ReturnButton;
        Button CopyButton;
        Button InsertButton;
        Button NewFolderButton;
        Button DeleteButton;
        Button RenameButton;
        Button ArchieveButton;
        Button LogoutButton;

        
        ListBox leftList;
        string path;
        MemoryStream copiedFile;
        string copiedDir;
        string copiedFileName;
        string copiedFileExtension;
        BinaryFormatter binFormatter;
        

        public FormMain()
        {
            CurrentUser = new User();

            InitializeComponent();
            Size = new Size(860, 600);
            Text = "Файловый менеджер";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = new Icon(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\ProgrammIcon.ico");
            CenterToScreen();

            copiedFile = new MemoryStream();
            BackColor = CurrentUser.BackgroundColor;
            GlobalFont = new Font(CurrentUser.FontFamily, (int)CurrentUser.FontSize);

            SearchLine = new ComboBox();
            SearchLine.Location = new Point(10, 10);
            SearchLine.Size = new Size(490, 30);
            SearchLine.Font = GlobalFont;
            string[] exampleSearches = { "Python", "C++", "Java" };
            foreach (string item in exampleSearches)
            {
                SearchLine.Items.Add(item);
            }
            Controls.Add(SearchLine);

            SearchButton = new Button();
            SearchButton.Location = new Point(510, 9);
            SearchButton.Size = new Size(100, 30);
            SearchButton.Font = GlobalFont;
            SearchButton.Text = "Найти";
            SearchButton.Click += SearchButton_Click;
            Controls.Add(SearchButton);

            ResultList = new ListBox();
            ResultList.Location = new Point(20, 100);
            ResultList.Size = new Size(800, 430);
            ResultList.Font = GlobalFont;
            ResultList.MouseDoubleClick += ResultList_MouseDoubleClick;
            Controls.Add(ResultList);

            





            ReturnButton = new Button();
            //ReturnButton.Text = "<";
            ReturnButton.Location = new Point(15, 50);
            ReturnButton.Size = new Size(40, 40);
            ReturnButton.BackColor = Color.White;
            ReturnButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\ReturnButtonIcon.png");
            ReturnButton.Click += ReturnButton_Click;
            Controls.Add(ReturnButton);

            leftList = new ListBox();
            leftList.Name = "Список файлов";
            leftList.Location = new Point(20, 100);
            leftList.Size = new Size(800, 430);
            leftList.Font = GlobalFont;
            //leftList.SelectionColor = Color.Pink;
            leftList.MouseDoubleClick += LeftList_MouseDoubleClick;
            //Controls.Add(leftList);

            path = @"C:\Users\Pugalo";
            ShowFiles(path);

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
            TextPath.Font = GlobalFont;
            leftList.Font = GlobalFont;
            this.BackColor = backColor;
        }


        //------------------Ф У Н К Ц И И   К Н О П О К----------------------------
        private void SearchButton_Click(object sender, EventArgs e)
        {
            ItsWebClient = new WebClient();
            string searchUrl = "https://www.amazon.com/s?k=python&i=stripbooks-intl-ship&ref=nb_sb_noss";
            string htmlCode = ItsWebClient.DownloadString(searchUrl);
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(htmlCode);
            string xpath = "//div[@data-component-type='s-search-result']";
            HtmlNodeCollection bookNodes = document.DocumentNode.SelectNodes(xpath);
            if (bookNodes != null)
            {
                List<Book> books = new List<Book>();

                foreach (HtmlNode node in bookNodes)
                {
                    var titleNode = node.SelectSingleNode(".//span[@class='a-size-medium a-color-base a-text-normal']");
                    string title = titleNode?.InnerText.Trim();

                    // Получаем ссылку на книгу
                    var linkNode = node.SelectSingleNode(".//a[contains(@class, 'a-link-normal') and contains(@class, 's-underline-text') and contains(@class, 's-underline-link-text') and contains(@class, 's-link-style') and contains(@class, 'a-text-normal')]");
                    string link = linkNode?.GetAttributeValue("href", "");

                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(link))
                    {
                        books.Add(new Book(title, "https://www.amazon.com" +link));
                    }
                }
                ShowResult(books);
            }
            else
            {
                MessageBox.Show("Ничего не найдено. Попробуйте переформулировать свой запрос");
            }
        }

        public void ShowResult(List<Book> books)
        {
            ResultList.Items.Clear();
            foreach (Book book in books)
            {
                ResultList.Items.Add(book);
            }
        }
        private void ResultList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Book currentBook = ResultList.SelectedItem as Book;
            Process.Start(currentBook.Link);
        }















        private void ReturnButton_Click(object sender, EventArgs e)
        {
            if (Path.GetDirectoryName(path) != null)
            {
                path = Path.GetDirectoryName(path);
            }
            ShowFiles(path);
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings(this, CurrentUser);
            formSettings.ShowDialog();
        }
        /*private void LogoutButton_Click(object sender, EventArgs e)
        {
            LoginForm.Show();
            Hide();
        }*/

        /*private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoginForm.Show();
        }*/



        //-----------------------О Б Щ И Е   Ф У Н К Ц И И-------------------------------------------------
        public void ShowFiles(string root)
        {
            leftList.Items.Clear();
            //TextPath.Text = root;
            DirectoryInfo dir = new DirectoryInfo(root);
            DirectoryInfo[] dirInfo = dir.GetDirectories();
            try
            {
                foreach (DirectoryInfo info in dirInfo)
                {
                
                    leftList.Items.Add(info);
                }
            }
            catch { }

            FileInfo[] files = dir.GetFiles();
            try
            {
                foreach (FileInfo info in files)
                {

                    leftList.Items.Add(info);

                }
            }
            catch { }
        }

        public void CopyFile(FileInfo fileInfo)
        {
            try
            {
                copiedFile = new MemoryStream();
                copiedDir = "";
                //FileInfo fileInfo = leftList.SelectedItem as FileInfo;
                copiedFileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                copiedFileExtension = Path.GetExtension(fileInfo.Name);
                using (FileStream fileStream = File.OpenRead(fileInfo.FullName))
                {
                    fileStream.CopyTo(copiedFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка при копировании файла в память: " + ex.Message);
            }
        }

        public void CopyDirectory(DirectoryInfo directoryInfo)
        {
            copiedDir = directoryInfo.FullName;
            copiedFile = new MemoryStream();
            copiedFileName = "";
            copiedFileExtension = "";
        }

        public string NewPath(string currentPath) //Вынести код изменения имени
        {
            if (File.Exists(currentPath))
            {
                string message = $"В этой папке уже существует файл с именем {copiedFileName}. Вы уверены, что хотите его заменить?";
                DialogResult result = MessageBox.Show(message, "Вставка", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    FileSystem.DeleteFile(currentPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin); // !!!
                }
                else
                {
                    int n = 0;
                    string newPath = currentPath;
                    while (File.Exists(newPath))
                    {
                        n += 1;
                        newPath = Path.Combine(Path.GetDirectoryName(currentPath), Path.GetFileNameWithoutExtension(currentPath) + $"({n})" + Path.GetExtension(currentPath));
                    }
                    currentPath = newPath;
                }
            }
            return currentPath;
        }
    }
}

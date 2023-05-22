using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.IO.Compression;


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

//Ошибка доступа к файлу
//Файл без расширения
//TODO: копирование папок
//TODO: цвета иконок
//TODO: создать норм пользователей
//TODO: вырезать, разархивировать???

//#F3A0A0

namespace AK_Project_36_Файловый_менеджер
{
    public partial class FormMain : Form
    {
        TextBox TextPath;
        Button ReturnButton;
        Button CopyButton;
        Button InsertButton;
        Button NewFolderButton;
        Button DeleteButton;
        Button RenameButton;
        Button ArchieveButton;
        Button LogoutButton;

        Button SettingsButton;
        ListBox leftList;
        string path;

        MemoryStream copiedFile;
        string copiedFileName;
        string copiedFileExtension;
        Font GlobalFont;
        BinaryFormatter binFormatter;
        User CurrentUser;
        FormLogin LoginForm;
        //LinkedList<User> Users;

        public FormMain(FormLogin loginForm, User currentUser)
        {
            CurrentUser = currentUser;
            LoginForm = loginForm;

            InitializeComponent();
            Size = new Size(860, 600);
            Text = "Файловый менеджер";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = new Icon(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\ProgrammIcon.ico");
            FormClosed += FormMain_FormClosed;
            CenterToScreen();

            copiedFile = new MemoryStream();
            BackColor = CurrentUser.BackgroundColor;
            GlobalFont = new Font(CurrentUser.FontFamily, (int)CurrentUser.FontSize);

            TextPath = new TextBox();
            TextPath.Location = new Point(10, 10);
            TextPath.Size = new Size(490, 30);
            TextPath.Font = GlobalFont;
            Controls.Add(TextPath);

            ReturnButton = new Button();
            //ReturnButton.Text = "<";
            ReturnButton.Location = new Point(15, 50);
            ReturnButton.Size = new Size(40, 40);
            ReturnButton.BackColor = Color.White;
            ReturnButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\ReturnButtonIcon.png");
            ReturnButton.Click += ReturnButton_Click;
            Controls.Add(ReturnButton);

            CopyButton = new Button();
            //CopyButton.Text = "C";
            CopyButton.Location = new Point(55, 50);
            CopyButton.Size = new Size(40, 40);
            CopyButton.BackColor = Color.White;
            CopyButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\CopyButtonIcon.png");
            CopyButton.Click += CopyButton_Click;
            Controls.Add(CopyButton);

            InsertButton = new Button();
            //InsertButton.Text = "V";
            InsertButton.Location = new Point(95, 50);
            InsertButton.Size = new Size(40, 40);
            InsertButton.BackColor = Color.White;
            InsertButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\InsertButtonIcon.png");
            InsertButton.Click += InsertButton_Click;
            Controls.Add(InsertButton);

            DeleteButton = new Button();
            //DeleteButton.Text = "D";
            DeleteButton.Location = new Point(135, 50);
            DeleteButton.Size = new Size(40, 40);
            DeleteButton.BackColor = Color.White;
            DeleteButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\DeleteButtonIcon.png");
            DeleteButton.Click += DeleteButton_Click;
            Controls.Add(DeleteButton);

            NewFolderButton = new Button();
            //NewFolderButton.Text = " + ";
            NewFolderButton.ForeColor = Color.LightPink;
            NewFolderButton.Font = new Font("Arial", 14);
            NewFolderButton.Location = new Point(175, 50);
            NewFolderButton.Size = new Size(40, 40);
            NewFolderButton.BackColor = Color.White;
            NewFolderButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\NewFolderIcon.png");
            NewFolderButton.Click += NewFolderButton_Click;
            Controls.Add(NewFolderButton);

            RenameButton = new Button();
            //RenameButton.Text = "R";
            RenameButton.Location = new Point(215, 50);
            RenameButton.Size = new Size(40, 40);
            RenameButton.BackColor = Color.White;
            RenameButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\RenameButtonIcon.png");
            RenameButton.Click += RenameButton_Click;
            Controls.Add(RenameButton);

            ArchieveButton = new Button();
            //ArchieveButton.Text = "Z";
            ArchieveButton.Location = new Point(255, 50);
            ArchieveButton.Size = new Size(40, 40);
            ArchieveButton.BackColor = Color.White;
            ArchieveButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\ArchieveButtonIcon.png");
            ArchieveButton.Click += ArchieveButton_Click;
            Controls.Add(ArchieveButton);



            leftList = new ListBox();
            leftList.Name = "Список файлов";
            leftList.Location = new Point(20, 100);
            leftList.Size = new Size(800, 430);
            leftList.Font = GlobalFont;
            //leftList.SelectionColor = Color.Pink;
            leftList.MouseDoubleClick += LeftList_MouseDoubleClick;
            Controls.Add(leftList);

            path = @"C:\Users\Pugalo";
            ShowFiles(path);

            SettingsButton = new Button();
            //SettingsButton.Text = "S";
            SettingsButton.Location = new Point(740, 50);
            SettingsButton.Size = new Size(40, 40);
            SettingsButton.BackColor = Color.White;
            SettingsButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\SettingsButtonIcon.png");
            SettingsButton.Click += SettingsButton_Click;
            Controls.Add(SettingsButton);

            LogoutButton = new Button();
            //LogoutButton.Text = "->";
            LogoutButton.Location = new Point(780, 50);
            LogoutButton.Size = new Size(40, 40);
            LogoutButton.BackColor = Color.White;
            LogoutButton.Image = Image.FromFile(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\LogOutButtonIcon.png");
            LogoutButton.Click += LogoutButton_Click;
            Controls.Add(LogoutButton);
        }

        public void ChangeAppearance(int fontSize, string fontFamily, Color backColor)
        {
            GlobalFont = new Font(fontFamily, fontSize);
            TextPath.Font = GlobalFont;
            leftList.Font = GlobalFont;
            this.BackColor = backColor;
        }


        //------------------Ф У Н К Ц И И   К Н О П О К----------------------------
        private void ReturnButton_Click(object sender, EventArgs e)
        {
            if (Path.GetDirectoryName(path) != null)
            {
                path = Path.GetDirectoryName(path);
            }
            ShowFiles(path);
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (leftList.SelectedItem != null)
            {
                if (leftList.SelectedItem is FileInfo fileInfo)
                {
                    CopyFile(fileInfo);
                }
                else if (leftList.SelectedItem is DirectoryInfo directoryInfo)
                {
                    CopyDirectory(directoryInfo);
                }
                else
                {
                    // объект не является ни FileInfo, ни DirectoryInfo
                }
            }
        }
        private void InsertButton_Click(object sender, EventArgs e)
        {
            if (copiedFileName != null)
            {
                string filePath = NewPath(Path.Combine(path, copiedFileName + copiedFileExtension));
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    copiedFile.WriteTo(fileStream);
                }
                ShowFiles(path);
            }
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (leftList.SelectedItem != null)
            {

                try
                {
                    //throw new Exception();
                    if (leftList.SelectedItem is FileInfo fileInfo)
                    {
                        string message = $"Вы уверены, что хотите удалить {fileInfo.Name}?";
                        DialogResult result = MessageBox.Show(message, "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            FileSystem.DeleteFile(fileInfo.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                        }
                    }
                    else if (leftList.SelectedItem is DirectoryInfo directoryInfo)
                    {
                        string message = $"Вы уверены, что хотите удалить {directoryInfo.Name}?";
                        DialogResult result = MessageBox.Show(message, "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            FileSystem.DeleteDirectory(directoryInfo.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show($"Не удалось удалить {leftList.SelectedItem}");
                }
                
            }
            ShowFiles(path);
        }
        private void NewFolderButton_Click(object sender, EventArgs e)
        {
            string NameOfFolder = NewPath("New Folder");
            Directory.CreateDirectory(Path.Combine(path, "New Folder"));
            ShowFiles(path);
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            if (leftList.SelectedItem is FileInfo fileInfo)
            {
                string oldName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                string extension = Path.GetExtension(fileInfo.Name);
                string newName = Microsoft.VisualBasic.Interaction.InputBox($"Введите новое имя файла {oldName}:", "Переименование файла", oldName);
                if (!string.IsNullOrEmpty(newName))
                {
                    string newPath = Path.Combine(Path.GetDirectoryName(fileInfo.FullName), newName + extension);
                    File.Move(fileInfo.FullName, newPath);
                }
            }
            else if (leftList.SelectedItem is DirectoryInfo directoryInfo)
            {
                string oldName = Path.GetFileNameWithoutExtension(directoryInfo.Name);
                string extension = Path.GetExtension(directoryInfo.Name);
                string newName = Microsoft.VisualBasic.Interaction.InputBox($"Введите новое имя папки {oldName}:", "Переименование папки", oldName);
                if (!string.IsNullOrEmpty(newName) && newName != oldName)
                {
                    string newPath = Path.Combine(Path.GetDirectoryName(directoryInfo.FullName), newName + extension);
                    Directory.Move(directoryInfo.FullName, newPath);
                }
            }
            ShowFiles(path);
        }

        private void ArchieveButton_Click(object sender, EventArgs e)
        {
            if (leftList.SelectedItem is FileInfo fileInfo)
            {
                string zipFilePath = Path.Combine(fileInfo.Directory.FullName, Path.GetFileNameWithoutExtension(fileInfo.Name) + ".zip");
                int n = 0;
                while (File.Exists(zipFilePath))
                {
                    n += 1;
                    zipFilePath = Path.Combine(fileInfo.Directory.FullName, Path.GetFileNameWithoutExtension(fileInfo.Name) + $"({n})"+ ".zip");
                }
                using (var archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name);
                }
                ShowFiles(path);
            }
            else if (leftList.SelectedItem is DirectoryInfo directoryInfo)
            {
                string zipDirName = $"{directoryInfo.FullName}.zip";
                int n = 0;
                while (File.Exists(zipDirName))
                {
                    n += 1;
                    zipDirName = Path.Combine(directoryInfo.Parent.FullName, Path.GetFileNameWithoutExtension(directoryInfo.FullName) + $"({n})" + ".zip");
                }
                ZipFile.CreateFromDirectory(directoryInfo.FullName, zipDirName);
                ShowFiles(path);
            }
        }

        private void LeftList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string currentPath = Path.Combine(path, leftList.SelectedItem.ToString());
            try 
            {
                if (!Path.HasExtension(currentPath))
                {
                    ShowFiles(currentPath);
                    path = currentPath;
                }
                else
                {
                    Process.Start(currentPath);
                }
            }
            catch 
            {
                MessageBox.Show($"Не получается открыть {Path.GetFileNameWithoutExtension(currentPath)}");
                ShowFiles(path);
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings(this, CurrentUser);
            formSettings.ShowDialog();
        }
        private void LogoutButton_Click(object sender, EventArgs e)
        {
            binFormatter = new BinaryFormatter();
            using (FileStream file = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                binFormatter.Serialize(file, LoginForm.Users);
            }
            Hide();
            FormLogin formLogin = new FormLogin();
            formLogin.ShowDialog();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            binFormatter = new BinaryFormatter();
            using (FileStream file = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                binFormatter.Serialize(file, LoginForm.Users);
            }
            Application.Exit();
        }



        //-----------------------О Б Щ И Е   Ф У Н К Ц И И-------------------------------------------------
        public void ShowFiles(string root)
        {
            leftList.Items.Clear();
            TextPath.Text = root;
            DirectoryInfo dir = new DirectoryInfo(root);
            IEnumerable<DirectoryInfo> dirInfo = dir.EnumerateDirectories("*", System.IO.SearchOption.AllDirectories);
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
            //Через архивирование???
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

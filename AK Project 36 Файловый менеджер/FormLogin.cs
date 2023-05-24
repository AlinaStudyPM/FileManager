using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AK_Project_36_Файловый_менеджер
{
    //Размер Label в зависимости от размера шрифта
    public partial class FormLogin : Form
    {
        Label LoginLabel;
        Label PasswordLabel;
        TextBox LoginBox;
        TextBox PasswordBox;
        Font LoginFormFont;
        Button LoginButton;
        Button RegistrationButton;

        int LabelHeight = 20;
        public LinkedList<User> Users = new LinkedList<User>();

        public FormLogin()
        {
            InitializeComponent();
            Text = "Файловый менеджер";
            Icon = new Icon(@"C:\Users\Pugalo\Documents\C#\AK Project 36 Файловый менеджер\Project 36 Icons\ProgrammIcon.ico");
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Size = new Size(320, 210);
            LoginFormFont = new Font("Arial", 11);
            //FormClosed += FormLogin_FormClosed;
            CenterToParent();

            LoginLabel = new Label();
            LoginLabel.Text = "Логин: ";
            LoginLabel.Font = LoginFormFont;
            LoginLabel.Location = new Point(30, 30);
            LoginLabel.Size = new Size(70, LabelHeight);
            //LoginLabel.BackColor = Color.Pink;
            Controls.Add(LoginLabel);

            LoginBox = new TextBox();
            LoginBox.Font = LoginFormFont;
            LoginBox.Location = new Point(120, 25);
            LoginBox.Size = new Size(150, 20);
            LoginBox.MaxLength = 16;
            Controls.Add(LoginBox);

            PasswordLabel = new Label();
            PasswordLabel.Text = "Пароль:";
            PasswordLabel.Font = LoginFormFont;
            PasswordLabel.Location = new Point(30, 70);
            PasswordLabel.Size = new Size(70, LabelHeight);
            //PasswordLabel.BackColor = Color.Pink;
            Controls.Add(PasswordLabel);

            PasswordBox = new TextBox();
            PasswordBox.Font = LoginFormFont;
            PasswordBox.Location = new Point(120, 65);
            PasswordBox.Size = new Size(150, 20);
            PasswordBox.MaxLength = 16;
            PasswordBox.PasswordChar = '*';
            Controls.Add(PasswordBox);

            LoginButton = new Button();
            LoginButton.Text = "Войти";
            LoginButton.Font = LoginFormFont;
            LoginButton.Location = new Point(190, 110);
            LoginButton.Size = new Size(100, LabelHeight * 3 / 2);
            //LoginButton.Click += LoginButton_Click;
            Controls.Add(LoginButton);

            RegistrationButton = new Button();
            RegistrationButton.Text = "Зарегистрироваться";
            RegistrationButton.Font = LoginFormFont;
            RegistrationButton.Location = new Point(20, 110);
            RegistrationButton.Size = new Size(160, LabelHeight * 3 / 2);
            //RegistrationButton.Click += RegistrationButton_Click;0000
            Controls.Add(RegistrationButton);


            BinaryFormatter binFormatter = new BinaryFormatter();
            using(FileStream file = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                if (file.Length > 0)
                {
                    try
                    {
                        Users = binFormatter.Deserialize(file) as LinkedList<User>;
                    }
                    catch
                    {
                        MessageBox.Show("Неудалось восставновить данные пользователей");
                    }
                }
                else
                {
                    Users = new LinkedList<User>();
                }
            }
        }

        

        /*private void RegistrationButton_Click(object sender, EventArgs e)
        {
            if (LoginBox.Text != "" && PasswordBox.Text != "")
            {
                if (FindLogin(Users, LoginBox.Text) == null)
                {
                    Users.AddLast(new User(LoginBox.Text, PasswordBox.Text));
                    FormMain formMain = new FormMain(this, Users.Last.Value);
                    formMain.FormClosed += FormMain_FormClosed;
                    Hide();
                    formMain.ShowDialog();
                    LoginBox.Text = "";
                    PasswordBox.Text = "";
                }
                else
                {
                    MessageBox.Show("Этот логин уже используется другим пользователем", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (LoginBox.Text != "" && PasswordBox.Text != "")
            {
                User FindedUser = FindLogin(Users, LoginBox.Text);
                if (FindedUser != null)
                {
                    if (FindedUser.CheckPassword(PasswordBox.Text))
                    {
                        FormMain formMain = new FormMain(this, FindedUser);
                        formMain.FormClosed += FormMain_FormClosed;
                        Hide();
                        formMain.ShowDialog();
                        LoginBox.Text = "";
                        PasswordBox.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Неправильный пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Пользователя с данным Логином не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Show();
        }

        public User FindLogin(LinkedList<User> UsersList, string login)
        {
            LinkedListNode<User> temp = UsersList.First;
            while (temp != null)
            {
                if (temp.Value.Login == login)
                {
                    return temp.Value;
                }
                temp = temp.Next;
            }
            return null;
        }
        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            using (FileStream file = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                binFormatter.Serialize(file, Users);
                //MessageBox.Show("Ок3");
            }
            Application.Exit();
        }*/
    }
}

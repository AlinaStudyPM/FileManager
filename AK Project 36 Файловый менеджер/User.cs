using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK_Project_36_Файловый_менеджер
{
    [Serializable]
    public class User
    {
        public string Login;
        private string Password;


        public decimal FontSize = 12;
        public string FontFamily = "Arial";
        public Color BackgroundColor = Color.LightCyan;

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public bool CheckPassword(string password)
        {
            if (Password == password)
            {
                return true;
            }
            return false;
        }
    }
}

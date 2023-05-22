using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AK_Project_36_Файловый_менеджер
{
    [Serializable]
    public class User
    {
        public string Login;
        private byte[] Password;


        public decimal FontSize = 12;
        public string FontFamily = "Arial";
        public Color BackgroundColor = Color.LightCyan;

        public User(string login, string password)
        {
            Login = login;
            Password = EncodeString(password);
        }

        public bool CheckPassword(string password)
        {
            byte[] attemptPass = EncodeString(password);
            if (attemptPass.Length != Password.Length)
            {
                return false;
            }
            for (int i = 0; i < attemptPass.Length; i++)
            {
                if (attemptPass[i] != Password[i])
                {
                    return false;
                }
            }
            return true;
        }
        private byte[] EncodeString(string input)
        {
            byte[] bytes;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
            return bytes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AK_Project_36_Файловый_менеджер
{
    [Serializable]
    public class User
    {
        public string Login;
        private string Password;


        public decimal FontSize = 14;
        public string FontFamily = "Candara";
        public Color BackgroundColor = Color.LightCyan;

        public User() { }
        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            Login = EncodeString(Login);
            Password = EncodeString(Password);
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            Login = DecodeString(Login);
            Password = DecodeString(Password);
        }

        private string EncodeString(string input)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(input);
            byte[] key = Encoding.Unicode.GetBytes("Anything");
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ key[i % key.Length]);
            }
            return Convert.ToBase64String(bytes);
        }

        private string DecodeString(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            byte[] key = Encoding.Unicode.GetBytes("Anything");
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ key[i % key.Length]);
            }
            return Encoding.Unicode.GetString(bytes);
        }
        public bool CheckPassword(string attemptPassword)
        {
            if (attemptPassword.Length != Password.Length)
            {
                return false;
            }
            for (int i = 0; i < attemptPassword.Length; i++)
            {
                if (attemptPassword[i] != Password[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

/*public bool CheckPassword(string password)
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
        private string EncodeString(string input)
        {
            byte[] bytes;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
            return Convert.ToBase64String(bytes);
        }*/

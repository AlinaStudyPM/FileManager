using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK_Project_36_Файловый_менеджер
{
    [Serializable]
    internal class UserPrefs
    {
        string Login;
        string Password;

        Color WindowColor = Color.LightCyan;
        int FontSize = 12;
        string FontFamily = "Arial";
    }
}

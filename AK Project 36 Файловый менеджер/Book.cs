using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AK_Project_36_Файловый_менеджер
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Rating { get; set; }
        public string Price { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }

        public Book(string title, string author, string rating, string price, string date, string link)
        {
            Title = title;
            Author = author;
            Rating = rating;
            Price = price;
            Date = date;
            Link = link;
        }
        public override string ToString()
        {
            return Title;
        }
    }
}

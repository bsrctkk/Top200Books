using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Top200Books
{
    public partial class Form1 : Form
    {
        List<Book> books = new List<Book>();
  
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {
            WebClient wc = new WebClient();
            string bookdata = wc.DownloadString("https://www.goodreads.com/list/show/18834.BBC_Top_200_Books/");
            int contentStartIndex = bookdata.IndexOf("class=\"tableList js-dataTooltip\">");
            int contentEndIndex = bookdata.IndexOf("</table");
            string content = bookdata.Substring(contentStartIndex, contentEndIndex - contentStartIndex);
            List<string> list = new List<string>();

            while (content.Contains("<tr itemscope itemtype=\"http://schema.org/Book\">"))
            {
                int trStartIndex = content.IndexOf("<tr itemscope itemtype=\"http://schema.org/Book\">");
                int trEndIndex = content.IndexOf("</tr>");

                string bookContent = content.Substring(trStartIndex, trEndIndex - trStartIndex);

                list.Add(bookContent);

                content = content.Substring(trEndIndex + 6);
            }

            foreach (string str in list)
            {
                Book book = new Book();
                int nameIndex = str.IndexOf("aria-level=");
                int nameEndIndex = str.IndexOf("</span>");

                book.name = str.Substring(nameIndex + 15, nameEndIndex - nameIndex - 15);

                books.Add(book);
            }
            dataGridView1.DataSource = books;
            lblCount.Text = books.Count.ToString(); 
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView1.DataSource = books;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searcText=txtSearch.Text.ToLower().Trim();
            var data=books.Where(x => x.name.ToLower().Contains(searcText)).ToList();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource= data;
            lblCount.Text= data.Count.ToString();
        }
    }
}

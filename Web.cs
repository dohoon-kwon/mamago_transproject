using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1002파파고
{
    public partial class Web : Form
    {
        public string Url { get; private set; }
        public Web()
        {
            InitializeComponent();
        }

        public void LinkSet(string str)
        {
            Url = str;
        }
        public void LinkLoad()
        {
            webBrowser1.Navigate(Url);
        }

        private void Web_Load(object sender, EventArgs e)
        {
            LinkLoad();
        }

        private void Web_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void Web_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}

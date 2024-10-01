using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyRestaurant
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private static string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Restaurant;Integrated Security=True";
        SqlConnection conn = new SqlConnection(conString);
        private static bool ISadmin = false;
        public static bool Funct()
        {
            return ISadmin;
        }

        MainForm mf = new MainForm();
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (NameBox.Text !=null && Passbox.Text !=null && NameBox.Text!="" && Passbox.Text!="") {
                if (Passbox.Text=="123" && NameBox.Text=="Sooraj") { 
                    label3.Visible = false;
                    //MessageBox.Show("wellcome Sooraj");
                    ISadmin = true;
                    this.Hide();
                    mf.Show();
                }
                else if (NameBox.Text=="guest" && Passbox.Text=="12345") { 
                    label3.Visible = false; 
                    MessageBox.Show("welcome admin");
                    ISadmin = false;
                }
                else { label3.Visible = true; }

            }
            else { MessageBox.Show("fill both fields first!"); }
            
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

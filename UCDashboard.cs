using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyRestaurant
{
    public partial class UCDashboard : UserControl
    {
        public UCDashboard()
        {
            InitializeComponent();
            ShowTable();
        }
        //....
        private static string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Restaurant;Integrated Security=True";
        SqlConnection conn = new SqlConnection(conString);
        UCInventory inv = new UCInventory();
        //.......Database table refresh
        public void ShowTable()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Customer", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                guna2DataGridView1.ColumnHeadersHeight = 30;
                guna2DataGridView1.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void UCDashboard_Load(object sender, EventArgs e)
        {
            ShowTable();
        }

        private void guna2DataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if(guna2DataGridView1.Columns[e.ColumnIndex].HeaderText == "Delete")
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Do you want to delete customer ","delete Customer", buttons);
                if (result == DialogResult.Yes)
                {
                     try
                    {
                        int delete = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value);
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("delete from Customer where Id= " + delete + "  ", conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        ShowTable();
                        inv.popIDcombo();
                    }
                    catch (Exception ex) {  MessageBox.Show(ex.Message+"delete unsuccesful");}
                }
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text == ""  || guna2TextBox1.Text==null)
            {
                ShowTable();
                guna2TextBox1.Text = null;
            }
            else
            {
                
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select * from Customer where Names like '" + guna2TextBox1.Text + "'", conn);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    guna2DataGridView1.ColumnHeadersHeight = 30;
                    guna2DataGridView1.DataSource = dt;
                    conn.Close();
                    guna2TextBox1.Text = null;
            }
        }
    }
}
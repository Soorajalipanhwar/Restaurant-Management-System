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
    public partial class UCPlaceOrder : UserControl
    {
        public UCPlaceOrder()
        {
            InitializeComponent();
        }
        private static string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=rest;Integrated Security=True";
        SqlConnection conn = new SqlConnection(conString);
        private string B1, B2, B3, B4, B5;
        //....show table in gridview Method
        public  void ShowTable()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Table1", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                guna2DataGridView1.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }
        //.......initializing combobox values for Id
        public void combobox2()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select Id from Table1", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.SelectCommand = cmd;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "Table1";
                comboBox2.ValueMember = "Id";
                conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Error in gunna2Combobox1");
                conn.Close();
            }
        }
        //........initializing combobox values for Table no
        public void comboboxTable()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select TableNo from Table1", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.SelectCommand = cmd;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Table1";
                comboBox1.ValueMember = "TableNo";
                conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Error in Combobox1");
                conn.Close();
            }
        }
        //.......Search method
        public void SearchBtn()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Table1 where Name like '%" + Convert.ToString(guna2TextBox1.Text) + "%' or Orders like '%"+Convert.ToString(guna2TextBox1)+ "'", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                guna2DataGridView1.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            //Delete button ..........
            string  del =Convert.ToString(comboBox2.SelectedValue);
            MessageBox.Show(del);
          try
            {
                 conn.Open();
                 SqlCommand cmd = new SqlCommand("delete from Table1 where Id ="+comboBox2.SelectedText,conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                ShowTable();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

                MessageBox.Show("Delete unsuccessful");
                conn.Close();
            }
            comboboxTable();
            combobox2();
           
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            //........Search Name 
            SearchBtn();
        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            SearchBtn();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "" && comboBox2.Text != "0")
            {
                guna2Button1.Enabled = true;
            }
            else
                guna2Button1.Enabled = false;
        }

        private void guna2DataGridView1_Load(object sender, DataGridViewCellEventArgs e)
        {
            ShowTable();
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            ShowTable();
            //.....clear gunaSearchBox
            guna2TextBox1.Text = null;
            //.....
        }

        private void UCPlaceOrder_Load_1(object sender, EventArgs e)
        {

            ShowTable();
            //.......Initializing guna2combobox1 values
            combobox2();
            //.........ComboBox1
            comboboxTable();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Insert Button( in Database table)......
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text != "" && comboBox1.Text !="0" )
            {
                guna2Button1.Enabled = true;
                //Add button (in Database Table)........
                try
                {
                    conn.Open();
                    B1 = System.Convert.ToString(comboBox2.Text);
                    B2 = System.Convert.ToString(textBox2.Text);
                    B3 = System.Convert.ToString(textBox3.Text);
                    B4 = System.Convert.ToString(textBox4.Text);
                    B5 = System.Convert.ToString(comboBox1.Text);
                    SqlCommand cmd = new SqlCommand("insert into Table1 values (' " + B1 + "  ','  " + B2 + "  ','  " + B3 + "  ','  " + B4 + "  ','   " + B5 + " ') ", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    textBox2.Text = null;
                    textBox3.Text = null;
                    textBox4.Text = null;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("Add  unsuccessful");
                    conn.Close();
                }
                //..........to refresh guna2combobox
                combobox2();
                //..........auto updating table in datagridview
                ShowTable();
                //..........
            }
            else
                guna2Button1.Enabled = false;
        }
    }
}
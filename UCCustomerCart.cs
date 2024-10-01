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
    public partial class UCCustomerCart : UserControl
    {
        public UCCustomerCart()
        {
            InitializeComponent();
        }


        //....
        private static string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=rest;Integrated Security=True";
        SqlConnection conn = new SqlConnection(conString);

        //.......Database table refresh
        public void ShowTable()
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
        //.......Search Method
        public void Search()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Table1 where Name like '%" + Convert.ToString(guna2TextBox1.Text) + "%' or Orders like '%" + Convert.ToString(guna2TextBox1) + "'", conn);
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
        //.......Initialize Combobox
        public void ComboBox()
        {
            try
            {
                if(conn.State == ConnectionState.Closed)
                conn.Open();
                SqlCommand cmd = new SqlCommand("select Id from Table1", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.SelectCommand = cmd;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Table1";
                comboBox1.ValueMember = "Id";
                conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Error in gunna2Combobox1");
                conn.Close();
            }
        }
        //.......Initialize the textboxes with data selecting by combobox
        public void FillData()
        {
            if (comboBox1.Text == "")
            {
            }
            else {
               try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand("select Id,Name,Orders,Bill,TableNo from Table1 where id ='" + Convert.ToString(comboBox1.Text) + "'", conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        textBox5.Text = rd.GetValue(0).ToString();
                        textBox1.Text = rd.GetValue(1).ToString();
                        textBox2.Text = rd.GetValue(2).ToString();
                        textBox3.Text = rd.GetValue(3).ToString();
                        textBox4.Text = rd.GetValue(4).ToString();
                    }
                    conn.Close();
                }
              catch (Exception ex)
                {
                     MessageBox.Show(ex.Message);
                      MessageBox.Show("Filling data unsuccessful");
                      conn.Close();
                }
                
                
            }
        }
        private void UCCustomerCart_Load(object sender, EventArgs e)
        {
            ShowTable();
            ComboBox();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            Search();
        }
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            FillData();
        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Search();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ComboBox();
        }
        private void guna2DataGridView1_Load(object sender, DataGridViewCellEventArgs e)
        {
            ComboBox();
            guna2DataGridView1.CurrentRow.Selected = true;
            comboBox1.SelectedValue = guna2DataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            textBox1.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString();
            textBox2.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Order"].Value.ToString();
            textBox3.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Bill"].Value.ToString();
            textBox4.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["TableNo"].Value.ToString();
            textBox5.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
        }
       private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                guna2DataGridView1.CurrentRow.Selected = true;
                comboBox1.SelectedValue = guna2DataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                textBox1.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                textBox2.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Orders"].Value.ToString();
                textBox3.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Bill"].Value.ToString();
                textBox4.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["TableNo"].Value.ToString();
                textBox5.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
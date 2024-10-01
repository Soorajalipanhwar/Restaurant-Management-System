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
    public partial class Orders : UserControl
    {
       
        public Orders()
        {
            InitializeComponent();
            ShowTablesInBox();
            ShowCustomers();
        }
        private static string constring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = Restaurant; Integrated Security = True";
        SqlConnection Conn = new SqlConnection(constring);
        private static int id;
        UCInventory inv = new UCInventory();
        public void ShowTablesInBox()
        {
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("select TableNo from TabFloor", Conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tab1 = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(tab1);
                TableBox1.DataSource = tab1;
                TableBox1.DisplayMember = "TableNo";
                Conn.Close();
            }catch(Exception ex) { MessageBox.Show(ex.Message); Conn.Close(); }
        }
        public void ShowCustomers()
        {
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Customer", Conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tab1 = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(tab1);
                guna2DataGridView1.DataSource = tab1;
                guna2DataGridView1.ColumnHeadersHeight = 36;
                Conn.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); Conn.Close(); }
        }
        public int loop()
        {
            int CId = 1;
            SqlCommand cmd = new SqlCommand("select Id from Customer", Conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int num = Convert.ToInt32(reader.GetInt32(0).ToString());
                    if (num != CId) { MessageBox.Show(num.ToString()); }
                    else if (CId == num)
                    {
                        ++CId;
                    }
                }
            }
            else
            {
                MessageBox.Show("No rows found.");
            }
            reader.Close();
            return CId;
        }
        public void UpdateCustomer()
        {
           try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("update Customer set Names='" + guna2TextBox1.Text + "', OrderType='" + guna2ComboBox2.Text + "',  TableNo=" + TableBox1.Text + "  where Id=" + id + " ", Conn);
                cmd.ExecuteNonQuery();
                Conn.Close();
                ShowCustomers();
                guna2TextBox1.Text = null;
            }
            catch(Exception ex){  MessageBox.Show(ex.Message);   Conn.Close();   }
            
        }

        private void guna2Button1_Click(object sender, EventArgs e)//Add customer
        {
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("insert into Customer values(" + loop() + ",'" + guna2TextBox1.Text + "','" + guna2ComboBox2.Text + "'," + TableBox1.Text + ",0)", Conn);
                cmd.ExecuteNonQuery();
                Conn.Close();
                ShowCustomers();
                guna2TextBox1.Text = null;
            }
           catch(Exception ex) { MessageBox.Show(ex.Message); Conn.Close(); }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text !="" && guna2TextBox1.Text !=null )
            {
                UpdateCustomer();
                inv.popIDcombo();
            }
            else
            {
                MessageBox.Show("please select a customer first");
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    try
                    {
                        guna2DataGridView1.CurrentRow.Selected = true;
                        id = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["Id"].FormattedValue);
                        Conn.Open();
                        SqlCommand cmd = new SqlCommand("select Names,OrderType,TableNo,Id from Customer where Id= " + id + "", Conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable tab1 = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(tab1);
                        foreach (DataRow dr in tab1.Rows)
                        {
                            guna2TextBox1.Text = dr["Names"].ToString();
                        }
                        Conn.Close();
                    }
                    catch (Exception ex) {MessageBox.Show(ex.Message); Conn.Close(); }
                }
                else
                {
                    MessageBox.Show("Table is empty");
                }

            }
            catch (System.ArgumentOutOfRangeException ex) { MessageBox.Show("Please select a proper row \n"+ex.Message); }
        }
    }
}

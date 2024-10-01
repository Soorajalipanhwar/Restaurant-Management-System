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
    public partial class test : Form
    {
        SqlConnection Conn = new SqlConnection("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = Restaurant; Integrated Security = True");
        private static string This;
        private static int DeleteID;
        public test()
        {
            InitializeComponent();
            ShowGridview1();
            CountRows();
            label4.Text = guna2DataGridView2.Rows[0].Cells[0].Value.ToString();
        }
        public void  CountRows()
        {
            Conn.Open();
            SqlCommand cmd = new SqlCommand("select COUNT(*) from FCategories ",Conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable tab1 = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(tab1);
            guna2DataGridView2.DataSource = tab1;
            guna2DataGridView2.ColumnHeadersHeight = 30;
            label3.Text = guna2DataGridView2.CurrentCell.Value.ToString();
            label4.Text = guna2DataGridView2.CurrentCell.Value.ToString();
            Conn.Close();
        }       
        public void ShowGridview1()
        {
            // try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("select * from FCategories", Conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tab1 = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(tab1);
                guna2DataGridView1.DataSource = tab1;
                guna2DataGridView3.DataSource = tab1;
                guna2DataGridView1.ColumnHeadersHeight = 30;
                Conn.Close();
            }
            //  catch (Exception ex) { MessageBox.Show(ex.Message); }
            //  finally { //Conn.Close();}
        }
        public int loop()
        {
            int CId=1;
            SqlCommand cmd = new SqlCommand("select Id from FCategories",Conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //MessageBox.Show(reader.GetInt32(0).ToString());
                    int num = Convert.ToInt32(reader.GetInt32(0).ToString());
                   // MessageBox.Show(num.ToString());
                    if (num !=CId) { MessageBox.Show(num.ToString()); }
                    else if (CId == num)
                    {
                        ++CId;
                    }
                }
                MessageBox.Show(CId.ToString());
            }
            else
            {
                MessageBox.Show("No rows found.");
            }
            reader.Close();
            return CId;
        }
        public  void add()
        {
            if (guna2TextBox1.Text != "" && guna2TextBox1.Text != null)
            {
                // CId=loop(CId, Count);
               // try
                {
                    CountRows();
                    loop();
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("Insert into Categories values( " + loop() + " ,' " + guna2TextBox1.Text + " ')", Conn);
                    cmd.ExecuteNonQuery();
                    guna2TextBox1.Text = null;
                    Conn.Close();
                    ShowGridview1();
                    CountRows();
                }
              //  catch (Exception ex)
                /*{
                    MessageBox.Show("error line 80 " + ex.Message);
                    Conn.Close();
                }*/
            }
            else { MessageBox.Show("please enter a Category first"); }
        }
        private void guna2Button1_Click(object sender, EventArgs e)//ADD
        {
            if (guna2TextBox1.Text != "" && guna2TextBox1.Text != null )
            {
                try
                {
                    CountRows();
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("Insert into FCategories values( " +loop()+ " ,' " + guna2TextBox1.Text + " ')", Conn);
                    cmd.ExecuteNonQuery();
                    guna2TextBox1.Text = null;
                    Conn.Close();
                    ShowGridview1();
                    CountRows();
                }
                catch (Exception ex) { MessageBox.Show("error line 80 " + ex.Message);
                    Conn.Close();
                }
            }
            else { MessageBox.Show("please enter a Category first"); }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            MainForm mf = new MainForm();
            
        }

        private void guna2Button2_Click(object sender, EventArgs e)//Delete
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Do you want to delete " + This, "Delete Category", buttons);
            if (result == DialogResult.Yes)
            {
                // try
                {
                    CountRows();
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from FCategories where Id=" + DeleteID, Conn);
                    cmd.ExecuteNonQuery();
                    Conn.Close();
                    ShowGridview1();
                    CountRows();
                }
                // catch (Exception ex) {  MessageBox.Show(ex.Message+"delete unsuccesful");}
            }
        }
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)//click
        {

            // try
            {
                if (guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    guna2DataGridView1.CurrentRow.Selected = true;
                    This = guna2DataGridView1.Rows[e.RowIndex].Cells["Cname"].FormattedValue.ToString();
                    DeleteID = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["Id"].FormattedValue);
                }
            }
            // catch (System.ArgumentOutOfRangeException ex) { MessageBox.Show("Please select a proper row \n"+ex.Message); }
        }
    }
}
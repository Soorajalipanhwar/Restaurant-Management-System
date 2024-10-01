using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MyRestaurant
{
    public partial class InvebtoryUC1 : UserControl
    {
        public InvebtoryUC1()
        {
            InitializeComponent();
            ShowTables();
            ShowGridview1();
            ShowGridview3();
            ShowCateInCombo();
            guna2DataGridView1.ColumnHeadersHeight = 36;
        }


        private static string constring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = Restaurant; Integrated Security = True";
        public static int ID, DeleteID,Id;
        public static string This, Item;
        public static int CId;
        SqlConnection conn = new SqlConnection(constring);
        UCInventory uc = new UCInventory();


        public int loop()
        {
            int CId = 1;
            SqlCommand cmd = new SqlCommand("select Id from Categories", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int num = Convert.ToInt32(reader.GetInt32(0).ToString());
                    if (num != CId) { //MessageBox.Show(num.ToString());
                                      }
                    else if (CId == num)
                    {
                        ++CId;
                    }
                }
              //  MessageBox.Show(CId.ToString());
            }
            else
            {
                MessageBox.Show("No rows found.");
            }
            reader.Close();
            return CId;
        }
        public int Itemsloop()
        {
            int CId = 1;
            SqlCommand cmd = new SqlCommand("select Id from Items", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int num = Convert.ToInt32(reader.GetInt32(0).ToString());
                    if (num != CId) { }
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
        public void ShowGridview1()
        {
           try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Categories", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tab1 = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(tab1);
                guna2DataGridView1.DataSource = tab1;
                guna2DataGridView1.ColumnHeadersHeight = 30;
                conn.Close();
            }
           catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void ShowTables()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from TabFloor", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tab1 = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(tab1);
                guna2DataGridView2.DataSource = tab1;
                guna2DataGridView2.ColumnHeadersHeight = 30;
                conn.Close();
            }
             catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        public void ShowGridview3()
        {
            try
            {
                conn.Close();
               conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Items", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tab1 = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(tab1);
                guna2DataGridView3.DataSource = tab1;
                guna2DataGridView3.ColumnHeadersHeight = 30;
                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally{ conn.Close();}

        }
        public void ShowGridview()
        {
             try
            {
                conn.Close();
                conn.Open();
                SqlCommand cmd = new SqlCommand("select Iname from Items  where ICategory like ' " +CategoryBox.SelectedItem.ToString()+"  '  ", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tab1 = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(tab1);
                guna2DataGridView1.DataSource = tab1;
                guna2DataGridView1.ColumnHeadersHeight = 30;
                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally{ conn.Close();}

        }
        public void ShowCateInCombo()
        {
                CategoryBox.Text = "Select a Category";
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Categories", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                CategoryBox.DataSource = dt;
                CategoryBox.DisplayMember = "Cname";
                conn.Close();
        }
        public void Add()
        {
            if (guna2TextBox1.Text != "" && guna2TextBox1.Text != null)
            {
                // CId=loop(CId, Count);
                try
                {
                    conn.Open();
                    loop();
                    SqlCommand cmd = new SqlCommand("Insert into Categories values( " + loop() + " ,' " + guna2TextBox1.Text + " ')", conn);
                    cmd.ExecuteNonQuery();
                    guna2TextBox1.Text = null;
                    conn.Close();
                    ShowGridview1();
                }
                 catch (Exception ex)
                {
                    MessageBox.Show("error line 80 " + ex.Message);
                    conn.Close();
                }
            }
            else { MessageBox.Show("please enter a Category first"); }
        }



        private void guna2Button3_Click(object sender, EventArgs e)//delete Category
        {

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Do you want to delete " + This, "Delete Category", buttons);
            if (result == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from Categories where Id=" + DeleteID, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex) {  MessageBox.Show(ex.Message+"delete unsuccesful");}
            }
        }
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    guna2DataGridView1.CurrentRow.Selected = true;
                    This = guna2DataGridView1.Rows[e.RowIndex].Cells["Cname"].FormattedValue.ToString();
                    DeleteID = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["Id"].FormattedValue);
                }
            }
            catch (System.ArgumentOutOfRangeException ex) { MessageBox.Show("Please select a proper row \n"+ex.Message); }
        }
        private void guna2Button2_Click_1(object sender, EventArgs e)//add Category
        {
            if (guna2TextBox1.Text != "" && guna2TextBox1.Text != null)
            {
                Add();
                ShowCateInCombo();
            }
        }
        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
                if (guna2DataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    guna2DataGridView2.CurrentRow.Selected = true;
                    ID = Convert.ToInt32(guna2DataGridView2.Rows[e.RowIndex].Cells["TableNo"].FormattedValue);
                }
            
        }
        private void guna2Button3_Click_1(object sender, EventArgs e)//delete Fcategories
        {

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Do you want to delete " + This, "Delete Category", buttons);
            if (result == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from Categories where Id=" + DeleteID, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    ShowGridview1();
                    ShowCateInCombo();
                }
                catch (Exception ex) {  MessageBox.Show(ex.Message+"delete unsuccesful");}
            }
        }
        private void guna2ComboBox1SelectCate_Click(object sender, EventArgs e)
        {
           
        }
        private void guna2Button4_Click_1(object sender, EventArgs e)//dlete Tables
        {

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Do you want to delete this", "Delete", buttons);
            if (result == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from TabFloor where TableNo=" + DeleteID, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    ShowTables();
                    guna2TextBox4.Text = null;
                    guna2ComboBox1.SelectedIndex = -1;
                    guna2ComboBox2.SelectedIndex = -1;
                }
                catch (Exception ex) {  MessageBox.Show(ex.Message+"delete unsuccesful");}
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)//add tables
        {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into TabFloor values(" + guna2TextBox4.Text + ",'" + guna2ComboBox2.SelectedItem.ToString() + "','" + guna2ComboBox1.SelectedItem.ToString() + "') ", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                ShowTables();
                guna2TextBox4.Text = null;
                guna2ComboBox1.SelectedIndex = -1;
                guna2ComboBox2.SelectedIndex = -1;
        }
        private void guna2Button5_Click(object sender, EventArgs e)//add items
        {
            if (CategoryBox.Text == null && guna2ComboBox2.Text==null || guna2TextBox3.Text == null){ MessageBox.Show("Fill All the fields first"); }
            else {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into Items values( "+Itemsloop()+",  '"+guna2TextBox2.Text+"',   '"+CategoryBox.Text.ToString()+"',"+guna2TextBox3.Text+")",conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                ShowGridview3();
            }
        }
        private void guna2DataGridView3_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
                if (guna2DataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    guna2DataGridView3.CurrentRow.Selected = true;
                    DeleteID = Convert.ToInt32(guna2DataGridView3.Rows[e.RowIndex].Cells["Id"].FormattedValue);
                }
        }
        private void guna2Button6_Click(object sender, EventArgs e)//delete Items
        {
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from Items where Id=" + DeleteID, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                ShowGridview3();
        }

        private void CategoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void CategoryBox_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }

        private void CategoryBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ShowGridview();
        }

        private void guna2DataGridView2_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
                if (guna2DataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    guna2DataGridView2.CurrentRow.Selected = true;
                    DeleteID = Convert.ToInt32(guna2DataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue);
                }
        }
        private void guna2Button5_Click_1(object sender, EventArgs e)//add Items
        {

            if ((guna2TextBox2.Text != "" && guna2TextBox2.Text != null) && (guna2TextBox3.Text != "" && guna2TextBox3.Text != null))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Insert into Items values('" + guna2TextBox2.Text.ToString() + "','" + CategoryBox.Text.ToString() + "','" + guna2TextBox3.Text.ToString() + "')", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    ShowGridview3();
                    guna2TextBox2.Text = null;
                    guna2TextBox3.Text = null;
                }
                catch (Exception ex) { MessageBox.Show("Add unsuccessful " + ex.Message); }
                finally { conn.Close(); }
                ShowGridview3();
            }
            else { MessageBox.Show("please fill all boxex first"); }

        }
        private void guna2DataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           // try
            {
                if (guna2DataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    guna2DataGridView1.CurrentRow.Selected = true;
                    Item = guna2DataGridView1.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                    Id = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["Id"].FormattedValue);
                }
            }
           // catch (Exception ex) { MessageBox.Show("Please select a proper row" + ex.Message); }
        }
    }
}
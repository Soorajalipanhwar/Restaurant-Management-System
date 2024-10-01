using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant
{
    public partial class UCInventory : UserControl
    {
        private static string constring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = Restaurant; Integrated Security = True";


        SqlConnection conn = new SqlConnection(constring);
        public static string Query;
        private static int quantity,DeleteID,sum,total=0;
        private static string pdfFileName = "C:/users/sooraj.pdf";
        //
        //
        //

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            using (Bitmap bm = new Bitmap(richTextBox1.Width, richTextBox1.Height))
            {
                richTextBox1.DrawToBitmap(bm, new Rectangle(0, 0, richTextBox1.Width, richTextBox1.Height));
                e.Graphics.DrawImage(bm, 0, 0);
            }
        }
        
        private void FormatDataGridViewInRichTextBox(DataGridView dataGridView, RichTextBox richTextBox)
        {
            // Clear the RichTextBox
            richTextBox.Clear();

            // Use a monospaced font for fixed-width formatting
            Font monospacedFont = new Font("Courier New", 12);

            // Set the font for the RichTextBox
            richTextBox.Font = monospacedFont;

            // Define column widths (adjust as needed)
            int[] columnWidths = { 20, 10, 10, 10 };

            // Add column headings
            richTextBox.AppendText("Item".PadRight(columnWidths[0]) + "Price".PadRight(columnWidths[1]) + "Quantity".PadRight(columnWidths[2]) + "Total".PadRight(columnWidths[3]) + Environment.NewLine);

            // Iterate through rows and columns in the DataGridView
            for (int row = 0; row < dataGridView.Rows.Count; row++)
            {
                for (int col = 0; col < dataGridView.Columns.Count; col++)
                {
                    string cellValue = dataGridView.Rows[row].Cells[col].Value.ToString();
                    // Pad cell value with spaces to match column width
                    int padding = columnWidths[col] - cellValue.Length;
                    if (padding > 0)
                    {
                        cellValue = cellValue.PadRight(columnWidths[col]);
                    }

                    // Add cell value to the RichTextBox
                    richTextBox.AppendText(cellValue);

                    // Separate columns with a space
                    richTextBox.AppendText(" ");
                }

                // Add a new line to separate rows
                richTextBox.AppendText(Environment.NewLine);
            }
        }
        //



        public UCInventory()
        {
            InitializeComponent();
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            popIDcombo();
            showOrders();
           // supportinggrid();
        }



        public void ShowCategories()
        {
            conn.Close();
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from Categories", conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.SelectCommand = cmd;
            sda.Fill(dt);
            CategoriesBox1.DataSource = dt;
            CategoriesBox1.DisplayMember = "Cname";
            conn.Close();
        }
        public  void Populate()
        {
            conn.Close();
            conn.Open();
            SqlCommand cmd = new SqlCommand("select Iname,Price from Items where ICategory like '%"+CategoriesBox1.Text.ToString()+"%'  ", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable tab1 = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(tab1);
            ItemsBox.DataSource = tab1;
            ItemsBox.DisplayMember = "Iname";
            PriceBox.DataSource = tab1;
            PriceBox.DisplayMember = "Price";

            conn.Close();
        }
        public void popIDcombo()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select Id from Customer",conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable tab = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(tab);
            IDCombo.DataSource = tab;
            IDCombo.DisplayMember = "Id";
            conn.Close();
        }
        public void loop(int id)
        {
            conn.Open();
            sum = 0;
            SqlCommand cmd = new SqlCommand("select Total from Orders where id="+id, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    sum +=reader.GetInt32(0);
                    
                }
                label11.Text=sum.ToString();
            }
            else
            {
                MessageBox.Show("Cart is empty");
                label11.Text = "0";
            }
            reader.Close();
            conn.Close();
        }
        public void clearcombo()
        {
            ItemsBox.SelectedIndex = -1;
            CategoriesBox1.SelectedIndex = -1;
            guna2NumericUpDown1.TabIndex = 0;
        }
        public void ShowCart(int id)
        {
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("select Item,Price,Quantity,Total from Orders where Id=" + id + ";", conn); ;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tab1 = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(tab1);

                // Add a new row to the DataTable to display the total bill
                DataRow totalRow = tab1.NewRow();
                totalRow["Item"] = "Total Bill"; // Update with the actual column name
                totalRow["Total"] =System.Convert.ToInt32( CalculateTotalBill(id)); // Calculate the total bill
                tab1.Rows.Add(totalRow);


                guna2DataGridView1.DataSource = tab1;
                guna2DataGridView1.ColumnHeadersHeight = 30;
                
            }
            catch (Exception ex)
            {
                // Catch the exception and display it in a message box
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                conn.Close(); }
        }
        public int CalculateTotalBill(int id)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            sum = 0;
            SqlCommand cmd = new SqlCommand("select Total from Orders where Id=" + id, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    sum += reader.GetInt32(0);
                }
                reader.Close();
            }
            conn.Close();

            return sum;
        }

        public void AddOrder()
        {
           
            if (ItemsBox.SelectedIndex != -1 && CategoriesBox1.SelectedIndex != -1)
            {
                if (quantity != 0)
                {
                    try
                    {
                        conn.Open();
                        // string OrderId =""+IDCombo.Text.ToString()+ItemsBox.Text.ToString();
                        total = (Convert.ToInt32(PriceBox.Text)) * (quantity);
                        SqlCommand cmd = new SqlCommand("insert into Orders values(" + IDCombo.Text.ToString() + ", '" + ItemsBox.Text.ToString() + "'," + PriceBox.Text + "," + guna2NumericUpDown1.Value + ","+total+")", conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        showOrders();
                    }
                    catch (Exception ex) { MessageBox.Show("the Item is already in the bill" + ex.Message); }
                }
                else
                {
                    MessageBox.Show("quantity mus be greater than 0");
                }
            }
            else
            {
                MessageBox.Show("please select items first");
            }
        }
        public void showOrders()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from Orders ", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable tab1 = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(tab1);
            guna2DataGridView2.DataSource = tab1;
            conn.Close();
        }

        public void UpdateCustomer(int id)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("update Customer set Bill="+label11.Text+" where Id="+id,conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void PrintFun()
        {

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument1;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument1.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
           quantity = Convert.ToInt32(guna2NumericUpDown1.Value);
           // Price =Convert.ToInt32( PriceBox.Text);
           //label11.Text =Convert.ToString( quantity * Price);
        }
        private void CategoriesBox1_Click_2(object sender, EventArgs e)
        {
            ShowCategories();
        }
        private void CategoriesBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Populate();
            guna2NumericUpDown1.Value = 0;
        }

        private void ItemsBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            guna2NumericUpDown1.Value = 0;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            FormatDataGridViewInRichTextBox(guna2DataGridView1, richTextBox1);
            PrintFun();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete from Orders where Price=" + DeleteID, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            AddOrder();
            int id = Convert.ToInt32(IDCombo.Text);
            ShowCart(id);
            UpdateCustomer(id);
        }

        private void guna2DataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.Columns[e.ColumnIndex].HeaderText == "Delete")
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Do you want to delete this Item?", "Delete Item", buttons);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Get the ID and Item values from the DataGridView
                        int id = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value);
                        MessageBox.Show(Convert.ToString(id));
                        string item = guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                        conn.Open();

                        // Use a simple SQL query to delete the record
                        string deleteQuery = $"DELETE FROM Orders WHERE Id = {id} AND Item = '{item}'";
                        SqlCommand cmd = new SqlCommand(deleteQuery, conn);

                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Delete unsuccessful: " + ex.Message);
                    }
                }
            }

        }


        private void IDCombo_SelectionChangeCommitted_1(object sender, EventArgs e)
        {
            //label12.Text = ;
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from Customer where Id=" + IDCombo.Text.ToString() + "", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable tab = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(tab);
            foreach (DataRow dr in tab.Rows)
            {
                guna2TextBox1.Text = dr["TableNo"].ToString();
                guna2TextBox2.Text = dr["OrderType"].ToString();
                guna2TextBox3.Text = dr["Names"].ToString();
            }
            conn.Close();
            int id = Convert.ToInt32(IDCombo.Text);
            ShowCart(id);
            loop(id);
            UpdateCustomer(id);
            FormatDataGridViewInRichTextBox(guna2DataGridView1, richTextBox1);
        }
    }
}
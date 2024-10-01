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
    public partial class MainForm : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;
        public MainForm()
        {
            InitializeComponent();
            random = new Random();
        }

        //.........................................///
        //
        private Color SelectThemeColor()
        {
            int index = random.Next(ThemeColor.Colorlist.Count);
            while (tempIndex == index)
            {
                index = random.Next(ThemeColor.Colorlist.Count);
            }
            tempIndex = index;
            string color = ThemeColor.Colorlist[index];
            return ColorTranslator.FromHtml(color);
        }
        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = SelectThemeColor();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.Font = new Font("Microsoft Sans Serif", 15.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)(0));
                    currentButton.ForeColor = Color.White;

                }
            }
        }
        //
        private void DisableButton()
        {
            foreach (Control previousBtn in Menupanel1 .Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)(0));

                }
            }
        }
        //
        //........................................///

        private void button1_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            ucDashboard1.BringToFront();
            ucDashboard1.Update();
            ucDashboard1.ShowTable();
            pictureBox1.BringToFront();
            guna2CirclePictureBox1.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            ucInventory1.BringToFront();
            ucInventory1.popIDcombo();
            ucInventory1.Update();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            orders1.BringToFront();
            orders1.Update();
            orders1.UpdateCustomer();
            orders1.ShowCustomers();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            invebtoryUC11.BringToFront();
            invebtoryUC11.Update();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
           this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            aboutUC1.BringToFront();
            guna2Button1.BringToFront();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ucDashboard1.BringToFront();
            guna2CirclePictureBox1.BringToFront();
        }
    }
}

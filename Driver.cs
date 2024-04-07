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
using System.Text.RegularExpressions; //using for phone number validation

namespace Transport_management_system
{
    public partial class Driver : Form
    {
        public Driver()
        {
            InitializeComponent();
            //GetCars();
            ShowDriver();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Driver_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HP\OneDrive\Documents\TransportDb.mdf;Integrated Security=True;Connect Timeout=30");

       /* private void GetCars()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select * from VehicleTbl", Con);
            SqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
            dt.Columns.Add("VLp", typeof(string));
            dt.Load(rdr);
            VehicleCb.ValueMember = "VLp";
            VehicleCb.DataSource = dt;
            Con.Close();
        }*/
        private void Clear()        //for delete btn
        {
            DrNameTb.Text = "";
            GenCb.SelectedIndex = -1;
            PhoneTb.Text = "";
            Dradd.Text = "";
        }
        private void ShowDriver()
        {
            Con.Open();
            string Query = "select * from DriverTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DriverDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (DrNameTb.Text == "" || GenCb.SelectedIndex == -1 || PhoneTb.Text == "" || Rating.SelectedIndex == -1 || Dradd.Text == "" || DOB.Value == DateTime.MinValue || JoinDate.Value == DateTime.MinValue)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Validate the phone number using regex
                Regex ex = new Regex("^[0-9]{10}");
                bool isValid = ex.IsMatch(PhoneTb.Text);

                if (!isValid)
                {
                    MessageBox.Show("Please enter a valid 10-digit numeric mobile number.");
                }
                else
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("insert into DriverTbl (DrName,Drphone,DrAdd,DrDOB ,DrJoinDate,DrGen,DrRating) values(@DRN,@DrP,@DrA,@DrD,@DrJ,@DrG,@DrR)", Con);
                        cmd.Parameters.AddWithValue("@DRN", DrNameTb.Text);
                        cmd.Parameters.AddWithValue("@DRP", PhoneTb.Text);
                        cmd.Parameters.AddWithValue("@DRA", Dradd.Text);
                        cmd.Parameters.AddWithValue("@DRD", DOB.Value.Date); // Assuming DOB is a DateTimePicker control
                        cmd.Parameters.AddWithValue("@DRJ", JoinDate.Value.Date);
                        cmd.Parameters.AddWithValue("@DRG", GenCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DRR", Rating.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Driver Recorded");
                        Con.Close();
                        ShowDriver();
                        Clear();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message);
                    }
                }
            }
        }

        string SelectedDriver;
        private void CountBookingbyDriver()
        {
            Con.Open();
            string Query = "select count(*) from BookingTbl where Driver='" + SelectedDriver + "'";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            CountBookingsLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
    
        int Rate;
        private void GetStars()
        {
            Rate = Convert.ToInt32(DriverDGV.SelectedRows[0].Cells[7].Value.ToString());
            RateLbl.Text = "" + Rate;
            if(Rate == 1 || Rate == 2)
            {
                LevelLbl.Text = "OK";
                LevelLbl.ForeColor = Color.Red;
            } else if(Rate == 3|| Rate == 4)
            {
                LevelLbl.Text = "GOOD";
                LevelLbl.ForeColor = Color.DodgerBlue;
            }
            else
            {
                LevelLbl.Text = "EXCELLENT";
                LevelLbl.ForeColor = Color.Green;
            }
        }
        int Key = 0;
        private void DriverDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DrNameTb.Text = DriverDGV.SelectedRows[0].Cells[1].Value.ToString();
            SelectedDriver = DrNameTb.Text;
            PhoneTb.Text = DriverDGV.SelectedRows[0].Cells[2].Value.ToString();
            Dradd.Text = DriverDGV.SelectedRows[0].Cells[3].Value.ToString();
            DOB.Text = DriverDGV.SelectedRows[0].Cells[4].Value.ToString();
            JoinDate.Text = DriverDGV.SelectedRows[0].Cells[5].Value.ToString();
            GenCb.Text = DriverDGV.SelectedRows[0].Cells[6].Value.ToString();
            Rating.Text = DriverDGV.SelectedRows[0].Cells[7].Value.ToString();

            if (DrNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(DriverDGV.SelectedRows[0].Cells[0].Value.ToString());
                CountBookingbyDriver();
                GetStars();
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a Driver");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from DriverTbl where DrId=@DrKey", Con);
                    cmd.Parameters.AddWithValue("@DrKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Driver Deleted");
                    Con.Close();
                    ShowDriver();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (DrNameTb.Text == "" || GenCb.SelectedIndex == -1 || PhoneTb.Text == "" || Rating.SelectedIndex == -1 || Dradd.Text == "" || DOB.Value == DateTime.MinValue || JoinDate.Value == DateTime.MinValue)
            {
                MessageBox.Show("Select A driver ");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update DriverTbl set DrName=@DRN,Drphone = @DrP, Dradd=@DrA , DrDOB=@DrD, DrJoinDate=@DrJ,DrGen=@DrG,DrRating=@DrR where DrId=@DrKey", Con);

                    cmd.Parameters.AddWithValue("@DRN", DrNameTb.Text);
                    cmd.Parameters.AddWithValue("@DRP", PhoneTb.Text);
                    cmd.Parameters.AddWithValue("@DRA", Dradd.Text);
                    cmd.Parameters.AddWithValue("@DRD", DOB.Value.Date); // Assuming DOB is a DateTimePicker control
                    cmd.Parameters.AddWithValue("@DRJ", JoinDate.Value.Date);
                    cmd.Parameters.AddWithValue("@DRG", GenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@DRR", Rating.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@DrKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Driver Updated");
                    Con.Close(); 
                    ShowDriver();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Vehicles Obj = new Vehicles();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Bookings Obj = new Bookings();
            Obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Vehicles Obj = new Vehicles();
            Obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Bookings Obj = new Bookings();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Dashboard Obj = new Dashboard();
            Obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Dashboard Obj = new Dashboard();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thank you for use");
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            this.Hide();
            Obj.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            this.Hide();
            Obj.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Transport Management System");
        }

        private void CountBookingsLbl_Click(object sender, EventArgs e)
        {
            //travel number 
        }
    }
}

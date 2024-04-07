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

namespace Transport_management_system
{
    public partial class Bookings : Form
    {
        public Bookings()
        {
            InitializeComponent();
            GetCustomers();
            ShowBookings();
            GetCars();
            UnameLbl.Text = Login.User; //show user login name
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDriver();
        }
        //database connection
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HP\OneDrive\Documents\TransportDb.mdf;Integrated Security=True;Connect Timeout=30");

        private void GetCustomers()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select * from CustomerTbl", Con);
            SqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);
            CustCb.ValueMember = "CustName";
            CustCb.DataSource = dt;
            Con.Close();
        }

        private void GetDriver()
        {
            if (Con.State == ConnectionState.Closed)
            {
                Con.Open();
            }

            if (VehicleCb.SelectedItem != null) // Check if an item is selected
            {
                string selectedValue = VehicleCb.SelectedValue.ToString();
                string Query = "select * from VehicleTbl where vlp = @SelectedValue";
                SqlCommand cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@SelectedValue", selectedValue);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    DriverTb.Text = dr["Driver"].ToString();
                }
                else
                {
                    DriverTb.Text = "Driver not found";
                }
            }

            Con.Close();
        }

        private void GetCars()
        {
            string IsBooked = "No";
            Con.Open();
            SqlCommand cmd = new SqlCommand("select * from VehicleTbl where Booked='"+IsBooked+"'", Con);
            SqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);
            VehicleCb.ValueMember = "VLP";
            VehicleCb.DataSource = dt;
            Con.Close();
        }
        private void Clear()
        {
            CustCb.SelectedIndex = -1;
            DriverTb.Text = "";
            VehicleCb.SelectedIndex = -1;
            AmountTb.Text = "";
        }

        private void ShowBookings()
        {
            Con.Open();
            string Query = "select * from BookingTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookingDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void UpdateVehicle()  //for update vehicle
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("update VehicleTbl set Booked=@VB where VLP=@VP", Con);
                cmd.Parameters.AddWithValue("@VB", "Yes");
                cmd.Parameters.AddWithValue("@VP", VehicleCb.SelectedValue.ToString());
                cmd.ExecuteNonQuery();
                
                Con.Close();
                
                Clear();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        private void PrintBill(string custName, string vehicle, string driver, DateTime pickupDate, DateTime dropOffDate, string amount)
        {
            StringBuilder billContent = new StringBuilder();
            billContent.AppendLine("--------------- Bill ---------------");
            billContent.AppendLine($"Customer: {custName}");
            billContent.AppendLine($"Vehicle: {vehicle}");
            billContent.AppendLine($"Driver: {driver}");
            billContent.AppendLine($"Pickup Date: {pickupDate.ToShortDateString()}");
            billContent.AppendLine($"Drop-Off Date: {dropOffDate.ToShortDateString()}");
            billContent.AppendLine($"Amount: {amount}");
            billContent.AppendLine("-----------------------------------");

            // Display the bill information in a MessageBox
            MessageBox.Show(billContent.ToString(), "Booking Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            // Implement the code to save a booking to the database
            if (CustCb.SelectedIndex == -1 || VehicleCb.SelectedIndex == -1 || DriverTb.Text == "" || AmountTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into BookingTbl (CustName,Vehicle,Driver,PickupDate,DropOffDate,Amount,BUser) values(@CN,@Veh,@Dri,@PDate,@DDate,@Am,@UN)", Con);
                    cmd.Parameters.AddWithValue("@CN", CustCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Veh", VehicleCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Dri", DriverTb.Text);
                    cmd.Parameters.AddWithValue("@PDate", PickUpDate.Value.Date);
                    cmd.Parameters.AddWithValue("@DDate", RetDate.Value.Date);
                    cmd.Parameters.AddWithValue("@Am", AmountTb.Text);
                    cmd.Parameters.AddWithValue("@UN", UnameLbl.Text);

                    cmd.ExecuteNonQuery();
                    Con.Close();
                    ShowBookings();
                    UpdateVehicle();
                    Clear();
                    PrintBill(CustCb.SelectedValue?.ToString(), VehicleCb.SelectedValue?.ToString(), DriverTb.Text, PickUpDate.Value.Date, RetDate.Value.Date, AmountTb.Text);


                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }


        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Vehicles Obj = new Vehicles();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Driver Obj = new Driver();
            Obj.Show();
            this.Hide();
        }

        private void VehicleCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            this.Hide();
            Obj.Show();
        }

        private void UnameLbl_Click(object sender, EventArgs e)
        {

        }

        private void Bookings_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            Vehicles Obj = new Vehicles();
            Obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Dashboard Obj = new Dashboard();
            Obj.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Transport Management System");
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Dashboard Obj = new Dashboard ();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Driver Obj = new Driver();
            Obj.Show();
            this.Hide();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            this.Hide();
            Obj.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Driver Obj = new Driver();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thank You for use");
        }

        private void label8_Click(object sender, EventArgs e)
        {
            //customer name label
        }

        private void label11_Click(object sender, EventArgs e)
        {
            //vehicle name label
        }

        private void label10_Click(object sender, EventArgs e)
        {
            //driver name label
        }

        private void label9_Click(object sender, EventArgs e)
        {
            //pickupdate label
        }

        private void label18_Click(object sender, EventArgs e)
        {
            //returndate label
        }

        private void label19_Click(object sender, EventArgs e)
        {
            //enter amount label
        }

        private void AmountTb_TextChanged(object sender, EventArgs e)
        {
            //amount text box
        }
    }
}

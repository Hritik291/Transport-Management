using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Transport_management_system
{
    public partial class Vehicles : Form
    {
        public Vehicles()
        {
            InitializeComponent();
            ShowVehicles();
            GetDriver();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HP\OneDrive\Documents\TransportDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void Clear()        //for delete btn
        {
            LPlateTb.Text = "";
            MarkCb.SelectedIndex = -1;
            Modeltb.Text = "";
            VYearCb.SelectedIndex = -1;
            VEngTypeCb.SelectedIndex = -1;
            ColorTb.Text = "";
            MilleageTb.Text = "";
            TypeCb.SelectedIndex = -1;
            BookedCb.SelectedIndex = -1;
        }
        private void ShowVehicles()
        {
            Con.Open();
            string Query = "select * from vehicleTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            VehicleDGV.DataSource = ds.Tables[0];
            Con.Close();

        }
        private void GetDriver()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select * from DriverTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("DrName", typeof(string));
            dt.Load(rdr);
            DriverCb.ValueMember = "DrName";
            DriverCb.DataSource = dt;
            Con.Close();
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (LPlateTb.Text == "" || MarkCb.SelectedIndex == -1 || Modeltb.Text == "" || VYearCb.SelectedIndex == -1 || VEngTypeCb.SelectedIndex == -1 || BookedCb.SelectedIndex == -1 || TypeCb.SelectedIndex == -1 || MilleageTb.Text == "" || ColorTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into VehicleTbl (VLp,Vmark,Vmodel,VYear,VEngType,Booked,VType,VMilleage,VColor,Driver) values(@VP,@Vma,@Vmo,@VY,@VEngType,@B,@VTy,@VMi,@VCo,@Dr)", Con);
                    cmd.Parameters.AddWithValue("@VP", LPlateTb.Text);
                    cmd.Parameters.AddWithValue("@Vma", MarkCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Vmo", Modeltb.Text);
                    cmd.Parameters.AddWithValue("@VY", VYearCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@VEngType", VEngTypeCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@B", BookedCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@VTy", TypeCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@VMi", MilleageTb.Text);
                    cmd.Parameters.AddWithValue("@VCo", ColorTb.Text);
                    cmd.Parameters.AddWithValue("@Dr", DriverCb.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Vehicle Recorded");
                    Con.Close();
                    ShowVehicles();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (LPlateTb.Text == "")
            {
                MessageBox.Show("Select a vehicle");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from VehicleTbl where VLP=@VPlate", Con);
                    cmd.Parameters.AddWithValue("@VPlate", LPlateTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Vehicle Deleted");
                    Con.Close();
                    ShowVehicles();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }
        string V;
        private void CountBookingbyVehicle()
        {
            Con.Open();
            string Query = "select count(*) from BookingTbl where Vehicle='" + V + "'";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            VNumLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void VehicleDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LPlateTb.Text = VehicleDGV.SelectedRows[0].Cells[0].Value.ToString();
            V = LPlateTb.Text;
            CountBookingbyVehicle();
            MarkCb.SelectedItem = VehicleDGV.SelectedRows[0].Cells[1].Value.ToString();
            Modeltb.Text = VehicleDGV.SelectedRows[0].Cells[2].Value.ToString();
            VYearCb.SelectedItem = VehicleDGV.SelectedRows[0].Cells[3].Value.ToString();
            VEngTypeCb.SelectedItem = VehicleDGV.SelectedRows[0].Cells[4].Value.ToString();
            ColorTb.Text = VehicleDGV.SelectedRows[0].Cells[5].Value.ToString();
            MilleageTb.Text = VehicleDGV.SelectedRows[0].Cells[6].Value.ToString();
            TypeCb.SelectedItem = VehicleDGV.SelectedRows[0].Cells[7].Value.ToString();
            BookedCb.SelectedItem = VehicleDGV.SelectedRows[0].Cells[8].Value.ToString();

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (LPlateTb.Text == "" || MarkCb.SelectedIndex == -1 || Modeltb.Text == "" || VYearCb.SelectedIndex == -1 || VEngTypeCb.SelectedIndex == -1 || BookedCb.SelectedIndex == -1 || TypeCb.SelectedIndex == -1 || MilleageTb.Text == "" || ColorTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update VehicleTbl set Vmark=@Vma,Vmodel=@Vmo,VYear=@VY,VEngType=@VEngType,Booked=@B,VType=@VTy,VMilleage=@VMi,VColor=@VCo,Driver=@Dr where VLP=@VP" , Con);
                    cmd.Parameters.AddWithValue("@VP", LPlateTb.Text);
                    cmd.Parameters.AddWithValue("@Vma", MarkCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Vmo", Modeltb.Text);
                    cmd.Parameters.AddWithValue("@VY", VYearCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@VEngType", VEngTypeCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@B", BookedCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@VTy", TypeCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@VMi", MilleageTb.Text);
                    cmd.Parameters.AddWithValue("@VCo", ColorTb.Text);
                    cmd.Parameters.AddWithValue("@Dr", DriverCb.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Vehicle Updated");
                    Con.Close();
                    ShowVehicles();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void MarkCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Bookings Obj = new Bookings();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Driver Obj = new Driver();
            Obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Driver Obj = new Driver();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
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

        private void label7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Transport Management System");
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            this.Hide();
            Obj.Show();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            this.Hide();
            Obj.Show();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
}

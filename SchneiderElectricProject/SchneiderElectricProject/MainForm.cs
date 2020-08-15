using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SchneiderElectricProject.Objects;
using SQLite;


namespace SchneiderElectricProject
{
    public partial class MainForm : Form
    {
        /*As a Backend we will use the library SQLite.NET which is a very basic ORM that allows you to easily
        store and retrieve objects in the local SQLite database on the device.*/

        //Creating database, if it doesn't already exist
        static readonly string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "schneiderElectric.sqlite");


        SQLiteConnection db;

        //this string stores the selected tab
        string TabPageSelected = "Water_meter";


        public MainForm()
        {
            InitializeComponent();
            try
            {

                db = new SQLiteConnection(dbPath);
                btnWaterMeters.BackColor = Color.LimeGreen;


                //Creating tables, if they doesn't already exist
                db.CreateTable<Water_meter>();
                db.CreateTable<Electricity_meter>();
                db.CreateTable<Gateway>();

                Load_data();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error starting the program: "+e.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }


        #region click events
        private void BtnWaterMeters_Click(object sender, EventArgs e)
        {
            TabPageSelected = "Water_meter";
            txtIP.Visible = false;
            txtPort.Visible = false;
            txtIP.Enabled = false;
            txtPort.Enabled = false;
            lblIP.Visible = false;
            lblPort.Visible = false;
            btnWaterMeters.BackColor = Color.LimeGreen;
            btnElectricityMeters.BackColor = Color.DarkSlateGray;
            btnGateways.BackColor = Color.DarkSlateGray;
            Load_data();
        }

        private void BtnElectricityMeters_Click(object sender, EventArgs e)
        {
            TabPageSelected = "Electricity_meter";
            txtIP.Visible = false;
            txtPort.Visible = false;
            txtIP.Enabled = false;
            txtPort.Enabled = false;
            lblIP.Visible = false;
            lblPort.Visible = false;
            btnWaterMeters.BackColor = Color.DarkSlateGray;
            btnElectricityMeters.BackColor = Color.LimeGreen;
            btnGateways.BackColor = Color.DarkSlateGray;
            Load_data();
        }

        private void BtnGateways_Click(object sender, EventArgs e)
        {
            TabPageSelected = "Gateway";
            txtIP.Visible = true;
            txtPort.Visible = true;
            txtIP.Enabled = true;
            txtPort.Enabled = true;
            lblIP.Visible = true;
            lblPort.Visible = true;
            btnWaterMeters.BackColor = Color.DarkSlateGray;
            btnElectricityMeters.BackColor = Color.DarkSlateGray;
            btnGateways.BackColor = Color.LimeGreen;
            Load_data();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMax_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnMax.Visible = false;
            btnRes.Visible = true;
        }

        private void BtnRes_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnMax.Visible = true;
            btnRes.Visible = false;
        }

        private void BtnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.ValidateChildren(ValidationConstraints.Enabled))
                {
                    //If the form validation is correct
                    switch (TabPageSelected)
                    {
                        case "Water_meter":

                            //Before saving the object we check that the serial number is unique
                            TableQuery<Water_meter> query_water = from s in db.Table<Water_meter>() where s.Serial.Equals(txtSerial.Text) select s;
                            if (query_water.Count() == 0)
                            {
                                Water_meter newWater_meter = new Water_meter
                                {
                                    Serial = txtSerial.Text,
                                    Brand = txtBrand.Text,
                                    Model = txtModel.Text
                                };
                                //The object is stored in the database
                                db.Insert(newWater_meter);
                                Load_data();
                            }
                            else
                            {
                                Error_Serial_Exist();
                            }
                            break;

                        case "Electricity_meter":

                            //Before saving the object we check that the serial number is unique
                            TableQuery<Electricity_meter> query_electricity = from s in db.Table<Electricity_meter>() where s.Serial.Equals(txtSerial.Text) select s;
                            if (query_electricity.Count() == 0)
                            {
                                Electricity_meter newElectricity_meter = new Electricity_meter
                                {
                                    Serial = txtSerial.Text,
                                    Brand = txtBrand.Text,
                                    Model = txtModel.Text
                                };
                                //The object is stored in the database
                                db.Insert(newElectricity_meter);
                                Load_data();
                            }
                            else
                            {
                                Error_Serial_Exist();
                            }
                            break;
                        default:

                            //Before saving the object we check that the serial number is unique
                            TableQuery<Gateway> query_gateway = from s in db.Table<Gateway>() where s.Serial.Equals(txtSerial.Text) select s;
                            if (query_gateway.Count() == 0)
                            {
                                Gateway newGateway = new Gateway
                                {
                                    Serial = txtSerial.Text,
                                    Brand = txtBrand.Text,
                                    Model = txtModel.Text,
                                    Ip = txtIP.Text,
                                    Port = txtPort.Text
                                };
                                //The object is stored in the database
                                db.Insert(newGateway);
                                Load_data();
                            }
                            else
                            {
                                Error_Serial_Exist();
                            }
                            break;
                    }

                }
                else
                {
                    MessageBox.Show("Some fields are missing to fill in",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding the item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnResetProgram_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset database? Your data will be deleted and the application will proceed to restart.",
                "Reset Database",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {

                db.Close();
                File.Delete(dbPath);
                Application.Restart();

            }

        }
        #endregion click events

        #region form validation
        private void TxtSerial_Validating(object sender, CancelEventArgs e)
        {
            if (txtSerial.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtSerial, "This field must be filled");
            }
        }
        private void TxtIP_Validating(object sender, CancelEventArgs e)
        {
            if (txtIP.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtIP, "This field must be filled");
            }
        }
        #endregion form validation

        #region extra method
        /// <summary>This method loads the data from the database into the DataGridView
        /// and clears TextBoxes and ErrorProvider</summary>
        private void Load_data()
        {
            try
            {
                switch (TabPageSelected)
                {
                    case "Water_meter":
                        dgv_water_meter.DataSource = db.Table<Water_meter>().ToList();
                        break;
                    case "Electricity_meter":
                        dgv_water_meter.DataSource = db.Table<Electricity_meter>().ToList();
                        break;
                    default:
                        dgv_water_meter.DataSource = db.Table<Gateway>().ToList();
                        break;
                }
                txtSerial.Clear();
                txtBrand.Clear();
                txtModel.Clear();
                txtIP.Clear();
                txtPort.Clear();
                errorProvider1.Clear();
            }            
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}

        /// <summary>This method opens a MessageBox and an ErrorProvider with the following text:
        /// This serial already exist </summary>
        private void Error_Serial_Exist()
        {
            errorProvider1.SetError(txtSerial, "This serial already exist");
            MessageBox.Show("This serial already exist",
                "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        #endregion extra method

        //this code allows you to drag the window by selecting the top green bar
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int IParam);

        private void MainBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
            if (this.WindowState == FormWindowState.Normal)
            {
                btnMax.Visible = true;
                btnRes.Visible = false;
            }
        }


    }
}

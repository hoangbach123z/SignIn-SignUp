using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace SignIn_SignUp
{
    public partial class Signup : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-930V8G9\SQLEXPRESS01;Initial Catalog=LoginData;Integrated Security=True");

        public Signup()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (signup_showPass.Checked)
            {
                signup_password.PasswordChar= '\0' ;
            }
            else
            {
                signup_password.PasswordChar = '*';
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void signup_loginHere_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void signup_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void signup_btn_Click(object sender, EventArgs e)
        {
            Regex mRegxExpression = new Regex(@"^([a-zA-Z0-9_\-])([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$");
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            if (signup_email.Text == "" || signup_username.Text == ""
                || signup_password.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!mRegxExpression.IsMatch(signup_email.Text.Trim()))
            {
                MessageBox.Show("E-mail address format is not correct.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                signup_email.Focus();
            }            
            else if(!hasLowerChar.IsMatch(signup_password.Text))
            {
                
                MessageBox.Show("Password should contain at least one lower case letter.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                signup_password.Focus();
            }
            else if (!hasUpperChar.IsMatch(signup_password.Text))
            {
                MessageBox.Show("Password should contain at least one upper case letter.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                signup_password.Focus();
            }
            else if (!hasMiniMaxChars.IsMatch(signup_password.Text))
            {
                MessageBox.Show("Password should not be lesser than 8 or greater than 15 characters.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                signup_password.Focus();
            }
            else if (!hasNumber.IsMatch(signup_password.Text))
            {
                MessageBox.Show("Password should contain at least one numeric value.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                signup_password.Focus();
            }

            else if (!hasSymbols.IsMatch(signup_password.Text))
            {
                MessageBox.Show("Password should contain at least one special case character.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                signup_password.Focus();
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();
                        String checkUsername = "SELECT * FROM admin WHERE username = '"
                            + signup_username.Text.Trim() + "'"; // admin is our table name

                        using (SqlCommand checkUser = new SqlCommand(checkUsername, connect))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(checkUser);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                MessageBox.Show(signup_username.Text + " is already exist", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string insertData = "INSERT INTO admin (email, username, password, created_date) " +
                                    "VALUES(@email, @username, @password, @created_date)";

                                DateTime date = DateTime.Today;

                                using (SqlCommand cmd = new SqlCommand(insertData, connect))
                                {
                                    cmd.Parameters.AddWithValue("@email", signup_email.Text.Trim());
                                    cmd.Parameters.AddWithValue("@username", signup_username.Text.Trim());
                                    cmd.Parameters.AddWithValue("@password", signup_password.Text.Trim());
                                    cmd.Parameters.AddWithValue("@created_date", date);

                                    cmd.ExecuteNonQuery();

                                    MessageBox.Show("Registered successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // TO SWITCH THE FORM 
                                    Form1 lForm = new Form1();
                                    lForm.Show();
                                    this.Hide();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting Database: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }

            }
        }

        private void signup_email_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void signup_password_TextChanged(object sender, EventArgs e)
        {
            

        }

    }
}

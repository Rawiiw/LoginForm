using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Data.SqlClient;

namespace UsersApp
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        string connectionString = @"Data Source=DBSRV\MAX2022;Initial Catalog=Demo;Integrated Security=True";
        public AuthWindow()
        {
            InitializeComponent();

            DoubleAnimation btnAnimation = new DoubleAnimation();
            DoubleAnimation btnAnimation1 = new DoubleAnimation();
            btnAnimation.From = 0;
            btnAnimation.To = 268;
            btnAnimation.Duration = TimeSpan.FromSeconds(3);
            SignInBtn.BeginAnimation(Button.WidthProperty, btnAnimation);

            btnAnimation1.From = 0;
            btnAnimation1.To = 80;
            btnAnimation1.Duration = TimeSpan.FromSeconds(3);
            SignUpBtn.BeginAnimation(Button.WidthProperty, btnAnimation1);

        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginLabel.Text.Trim();
            string pass = PassLabel.Password.Trim();

            if (login.Length < 5)
            {
                LoginLabel.ToolTip = "Login must be more than 5 characters!";
                LoginLabel.Background = Brushes.MediumPurple;
            }
            else if (pass.Length < 5)
            {
                PassLabel.ToolTip = "Password must be more than 5 characters!";
                PassLabel.Background = Brushes.MediumPurple;
            }
            else
            {
                LoginLabel.ToolTip = "";
                LoginLabel.Background = Brushes.Transparent;
                PassLabel.ToolTip = "";
                PassLabel.Background = Brushes.Transparent;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT * FROM UserLog WHERE Login = @login AND Password = @password", connection);
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", pass);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string userLogin = reader["Login"].ToString();
                            string userPassword = reader["Password"].ToString();

                            if (userLogin == "Admin")
                            {
                                MessageBox.Show("Welcome Admin!");
                                Moderator adminWin = new Moderator();
                                adminWin.Show();
                                this.Hide();
                                return;
                            }
                            else
                            {
                                MessageBox.Show("User has been found successfully!");
                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                this.Hide();
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to find user.");
                    }
                }
            }
        }


        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Auth.Close();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void LoginLabel_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

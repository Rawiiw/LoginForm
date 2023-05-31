using MongoDB.Driver.Core.Configuration;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UsersApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // string connectionString = @"Data Source=DBSRV\MAX2022;Initial Catalog=DemoKosinova;Integrated Security=True";
        string connectionString = @"Data Source=DBSRV\MAX2022;Initial Catalog=Demo;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();

         

            DoubleAnimation btnAnimation = new DoubleAnimation
            {
                From = 0,
                To = 268,
                Duration = TimeSpan.FromSeconds(3)
            };
            regButton.BeginAnimation(Button.WidthProperty,btnAnimation);

        }

            private void Button_Reg_Click(object sender, RoutedEventArgs e)
            {
                string login = LoginLabel.Text.Trim();
                string pass = PassLabel.Password.Trim();
                string email = EmailLabel.Text.Trim();

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

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            SqlCommand command = new SqlCommand("INSERT INTO UserLog (Login, Password, Email) VALUES (@login, @password, @email)", connection);
                            command.Parameters.AddWithValue("@login", login);
                            command.Parameters.AddWithValue("@password", pass);
                            command.Parameters.AddWithValue("@email", email);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 1)
                        {
                            MessageBox.Show("Please enter captha!");
                            CapchaPage capchaPage = new CapchaPage();
                            capchaPage.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Failed to add user.");
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while adding the user. Error message: " + ex.Message);
                    }
                }
            }

        
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            App.Close();
        }

        private void Button_Window_Auth_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Hide();
        }
    }
}


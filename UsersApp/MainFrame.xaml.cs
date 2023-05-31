using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
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

namespace UsersApp
{
    /// <summary>
    /// Логика взаимодействия для MainFrame.xaml
    /// </summary>
    public partial class MainFrame : Window
    {
        string connectionString = @"Data Source=DBSRV\MAX2022;Initial Catalog=Demo;Integrated Security=True";

        public MainFrame()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Создаем команду для выборки данных
                SqlCommand command = new SqlCommand("SELECT * FROM Events", connection);

                // Создаем адаптер данных и заполняем DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Привязываем данные к EventsGrid
                EventsGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow autWindow = new AuthWindow();
            autWindow.Show();
            this.Hide();
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
          MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {

            int selectedDirectionId = (int)DirectionFilter.SelectedValue;
            DateTime selectedDate = DateFilter.SelectedDate.Value;

            // Создаем соединение с базой данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Создаем команду для выборки данных
                SqlCommand command = new SqlCommand($"SELECT * FROM Events WHERE start_date = '{selectedDate}' AND moderator_id = {selectedDirectionId}", connection);

                // Создаем адаптер данных и заполняем DataSet
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                // Привязываем данные к DataGrid
                EventsGrid.ItemsSource = dataSet.Tables[0].DefaultView;
            }
        }


            private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EventsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

            private void DirectionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                // Создаем соединение с базой данных
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Создаем команду для выборки данных
                    SqlCommand command = new SqlCommand("SELECT direction_id, direction_name FROM Directions", connection);

                    // Создаем адаптер данных и заполняем DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Привязываем данные к DirectionFilter
                    DirectionFilter.ItemsSource = dataTable.DefaultView;
                    DirectionFilter.DisplayMemberPath = "direction_name";
                    DirectionFilter.SelectedValuePath = "direction_id";
                }
            }

        }

    }
    



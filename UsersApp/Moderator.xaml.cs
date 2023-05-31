using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Configuration;
using NUnit.Framework.Internal.Execution;
using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
using UsersApp.Properties;

namespace UsersApp
{
    /// <summary>
    /// Логика взаимодействия для Moderator.xaml
    /// </summary>
    public partial class Moderator : Window
    {
        readonly Settings settings = new Settings();
        public Moderator()
        {
            
            InitializeComponent();
            var currentTime = DateTime.Now.TimeOfDay;
            if (currentTime >= new TimeSpan(9, 0, 0) && currentTime <= new TimeSpan(11, 0, 0))
                GreetLab.Content = $"Доброе утро, {Environment.UserName}!";
            else if (currentTime > new TimeSpan(11, 0, 0) && currentTime <= new TimeSpan(18, 0, 0))
                GreetLab.Content = $"Добрый день, {Environment.UserName}!";
            else
                GreetLab.Content = $"Добрый вечер, {Environment.UserName}!";
            using (SqlConnection connection = new SqlConnection(settings.DemoConnectionString))
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






        private void DeleteEventButton_Click_1(object sender, RoutedEventArgs e)
        {
            // Получаем выделенную запись в EventsGrid
            DataRowView selectedRow = (DataRowView)EventsGrid.SelectedItem;

            if (selectedRow != null)
            {
                // Получаем ID записи и удаляем ее из таблицы Events
                Guid event_id = (Guid)selectedRow["event_id"];
                using (SqlConnection connection = new SqlConnection(settings.DemoConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Events WHERE event_id = @event_id", connection);
                    command.Parameters.AddWithValue("@event_id", event_id);
                    int rowsAffected = command.ExecuteNonQuery();

                    // Если удалена хотя бы одна запись, обновляем данные в EventsGrid
                    if (rowsAffected > 0)
                    {
                        // Создаем команду для выборки данных
                        command = new SqlCommand("SELECT * FROM Events", connection);

                        // Создаем адаптер данных и заполняем DataTable
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Привязываем данные к EventsGrid
                        EventsGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
        }

        private void SendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(settings.DemoConnectionString))
            {
                connection.Open();

                // Создаем команду для вставки новой записи в таблицу Events
                SqlCommand command = new SqlCommand("INSERT INTO Events (event_id, event_name, start_date) VALUES (NEWID(), @event_name, @start_date)", connection);
                command.Parameters.AddWithValue("@event_name", EventNameTextBox.Text);
                command.Parameters.AddWithValue("@start_date", EventDatePicker.SelectedDate);

                // Выполняем команду и получаем количество добавленных записей
                int rowsAffected = command.ExecuteNonQuery();

                // Если добавлено хотя бы одна запись, обновляем данные в EventsGrid
                if (rowsAffected > 0)
                {
                    // Создаем команду для выборки данных
                    command = new SqlCommand("SELECT * FROM Events", connection);

                    // Создаем адаптер данных и заполняем DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Привязываем данные к EventsGrid
                    EventsGrid.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); 
        }
    }
}






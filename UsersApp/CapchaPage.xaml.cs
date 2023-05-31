using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace UsersApp
{
    /// <summary>
    /// Логика взаимодействия для CapchaPage.xaml
    /// </summary>
    public partial class CapchaPage : Window
    {
        public CapchaPage()
        {
            InitializeComponent();
            DisplayCaptcha();
            GenerateCaptchaText();
            TextBox CapchaInput = new TextBox();
            string CapchaText = CapchaInput.Text;
            GenerateCaptchaImage(CapchaText);
            DoubleAnimation btnAnimation = new DoubleAnimation
            {
                From = 0,
                To = 268,
                Duration = TimeSpan.FromSeconds(3)
            };
            Enterbtn.BeginAnimation(Button.WidthProperty, btnAnimation);
            File.Create("@captcha.json");
        }

        private void DisplayCaptcha()
        {
            TextBox captchaInput = new TextBox
            {
                Text = GenerateCaptchaText()
            };
            var captchaImage = GenerateCaptchaImage(captchaInput.Text);
            CaptchaImage.Source = captchaImage;
            var captchaJson = new JObject
            {
                ["text"] = captchaInput.Text
            };
            File.WriteAllText(@"captcha.json", captchaJson.ToString());
        }

        private string GenerateCaptchaText()
        {

            var captchaText = "";
            var random = new Random();
            for (var i = 0; i < 4; i++)
                captchaText += (char)random.Next('a', 'z' + 1);
            return captchaText;
        }

        private BitmapSource GenerateCaptchaImage(string captchaText)
        {
            // Create bitmap image object
            var bitmap = new Bitmap(150, 50);

            // Create graphics object from the bitmap
            var graphics = Graphics.FromImage(bitmap);

            // Draw captcha text on the image
            var font = new Font("Arial", 24, System.Drawing.FontStyle.Bold);
            var textBrush = new SolidBrush(System.Drawing.Color.Black);
            var textPosition = new System.Drawing.Point(10, 5);
            graphics.DrawString(captchaText, font, textBrush, textPosition);

            // Create noise for the image (optional)
            var random = new Random();
            for (var i = 0; i < 300; i++)
            {
                var x = random.Next(bitmap.Width);
                var y = random.Next(bitmap.Height);
                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)));
            }

            // Create bitmap source from the bitmap
            var handle = bitmap.GetHbitmap();
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();

            // Dispose objects
            bitmap.Dispose();
            graphics.Dispose();
            textBrush.Dispose();
            font.Dispose();

            return bitmapSource;
        }


        //Обработка нажатия кнопки "Login" и проверка введенной капчи:

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            _ = new TextBox();
            // Get captcha text from json file
            JObject captchaJson = JObject.Parse(File.ReadAllText(@"captcha.json"));
            var captchaText = captchaJson["text"].ToString();

            // Get user input for captcha
            var userInput = CaptchaInput.Text;

            // Check if captcha is correct
            if (userInput.ToLower() == captchaText.ToLower())
            {
                MessageBox.Show("You are not a robot!");
                // Captcha is correct, redirect user to home page
                UserPageWindow userPage = new UserPageWindow();
                userPage.Show();
                this.Hide();
               
            }
            else
            {
                MessageBox.Show("Incorrect Captcha!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               
                
            }
        }
        

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            File.Delete(@"captcha.json");
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DisplayCaptcha();
            GenerateCaptchaText();
            TextBox CapchaInput = new TextBox();
            string CapchaText = CapchaInput.Text;
            GenerateCaptchaImage(CapchaText);
        }
    }
}

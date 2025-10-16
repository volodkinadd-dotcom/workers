using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Workers
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Worker> Workers { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Workers = new ObservableCollection<Worker>();
            dgWorkers.ItemsSource = Workers;
            UpdateCount();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            var worker = new Worker
            {
                FullName = txtFullName.Text.Trim(),
                BirthDate = txtBirthDate.Text.Trim(),
                Age = txtAge.Text.Trim(),
                INN = txtINN.Text.Trim(),
                Passport = txtPassport.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Gender = rbMale.IsChecked == true ? "М" : "Ж"
            };

            Workers.Add(worker);
            ClearForm();
            UpdateCount();
            txtError.Text = "Сотрудник добавлен";
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgWorkers.SelectedItem is Worker emp)
            {
                Workers.Remove(emp);
                UpdateCount();
            }
        }

        private bool Validate()
        {
            // ФИО
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowError("Введите ФИО");
                return false;
            }

            // Дата рождения
            var birthDate = txtBirthDate.Text.Trim();
            if (!Regex.IsMatch(birthDate, @"^\d{2}\.\d{2}\.\d{4}$"))
            {
                ShowError("Дата рождения: дд.мм.гггг");
                return false;
            }

            // Возраст
            if (string.IsNullOrWhiteSpace(txtAge.Text))
            {
                ShowError("Введите возраст");
                return false;
            }

            // ИНН
            var inn = txtINN.Text.Trim();
            if (inn.Length != 12 || !long.TryParse(inn, out _))
            {
                ShowError("ИНН должен быть 12 цифр");
                return false;
            }

            // Паспорт
            var passport = txtPassport.Text.Trim();
            if (passport.Length != 11 || !Regex.IsMatch(passport, @"^\d{4}\s\d{6}$"))
            {
                ShowError("Паспорт: 1234 567890");
                return false;
            }

            // Телефон
            var phone = txtPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                ShowError("Введите телефон");
                return false;
            }

            // Пол
            if (!rbMale.IsChecked.Value && !rbFemale.IsChecked.Value)
            {
                ShowError("Выберите пол");
                return false;
            }

            // Дубликаты
            foreach (var emp in Workers)
            {
                if (emp.INN == inn || emp.Passport == passport)
                {
                    ShowError("Такой ИНН или паспорт уже есть");
                    return false;
                }
            }

            return true;
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
        }

        private void ClearForm()
        {
            txtFullName.Text = "";
            txtBirthDate.Text = "";
            txtAge.Text = "";
            txtINN.Text = "";
            txtPassport.Text = "";
            txtPhone.Text = "";
            rbMale.IsChecked = rbFemale.IsChecked = false;
            txtError.Text = "";
        }

        private void UpdateCount()
        {
            txtCount.Text = $"Сотрудников: {Workers.Count}";
        }
    }
}
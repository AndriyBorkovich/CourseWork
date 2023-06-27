using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace CourseWork
{
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();
            isListSaved = false;
        }

        // змінна, яка визначає чи список було збережено у файл
        private bool isListSaved;
        
        // зберігаємо список у текстовий файл
        private void WriteToFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            var warriors = this.warriorsTable.ItemsSource.Cast<Warrior>().ToList();//конвертуємо список з таблиці в змінну
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save list as"
            };
            try
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    var filePath = saveFileDialog.FileName;
                    if (!filePath.Contains("txt"))
                        throw new FileFormatException("Вибрано не текстовий файл!\nБудь ласка, виберіть файл з потрібним розширенням - txt");
                    bool result = false;
                    File.AppendAllText(filePath, $"{labelTitle.Content}\n");
                    string columnsTitles = "|    Прізвище     |      Ім‘я      |Вік|Група крові|    Звання  |         Боєкомплект          |  \n";
                    File.AppendAllText(filePath, columnsTitles);
                    foreach (var warrior in warriors) result = warrior + filePath;
                    isListSaved = result;
                    MessageBox.Show("Список бійців було успішно записано до файлу!", "Вітання!", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    throw new FileNotFoundException(
                        "Не вдалося створити файл для збереження списку!\nБудь ласка, спробуйте ще раз");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Помилка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        //обробник події закриття вікна
        private void SecondWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!isListSaved)
            {
                MessageBoxResult clickResult = MessageBox.Show("Список не записаний у файл! Дійсно бажаєте вийти?", "Увага!",
                    MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (clickResult == MessageBoxResult.No) e.Cancel = true;
            }
        }
        //завантаження номеру поточного рядка
        private void WarriorsTable_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}

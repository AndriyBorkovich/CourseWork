using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CourseWork
{
    public partial class AddWarriorWindow : Window
    {
        public AddWarriorWindow()
        {
            InitializeComponent();
            isWarriorAdded = false;
        }

        private bool isWarriorAdded;

        // зчитування даних про бійця з форми і додовання його до загального списку
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.nameBox.Text == string.Empty || this.surnameBox.Text == string.Empty || this.ageBox.Text == string.Empty)
            {
                MessageBox.Show(this, "Обов'язкові поля залишились пустими!\n" +
                                      "Please, fulfill them!", "Attention!", MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                string name = nameBox.Text, surname = surnameBox.Text;
                if (!name.All(char.IsLetter) || !surname.All(char.IsLetter))
                {
                    throw new FormatException("Прізвище або ім'я містить заборонені символи!");
                }

                int age = Convert.ToInt32(ageBox.Text);
                if (age < 18 || age > 65)
                {
                    throw new FormatException("Вік бійця не придатний для служби!");
                }

                BloodGroups bloodGroup = new BloodGroups();
                
                // конвертуємо групу крові, вибрану користувачем з випадного списку
                switch (this.bloodGroupsBox.Text)
                {
                    case "I-":
                        {
                            bloodGroup = BloodGroups.OMinus;
                            break;
                        }

                    case "I+":
                        {
                            bloodGroup = BloodGroups.OPlus;
                            break;
                        }

                    case "II-":
                        {
                            bloodGroup = BloodGroups.AMinus;
                            break;
                        }

                    case "II+":
                        {
                            bloodGroup = BloodGroups.APlus;
                            break;
                        }

                    case "III-":
                        {
                            bloodGroup = BloodGroups.BMinus;
                            break;
                        }

                    case "III+":
                        {
                            bloodGroup = BloodGroups.BPlus;
                            break;
                        }

                    case "IV-":
                        {
                            bloodGroup = BloodGroups.AbMinus;
                            break;
                        }

                    case "IV+":
                        {
                            bloodGroup = BloodGroups.AbPlus;
                            break;
                        }
                }

                // конвертуємо вибране користувачем звання з випадного списку
                Ranks rank = (Ranks)Enum.Parse(typeof(Ranks), ranksBox.Text);
                List<string> ammunitionList = new List<string>();
                ammunitionList.Capacity = 0;
                if (ammoBox.Text != string.Empty)
                {
                    ammunitionList = ammoBox.Text.Split(',').ToList();
                }

                // доступаємось до головного вікна проекту
                var window = Application.Current.MainWindow;
                if (window is MainWindow mainWindow)
                {
                    Warrior newWarrior = new(name, surname, age, bloodGroup, ammunitionList, rank);

                    // доступаємось до елементів головної форми

                    mainWindow.Warriors.Add(newWarrior); // додаємо цього бійця до загального списку
                    mainWindow.Table.ItemsSource = mainWindow.Warriors; // встановлюємо оновлений список як джерело даних для таблиці
                    mainWindow.Table.Items.Refresh();
                    isWarriorAdded = true;
                    Close();

                    MessageBox.Show(
                        this,
                        $"Warrior {surname} {name} was added to батальйону successfully!",
                        "Congratulations!",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Information);

                    if (!string.IsNullOrEmpty(mainWindow.FilePathToWrite))
                    {
                        File.AppendAllText(mainWindow.FilePathToWrite, "Список після додавання бійця:\n");
                        string columnsTitles = "|    Прізвище     |      Ім‘я      |Вік|Група крові|    Звання  |         Боєкомплект          |  \n";
                        File.AppendAllText(mainWindow.FilePathToWrite, columnsTitles);
                        bool result = false;
                        foreach (var warrior in mainWindow.Warriors)
                        {
                            result = warrior + mainWindow.FilePathToWrite;
                        }

                        mainWindow.IsListSaved = result;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        // забезпечити ввід лише цифр в поле для введення віку бійця
        private void AgeBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddWarriorWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!this.isWarriorAdded)
            {
                MessageBoxResult clickResult = MessageBox.Show(
                    "Warrior won't be added to list!\n " + "Do you really about to left?",
                    "Attention!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);
                if (clickResult == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

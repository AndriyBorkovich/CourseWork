namespace CourseWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Microsoft.Win32;

    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Warriors = new List<Warrior>();
            IsListSaved = true;
        }

        /// <summary>
        /// The warriors list to operate with.
        /// </summary>
        internal List<Warrior> Warriors;

        /// <summary>
        /// The file path to write the list of warriors.
        /// </summary>
        internal string FilePathToWrite;

        /// <summary>
        /// check if list is saved to file.
        /// </summary>
        internal bool IsListSaved; 
        
        // event handler for closing window
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!this.IsListSaved || this.Warriors.Count != 0)
            {
                MessageBoxResult clickResult = MessageBox.Show(
                    "List isn't saved to file! Are you really want to exit?",
                    "Attention!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);
                if (clickResult == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        // read warriors list from file
        private void ReadFromFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new ()
                                                {
                                                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                                                          Multiselect = true,
                                                          CheckFileExists = true,
                                                          Title = "Choose the list of warriors"
                                                };
            try
            {
                string filePathToRead;
                if (openFileDialog.ShowDialog() == true)
                {
                    filePathToRead = openFileDialog.FileName;
                    if (!Path.GetExtension(filePathToRead).Contains(".txt"))
                    {
                        throw new FileFormatException(
                            "You didn't choose a text file\nPlease, choose file with needed extension (*.txt)");
                    }
                }
                else
                {
                    throw new FileNotFoundException("Can't open the file!\nTry again!");
                }

                Warrior warrior = new ();
                this.Warriors.AddRange(warrior - filePathToRead);
                Table.ItemsSource = this.Warriors;
                Table.Items.Refresh();
                statusBar.Text = "List of warriors was successfully read from file!";
                MessageBox.Show(
                    this,
                    "List of warriors was successfully read from file!",
                    "Congratulations!",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Information);
                this.IsListSaved = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        // вивід списку бійців у файл
        private void MiWriteToFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Warriors.Count == 0)
            {
                MessageBox.Show(
                    this,
                    "List of warriors is empty!\nNothing to write in file!",
                    "Attention!",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new ()
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save list of warriors"
            }; 
            try
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    this.FilePathToWrite = saveFileDialog.FileName;
                    if (!this.FilePathToWrite.Contains("txt"))
                    {
                        throw new FileFormatException(
                            "You didn't choose a text file\nPlease, choose file with needed extension (*.txt)");
                    }

                    var result = false;
                    string columnsTitles = "|    Прізвище     |      Ім‘я      |Вік|Група крові|    Звання  |         Боєкомплект          |  \n";
                    File.AppendAllText(this.FilePathToWrite, columnsTitles);

                    foreach (var warrior in this.Warriors)
                    {
                        result = warrior + this.FilePathToWrite;
                    }

                    this.IsListSaved = result;
                    statusBar.Text = "List of warriors was written to file successfully!";
                    MessageBox.Show(
                        "List of warriors was written to file successfully",
                        "Congratulations!",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Information);
                }
                else
                {
                    throw new FileNotFoundException(
                        "Can't create/find file to save the list!\nPlease, try again");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        // сортування бійців за званням
        private void MiSortByRank_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Warriors.Count == 0)
            {
                MessageBox.Show(
                    "List is empty!\nPlease, fulfill it!",
                    "Attention!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            ListOfWarriorsOperations.SortByRank(this.Warriors);
            MessageBox.Show(
                this,
                "List of warriors was sorted successfully!",
                "Congratulations!",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Information);
            Table.Items.Refresh();
            Table.ItemsSource = this.Warriors;
            statusBar.Text = "List of warriors was sorted successfully by rank!";
            if (!string.IsNullOrEmpty(this.FilePathToWrite))
            {
                File.AppendAllText(this.FilePathToWrite, "Список, посортований за званнями:\n");
                string columnsTitles = "|    Прізвище     |      Ім‘я      |Вік|Група крові|    Звання  |         Боєкомплект          |  \n";
                File.AppendAllText(this.FilePathToWrite, columnsTitles);
                bool result = false;
                foreach (var warrior in this.Warriors)
                {
                    result = warrior + this.FilePathToWrite;
                }

                this.IsListSaved = result;
            }
        }

        /// <summary>
        /// The mi add warrior_ on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MiAddWarrior_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                AddWarriorWindow w = new ();
                w.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        // підрахувати кількість озброєння виданого бійцям (для кожного звання)
        private void CountAmmunition_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Warriors.Count == 0)
            {
                MessageBox.Show(
                    "Список пустий!\nБудь ласка, наповніть його!",
                    "Увага!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int recruitsWeapons = ListOfWarriorsOperations.CountAmmunition(ListOfWarriorsOperations.GetRecruits(this.Warriors)),
                soldiersWeapons = ListOfWarriorsOperations.CountAmmunition(ListOfWarriorsOperations.GetSoldiers(this.Warriors)),
                sergeantsWeapons = ListOfWarriorsOperations.CountAmmunition(ListOfWarriorsOperations.GetSergeants(this.Warriors)),
                lieutenantsWeapons = ListOfWarriorsOperations.CountAmmunition(ListOfWarriorsOperations.GetLieutenants(this.Warriors)),
                captainsWeapons = ListOfWarriorsOperations.CountAmmunition(ListOfWarriorsOperations.GetCaptains(this.Warriors)),
                mayorsWeapons = ListOfWarriorsOperations.CountAmmunition(ListOfWarriorsOperations.GetMayors(this.Warriors)),
                lieutenantsColonelsWeapons = ListOfWarriorsOperations.CountAmmunition(ListOfWarriorsOperations.GetLieutenantColonels(this.Warriors)),
                allWarriorsWeapons = ListOfWarriorsOperations.CountAmmunition(this.Warriors);
            var text = $"Одиниць озброєння:\nРекрути: {recruitsWeapons} од.\nСолдати: {soldiersWeapons} од.\nСержанти: {sergeantsWeapons} од.\n" +
                       $"Лейтенанти: {lieutenantsWeapons} од.\nКапітани: {captainsWeapons} од.\nМайори: {mayorsWeapons} од.\n" +
                       $"Підполковники: {lieutenantsColonelsWeapons} од.\nЗагалом: {allWarriorsWeapons} од.\n";
            if (!string.IsNullOrEmpty(this.FilePathToWrite))
            {
                File.AppendAllText(this.FilePathToWrite, text);
            }

            MessageBox.Show(text, "Батальйон", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // підрахувати середній вік для кожного звання і загалом для батальйону
        private void CountAverageAge_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Warriors.Count == 0)
            {
                MessageBox.Show(
                    "List is empty!\nPlease, fulfill it!",
                    "Attention!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            double recruitsAverageAge = ListOfWarriorsOperations.CalculateAverageAge(ListOfWarriorsOperations.GetRecruits(this.Warriors)),
                soldiersAverageAge = ListOfWarriorsOperations.CalculateAverageAge(ListOfWarriorsOperations.GetSoldiers(this.Warriors)),
                sergeantsAverageAge = ListOfWarriorsOperations.CalculateAverageAge(ListOfWarriorsOperations.GetSergeants(this.Warriors)),
                lieutenantsAverageAge = ListOfWarriorsOperations.CalculateAverageAge(ListOfWarriorsOperations.GetLieutenants(this.Warriors)),
                captainsAverageAge = ListOfWarriorsOperations.CalculateAverageAge(ListOfWarriorsOperations.GetCaptains(this.Warriors)),
                mayorsAverageAge = ListOfWarriorsOperations.CalculateAverageAge(ListOfWarriorsOperations.GetMayors(this.Warriors)),
                lieutenantsColonelAverageAge = ListOfWarriorsOperations.CalculateAverageAge(ListOfWarriorsOperations.GetLieutenantColonels(this.Warriors)),
                allWarriorsAverageAge = ListOfWarriorsOperations.CalculateAverageAge(this.Warriors);
            var text = $"Середній вік для:\nРекрутів: {recruitsAverageAge} р.\nСолдатів: {soldiersAverageAge} р.\nСержантів: {sergeantsAverageAge} р.\n" +
                       $"Лейтенантів: {lieutenantsAverageAge} р.\nКапітанів: {captainsAverageAge} р.\nМайорів: {mayorsAverageAge} р.\n" +
                       $"Підполковників: {lieutenantsColonelAverageAge} р.\nВсіх бійців: {allWarriorsAverageAge} р.\n";
            if (!string.IsNullOrEmpty(this.FilePathToWrite))
            {
                File.AppendAllText(this.FilePathToWrite, text);
            }

            MessageBox.Show(this, text, "Battalion", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Бійці, які мають більше однієї одиниці зброї
        private void Mi1_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Warriors.Count == 0)
            {
                MessageBox.Show(
                    "List is empty!\nPlease, fulfill it!",
                    "Attention!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (ListOfWarriorsOperations.GetWithMoreThanOneWeapon(this.Warriors).Count == 0)
            {
                MessageBox.Show(
                    this,
                    "Бійців за даною ознакою немає",
                    "Увага!",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Exclamation);
                return;
            }

            SecondWindow warriorsWindow = new ()
                                              {
                                                  labelTitle =
                                                      {
                                                          Content = "Бійці, які мають більше однієї одиниці зброї"
                                                      },
                                                  warriorsTable =
                                                      {
                                                          ItemsSource = ListOfWarriorsOperations.GetWithMoreThanOneWeapon(this.Warriors)
                                                      }
                                              };
            warriorsWindow.Show();
        }

        // Бійці, які мають АК-47 і гранатомет
        private void Mi2_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Warriors.Count == 0)
            {
                MessageBox.Show(
                    "List is empty!\nPlease, fulfill it!",
                    "Attention!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (ListOfWarriorsOperations.GetWithAk47AndGrenadeGun(this.Warriors).Count == 0)
            {
                MessageBox.Show(
                    this,
                    "Бійців за даною ознакою немає",
                    "Увага!",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Exclamation);
                return;
            }

            SecondWindow warriorsWindow = new ()
            {
                warriorsTable =
                {
                    ItemsSource = ListOfWarriorsOperations.GetWithAk47AndGrenadeGun(this.Warriors)
                },
                labelTitle =
                {
                    Content = "Бійці, які мають АК-47 і гранатомет"
                }
            };
            warriorsWindow.Show();
        }

        // Бійці, які мають  пістолет і будь-який автомат
        private void Mi3_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Warriors.Count == 0)
            {
                MessageBox.Show(
                    "List is empty!\nPlease, fulfill it!", 
                    "Attention!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (ListOfWarriorsOperations.GetWithGunAndAnyRifle(this.Warriors).Count == 0)
            {
                MessageBox.Show("Бійців за даною ознакою немає", "Увага!", MessageBoxButton.OKCancel,
                    MessageBoxImage.Exclamation);
                return;
            }

            SecondWindow warriorsWindow = new ()
            {
                labelTitle =
                {
                    Content = "Бійці, які мають  пістолет і будь-який автомат"
                },
                warriorsTable =
                {
                    ItemsSource = ListOfWarriorsOperations.GetWithGunAndAnyRifle(this.Warriors)
                }
            };
            warriorsWindow.Show();
        }

        // Бійці, які мають першу групу крові резус мінус і четверту групу резус плюс віком до 25 років
        private void Mi4_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Warriors.Count == 0)
            {
                MessageBox.Show(
                    "List is empty!\nPlease, fulfill it!", 
                    "Attention!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (ListOfWarriorsOperations.GetWithSpecificBloodGroupAndAge(this.Warriors).Count == 0)
            {
                MessageBox.Show(
                    "Бійців за даною ознакою немає",
                    "Увага!",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Exclamation);
                return;
            }

            SecondWindow warriorsWindow = new ()
            {
                labelTitle =
                {
                    Content = "Бійці, які мають першу групу крові резус мінус або четверту групу резус плюс віком до 25 років"
                },
                warriorsTable =
                {
                    ItemsSource = ListOfWarriorsOperations.GetWithSpecificBloodGroupAndAge(this.Warriors)
                }
            };
            warriorsWindow.Show();
        }

        // Солдати віком до 20 років без боєкомплекту
        private void Mi5_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Warriors.Count == 0)
            {
                MessageBox.Show(
                    "List is empty!\nPlease, fulfill it!",
                    "Attention!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (ListOfWarriorsOperations.GetSoldiersWithNoWeaponsAndYoungerThan20Years(this.Warriors).Count == 0)
            {
                MessageBox.Show(
                    "Бійців за даною ознакою немає",
                    "Увага!",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Exclamation);
                return;
            }

            SecondWindow warriorsWindow = new ()
            {
                labelTitle =
                {
                    Content = "Cолдати віком до 20 років без боєкомплекту"
                },
                warriorsTable =
                {
                    ItemsSource = ListOfWarriorsOperations.GetSoldiersWithNoWeaponsAndYoungerThan20Years(this.Warriors)
                }
            };
            warriorsWindow.Show();
        }

        // Донори для кожної групи крові
        private void Mi6_OnClick(object sender, RoutedEventArgs e)
        {
            if (Warriors.Count == 0)
            {
                MessageBox.Show(
                    "List is empty!\nPlease, fulfill it!", "Attention!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            else
            {
                List<Warrior>[] donors = new List<Warrior>[4];
                donors[0] = ListOfWarriorsOperations.GetDonorsForFirstBloodGroup(this.Warriors);
                donors[1] = ListOfWarriorsOperations.GetDonorsForSecondBloodGroup(this.Warriors);
                donors[2] = ListOfWarriorsOperations.GetDonorsForThirdBloodGroup(this.Warriors);
                donors[3] = ListOfWarriorsOperations.GetDonorsForFourthBloodGroup(this.Warriors);
                SecondWindow[] windows = new SecondWindow[4];
                for (int i = 0; i < 4; i++)
                {
                    if (donors[i].Count == 0)
                    {
                        MessageBox.Show(
                            $"На жаль, донорів для {i + 1} групи крові немає",
                            "Увага!",
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Warning);
                    }
                    else
                    {
                        windows[i] = new SecondWindow
                        {
                            labelTitle =
                            {
                                Content = $"Донори для {i + 1} групи крові"
                            },
                            warriorsTable =
                            {
                                ItemsSource = donors[i]
                            }
                        };
                        windows[i].Show();
                    }
                }
            }
        }

        // виклик вікна з загальними відомостями для програми
        private void MiInstruction_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Звання в батальйоні:\nРекрут\nСолдат\nСержант\nЛейтенант\nКапітан\nМайор\nПідполковник\n"
                + "Групи крові: I - O, II - A, III - B, IV - AB\nРезус фактор: + або -\n",
                "Пояснення",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // виклик вікна, яке містить інформацію про автора
        private void MiAboutCreator_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Цю програму було створено студентом групи ПЗ-23 Борковичем Андрієм\nНУЛП\t2022 р.",
                "Про розробника",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // завантаження номеру поточного рядка
        private void Table_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }


        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!e.Handled)
            {
                MessageBox.Show("Warrior was deleted from list successfully","Congratulations!", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            }
        }
    }
}

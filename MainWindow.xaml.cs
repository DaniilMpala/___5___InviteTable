
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{

    public class Levels
    {
        public int Level { get; set; }
        public string Step { get; set; }
        public int SumPerexoda { get; set; }
    }
    public class Person
    {
        public int ID { get; set; }
        public string IDRefferala { get; set; }
        public string FIO { get; set; }
        public string Level { get; set; }
        public string Sleh { get; set; }
        public int CountRef { get; set; }
        public string Date { get; set; }

        //public ObservableCollection<Person> refferals { get; set; } = new ObservableCollection<Person>();
    }
    public partial class MainWindow : Window
    {

        public ObservableCollection<Person> People { get; set; }
        public ObservableCollection<Person> PeopleVsego { get; set; }
        public ObservableCollection<Levels> ListLevels { get; set; }

        public bool checkedsaveizm = false;
        readonly string path = Environment.GetEnvironmentVariable("APPDATA");
        public int idnext = 1;
        public int tmp;
        bool reload = false;
        string pathSaveBd = "-";
        Person temp;
        public MainWindow()
        {
            People = new ObservableCollection<Person>();
            PeopleVsego = new ObservableCollection<Person>();
            ListLevels = new ObservableCollection<Levels>() {
                new Levels() { Level = 1, Step = "", SumPerexoda = 500 },
                new Levels() { Level = 2, Step = "А", SumPerexoda = 1000  },
                new Levels() { Level = 2, Step = "Б", SumPerexoda = 1000  },
                new Levels() { Level = 3, Step = "А", SumPerexoda = 3000  },
                new Levels() { Level = 3, Step = "Б", SumPerexoda = 3000  },
                new Levels() { Level = 4, Step = "А", SumPerexoda = 9000  },
                new Levels() { Level = 4, Step = "Б", SumPerexoda = 9000  },
                new Levels() { Level = 5, Step = "А", SumPerexoda = 30000  },
                new Levels() { Level = 5, Step = "Б", SumPerexoda = 30000  },
                new Levels() { Level = 6, Step = "А", SumPerexoda = 90000  },
                new Levels() { Level = 6, Step = "Б", SumPerexoda = 90000  },
                new Levels() { Level = 7, Step = "А", SumPerexoda = 310000  },
                new Levels() { Level = 7, Step = "Б", SumPerexoda = 310000  },
                new Levels() { Level = 8, Step = "А", SumPerexoda = 320000  },
                new Levels() { Level = 8, Step = "Б", SumPerexoda = 550000  },
            };
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                dirInfo.CreateSubdirectory(@"InviteTable\");
                if (!File.Exists(path + @"\\InviteTable\\setting.txt"))
                {
                    File.WriteAllText(path + @"\\InviteTable\\setting.txt", "-|-");
                }
                string[] setting = File.ReadAllLines(path + @"\\InviteTable\\setting.txt", Encoding.UTF8);
                String[] stre = setting[0].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                checkedsaveizm = stre[1] == "1";
                pathSaveBd = stre[0];


                if (!File.Exists(path + @"\\InviteTable\\bdProgramm.txt"))
                {
                    File.WriteAllText(path + @"\\InviteTable\\bdProgramm.txt", "");
                }

                if (pathSaveBd.Trim() == "-")
                {
                    pathSaveBd = path + @"\\InviteTable\\bdProgramm.txt";

                    string[] bd = File.ReadAllLines(pathSaveBd, Encoding.UTF8);
                    foreach (string str in bd)
                    {
                        Person p = new Person();
                        String[] strMain = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        p.FIO = Regex.IsMatch(strMain[0], "_") ? strMain[0].Replace('_', ' ') : strMain[0];
                        p.ID = Convert.ToInt32(strMain[1]);
                        p.Sleh = "/";
                        p.IDRefferala = strMain[3];
                        p.CountRef = Convert.ToInt32(strMain[4]);
                        p.Level = strMain[5];
                        p.Date = strMain[6] + " " + strMain[7];

                        if (p.ID >= idnext)
                            idnext = p.ID + 1;


                        PeopleVsego.Add(p);
                    }
                    pathSaveBd = "-";
                }
                else
                {
                    string[] bd = File.ReadAllLines(pathSaveBd, Encoding.UTF8);
                    foreach (string str in bd)
                    {
                        Person p = new Person();
                        String[] strMain = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        p.FIO = Regex.IsMatch(strMain[0], "_") ? strMain[0].Replace('_', ' ') : strMain[0];
                        p.ID = Convert.ToInt32(strMain[1]);
                        p.Sleh = "/";
                        p.IDRefferala = strMain[3];
                        p.CountRef = Convert.ToInt32(strMain[4]);
                        p.Level = strMain[5];
                        p.Date = strMain[6] + " " + strMain[7];

                        if (p.ID >= idnext)
                            idnext = p.ID + 1;


                        PeopleVsego.Add(p);
                    }
                }

            }
            catch (Exception e)
            {
                File.WriteAllText(path + @"\\InviteTable\\error.txt", e.ToString());
                MessageBoxResult result = MessageBox.Show($"С файлом БазаДанных произошла ошибка, удалить? \n После удаление он создаться снова, но вся информация сотрётся (настройки в том числе), но приложение запуститься.\n Файл с ошибкой находится {path + @"\\InviteTable\\error.txt"}",
                                          "Подверждение",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    reload = true;
                    Directory.Delete(path + @"\\InviteTable", true);
                    System.Windows.Forms.Application.Restart();
                    System.Windows.Application.Current.Shutdown();
                }
                if (result == MessageBoxResult.No)
                {
                    Environment.Exit(0);
                }
            }

            InitializeComponent();

            listlevel.ItemsSource = ListLevels;
            listbox.ItemsSource = People;
            standartcheck.IsChecked = true;
            chekced.IsChecked = checkedsaveizm;

        }

        public void Selectitem(object sender, SelectionChangedEventArgs e)
        {
            if (listlevel.SelectedIndex > -1)
            {
                People.Clear();
                Levels lev = (Levels)listlevel.SelectedItem;

                foreach (var item in PeopleVsego)
                {
                    if (item.Level == $"{lev.Level}{lev.Step}")
                        People.Add(item);
                }
            }
            else
            {
                People.Clear();
            }
        }
        public void Changedselect(object sender, SelectionChangedEventArgs e)
        {

        }

        public void AddListItem(object sender, RoutedEventArgs e)
        {
            if (ima.Text.Trim() == "" || fam.Text.Trim() == "")
            {
                MessageBox.Show("Заполните поле ИМЯ и ФАМИЛИЯ");
            }
            else
            {
                AddusersD(0, true);
            }
        }

        public void SearchFio(object sender, RoutedEventArgs e)
        {
            People.Clear();
            if (listlevel.SelectedIndex > -1)
            {
                Levels lev = (Levels)listlevel.SelectedItem;
                foreach (Person item in PeopleVsego)
                {
                    if (item.Level == $"{lev.Level}{lev.Step}")
                        People.Add(item);
                }
            }


            if (searchfio.Text.Trim() != "")
            {
                if (Int32.TryParse(searchfio.Text.Trim(), out _))
                {
                    List<Person> numbers = new List<Person>();
                    foreach (Person item in People)
                    {
                        if (item.ID.ToString().IndexOf(searchfio.Text.Trim()) == -1)
                            numbers.Add(item);
                    }
                    foreach (Person item in numbers)
                    {
                        People.Remove(item);
                    }
                }
                else
                {
                    List<Person> numbers = new List<Person>();
                    foreach (Person item in People)
                    {
                        if (item.FIO.ToLower().IndexOf(searchfio.Text.Trim().ToLower()) == -1)
                            numbers.Add(item);
                    }
                    foreach (Person item in numbers)
                    {
                        People.Remove(item);
                    }
                }

            }
        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!reload)
                Saveinfail();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem button)
            {
                Person task = button.DataContext as Person;

                if (task.IDRefferala != "-" && task.CountRef != 0)
                {
                    Person pe = PeopleVsego.Where(x => x.ID == Convert.ToInt32(task.IDRefferala)).FirstOrDefault();
                    if(pe == null)
                    {
                        pe = task;
                    }
                    pe.CountRef -= 1;
                    int index = PeopleVsego.IndexOf(pe);
                    PeopleVsego.RemoveAt(index);
                    PeopleVsego.Insert(index, pe);

                    index = People.IndexOf(pe);
                    if (index > -1)
                    {
                        People.RemoveAt(index);
                        People.Insert(index, pe);
                    }

                }



                People.Remove(task);
                PeopleVsego.Remove(task);
                listbox.SelectedIndex = -1;

                if (checkedsaveizm)
                    Saveinfail();
            }
            else
            {
                return;
            }
        }
        private void Addrefuser(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Person p = button.DataContext as Person;

                tmp = p.ID;
                temp = p;
                can.Visibility = Visibility.Visible;
                gridingcanvas.Visibility = Visibility.Visible;
                addrefBut.Visibility = Visibility.Visible;

            }
            else
            {
                return;
            }
        }
        private void Addrefuserkuser(object sender, RoutedEventArgs e)
        {
            AddusersD(tmp, false);
        }
        void AddusersD(int usersGlava, bool newusers)
        {
            Person p = new Person()
            {
                FIO = $"{FirstUpper(ima.Text.Trim())} {FirstUpper(fam.Text.Trim())} {FirstUpper(oth.Text.Trim())}",
                Level = "1",
                ID = idnext,
                IDRefferala = newusers ? "-" : usersGlava.ToString(),
                Sleh = "/",
                CountRef = 0,
                Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };

            idnext++;

            PeopleVsego.Add(p);

            if (!newusers)
            {
                int counttemp4 = temp.CountRef / 4;
                temp.CountRef += 1;
                temp.Level = counttemp4 != temp.CountRef / 4 ? Perevod(temp.Level, true) : temp.Level ;
                //int index = People.IndexOf(temp);
                //People.RemoveAt(index);
                //People.Insert(index, temp);

                int index = PeopleVsego.IndexOf(temp);
                PeopleVsego.RemoveAt(index);
                PeopleVsego.Insert(index, temp);
            }

            chel4();


            listlevel.SelectedIndex = -1;
            listlevel.SelectedIndex = 0;
            listbox.SelectedIndex = People.IndexOf(p);

            listbox.ScrollIntoView(listbox.SelectedItem);

            can.Visibility = Visibility.Hidden;
            gridingcanvas.Visibility = Visibility.Hidden;
            fiobut.Visibility = Visibility.Hidden;
            addrefBut.Visibility = Visibility.Hidden;

            ima.Text = "";
            fam.Text = "";
            oth.Text = "";

            if (checkedsaveizm)
                Saveinfail();
        }



        private void Otkcanvasuser(object sender, RoutedEventArgs e)
        {
            can.Visibility = Visibility.Visible;
            gridingcanvas.Visibility = Visibility.Visible;
            fiobut.Visibility = Visibility.Visible;
        }
        private void Otkcanvasref(object sender, RoutedEventArgs e)
        {
            can.Visibility = Visibility.Visible;
            gridingcanvas.Visibility = Visibility.Visible;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            can.Visibility = Visibility.Hidden;
            gridingcanvas.Visibility = Visibility.Hidden;
            fiobut.Visibility = Visibility.Hidden;
            gridingcanvassetting.Visibility = Visibility.Hidden;
            addrefBut.Visibility = Visibility.Hidden;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            can.Visibility = Visibility.Visible;
            gridingcanvassetting.Visibility = Visibility.Visible;
        }

        private void Chekced_Checked(object sender, RoutedEventArgs e)
        {
            checkedsaveizm = (bool)chekced.IsChecked;
            string[] readText = File.ReadAllLines(path + @"\\InviteTable\\setting.txt", Encoding.UTF8);
            String[] stre = readText[0].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            stre[1] = "1";
            string one = string.Join("|", stre);
            File.WriteAllText(path + @"\\InviteTable\\setting.txt", one);
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            checkedsaveizm = (bool)chekced.IsChecked;
            string[] readText = File.ReadAllLines(path + @"\\InviteTable\\setting.txt", Encoding.UTF8);
            String[] stre = readText[0].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            stre[1] = "0";
            string one = string.Join("|", stre);
            File.WriteAllText(path + @"\\InviteTable\\setting.txt", one);
        }
        private void Standartcheck_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox box)
                box.IsChecked = !box.IsChecked;
        }

        public string FirstUpper(string str)
        {
            string[] s = str.Split(' ');

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Length > 1)
                    s[i] = s[i].Substring(0, 1).ToUpper() + s[i].Substring(1, s[i].Length - 1).ToLower();
                else s[i] = s[i].ToUpper();
            }
            return string.Join(" ", s);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        string Perevod(string level, bool upper)
        {
            string f = "";
            switch (level)
            {
                case "1":
                    f = upper ? "2А" : "";
                    break;
                case "2А":
                    f = upper ? "2Б" : "1";
                    break;
                case "2Б":
                    f = upper ? "3А" : "2А";
                    break;
                case "3А":
                    f = upper ? "3Б" : "2Б";
                    break;
                case "3Б":
                    f = upper ? "4А" : "3А";
                    break;
                case "4А":
                    f = upper ? "4Б" : "3Б";
                    break;
                case "4Б":
                    f = upper ? "5А" : "4А";
                    break;
                case "5А":
                    f = upper ? "5Б" : "4Б";
                    break;
                case "5Б":
                    f = upper ? "6А" : "5А";
                    break;
                case "6А":
                    f = upper ? "6Б" : "5Б";
                    break;
                case "6Б":
                    f = upper ? "7А" : "6А";
                    break;
                case "7А":
                    f = upper ? "7Б" : "6Б";
                    break;
                case "7Б":
                    f = upper ? "8А" : "7А";
                    break;
                case "8А":
                    f = upper ? "8Б" : "7Б";
                    break;
                case "8Б":
                    f = upper ? "" : "8А";
                    break;
            }
            return f;
        }

        private void Button_Click_36(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {

                Person task = button.DataContext as Person;
                string lev = Perevod(task.Level, true);
                if (lev.Trim() != "")
                    task.Level = lev;
                int ind = People.IndexOf(task);
                People.RemoveAt(ind);
                ind = PeopleVsego.IndexOf(task);
                PeopleVsego.RemoveAt(ind);
                PeopleVsego.Insert(ind, task);

                chel4();

                int tempe = listlevel.SelectedIndex;
                listlevel.SelectedIndex = -1;
                listlevel.SelectedIndex = tempe;

                if (checkedsaveizm)
                    Saveinfail();
            }
            else
            {
                return;
            }
        }

        public void chel4()
        {
            if (PeopleVsego.Where(x => x.Level == "1").Count() >= 5)
            {
                Person pe = PeopleVsego.Where(x => x.Level == "1").FirstOrDefault();
                int index = People.IndexOf(pe);
                if (index > -1)
                    People.Remove(pe);
                index = PeopleVsego.IndexOf(pe);
                pe.Level = Perevod(pe.Level, true);

                PeopleVsego.RemoveAt(index);
                PeopleVsego.Insert(index, pe);
            }

            if (PeopleVsego.Where(x => x.Level == "2А").Count() >= 5)
            {
                Person pe = PeopleVsego.Where(x => x.Level == "2А").FirstOrDefault();
                int index = People.IndexOf(pe);
                if (index > -1)
                    People.Remove(pe);
                index = PeopleVsego.IndexOf(pe);
                pe.Level = Perevod(pe.Level, true);

                PeopleVsego.RemoveAt(index);
                PeopleVsego.Insert(index, pe);
            }

            if (PeopleVsego.Where(x => x.Level == "2Б").Count() >= 5)
            {
                Person pe = PeopleVsego.Where(x => x.Level == "2Б").FirstOrDefault();
                int index = People.IndexOf(pe);
                if (index > -1)
                    People.Remove(pe);
                index = PeopleVsego.IndexOf(pe);
                pe.Level = Perevod(pe.Level, true);

                PeopleVsego.RemoveAt(index);
                PeopleVsego.Insert(index, pe);
            }
        }
        private void Button_Click_37(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Person task = button.DataContext as Person;
                string lev = Perevod(task.Level, false);
                if (lev.Trim() != "")
                    task.Level = lev;
                int ind = People.IndexOf(task);
                People.RemoveAt(ind);
                ind = PeopleVsego.IndexOf(task);
                PeopleVsego.RemoveAt(ind);
                PeopleVsego.Insert(ind, task);

                chel4();

                int tempe = listlevel.SelectedIndex;
                listlevel.SelectedIndex = -1;
                listlevel.SelectedIndex = tempe;

                if (checkedsaveizm)
                    Saveinfail();
            }
            else
            {
                return;
            }
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (pathSaveBd.Trim() != "-")
            {
                string[] readText = pathSaveBd.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                readText[readText.Length - 1] = "";
                Process.Start(string.Join("\\", readText));
            }
            else
                Process.Start(path + @"\\InviteTable");
        }


        private void SavePatch_click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text documents (*.txt)|*.txt",
                FilterIndex = 2
            };

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                pathSaveBd = filename;

                string[] readText = File.ReadAllLines(path + @"\\InviteTable\\setting.txt", Encoding.UTF8);
                String[] stre = readText[0].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                stre[0] = pathSaveBd;
                string one = string.Join("|", stre);
                File.WriteAllText(path + @"\\InviteTable\\setting.txt", one);
                MessageBox.Show($"Пусть {pathSaveBd} успешно добавлен");

                People.Clear();
                PeopleVsego.Clear();
                listbox.SelectedIndex = -1;
                listlevel.SelectedIndex = -1;
                try
                {
                    string[] bd = File.ReadAllLines(pathSaveBd, Encoding.UTF8);
                    foreach (string str in bd)
                    {
                        Person p = new Person();
                        String[] strMain = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        p.FIO = Regex.IsMatch(strMain[0], "_") ? strMain[0].Replace('_', ' ') : strMain[0];
                        p.ID = Convert.ToInt32(strMain[1]);
                        p.Sleh = "/";
                        p.IDRefferala = strMain[3];
                        p.CountRef = Convert.ToInt32(strMain[4]);
                        p.Level = strMain[5];
                        p.Date = strMain[6] + " " + strMain[7];

                        PeopleVsego.Add(p);
                    }
                }
                catch (Exception g)
                {
                    File.WriteAllText(path + @"\\InviteTable\\error.txt", g.ToString());
                    MessageBoxResult resulte = MessageBox.Show($"Вы загрузили поврежденный файл БазыДанных! \n Если вы нажмете ДА, то БазаДанных будет браться изпервоночального положения(можете взайти в нее после перезапуска приложения по кнопке ОТКРЫТЬ ПАПКУ\n Если нажмете НЕТ, то вы можете сменить файл и открыть приложение снова",
                                              "Подверждение",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Question);
                    if (resulte == MessageBoxResult.Yes)
                    {
                        if (!File.Exists(path + @"\\InviteTable\\setting.txt"))
                        {
                            File.WriteAllText(path + @"\\InviteTable\\setting.txt", "-|-");
                        }
                        string[] setting = File.ReadAllLines(path + @"\\InviteTable\\setting.txt", Encoding.UTF8);
                        String[] streg = setting[0].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                        streg[1] = streg[1];
                        streg[0] = path + @"\\InviteTable";

                        reload = true;
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                    }
                    if (resulte == MessageBoxResult.No)
                    {
                        reload = true;
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                    }
                }



            }
        }


        public void Saveinfail()
        {
            Title = $"{ Title} - Сохранение...";

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo.CreateSubdirectory(@"InviteTable\");


            string str = "";
            foreach (Person item in PeopleVsego)
            {
                str += $"{item.FIO.Trim().Replace(" ", "_")} {item.ID} / {(item.IDRefferala == "" ? "-" : item.IDRefferala)} {item.CountRef} {item.Level} {item.Date}\n";
            }
            File.WriteAllText(path + @"\\InviteTable\\bdProgramm.txt", str.Trim());
            if (pathSaveBd.Trim() != "-")
            {
                if (pathSaveBd.IndexOf(".txt") == -1)
                {
                    if (!File.Exists(pathSaveBd + @"\\bdProgramm.txt"))
                    {
                        File.WriteAllText((pathSaveBd + @"\\bdProgramm.txt"), "");
                    }
                }

                File.WriteAllText(pathSaveBd, str.Trim());
            }


            Title = "InviteTable";
        }

        private void SavePatchFolder(object sender, RoutedEventArgs e)
        {
            using (var fldrDlg = new System.Windows.Forms.FolderBrowserDialog())
            {

                if (fldrDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    pathSaveBd = fldrDlg.SelectedPath + @"\\bdProgramm.txt";

                    string[] readText = File.ReadAllLines(path + @"\\InviteTable\\setting.txt", Encoding.UTF8);
                    String[] stre = readText[0].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    stre[0] = pathSaveBd;
                    string one = string.Join("|", stre);
                    File.WriteAllText(path + @"\\InviteTable\\setting.txt", one);
                    MessageBox.Show($"Пусть {pathSaveBd} успешно добавлен");


                }
            }
        }
    }
}

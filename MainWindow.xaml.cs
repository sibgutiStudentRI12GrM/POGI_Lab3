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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;
using System.Windows.Forms;

namespace POGI_Lab3
{
    

    public partial class MainWindow : Window {
        




        public List<string> TimeFormats = new List< string>() {
            {"D:H:M:S"},
            {"H:M:S"},
            {"M:S"},
            {"S"},
        };

        public Dictionary<string, DateTime> Timers = new Dictionary<string, DateTime>();

        public static System.Windows.Threading.DispatcherTimer TimerForUpdateRestTime;

        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
        public MainWindow(){
            InitializeComponent();
            ni.Icon = new System.Drawing.Icon("..\\..\\timer.ico");
            ni.Visible = true;
            ni.DoubleClick += delegate (object sender, EventArgs args) {
                this.Show();
                this.WindowState = WindowState.Normal;
                
            };


            foreach (var timeFormat in TimeFormats) {
                TimeFormatComboBox.Items.Add(timeFormat);
            }

            TimeFormatComboBox.SelectedIndex = 0;
            




        }

        protected override void OnStateChanged(EventArgs e) {

            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);
        }




        public void ShowTimerTimeLeft(DateTime timerDate) {
            

            DateTime currentTime = DateTime.Now;

            TimeSpan deltaTime = timerDate - currentTime;

            // getting current time format 
            int currentTimeFormatIndex = TimeFormatComboBox.SelectedIndex;
            
            string currentFormat = TimeFormats[currentTimeFormatIndex];

            int days = deltaTime.Days;
            int hours = deltaTime.Hours;
            int minutes = deltaTime.Minutes;
            int seconds = deltaTime.Seconds;

            LeftTimeLabel.Content = "Time left: ";

            if (currentFormat.Contains("D")) {
                
            } else if (currentFormat.Contains("H")) {
                hours = days*24 + hours;
            } else if (currentFormat.Contains("M")) {
                minutes = (days * 24 + hours) * 60 + minutes;
            } else if (currentFormat.Contains("S")) {
                seconds = ((days * 24 + hours) * 60 + minutes) * 60 + seconds;
            }

            string timeToShow = currentFormat
                    .Replace("D", Convert.ToString(days) + "D")
                    .Replace("H", Convert.ToString(hours) + "H")
                    .Replace("M", Convert.ToString(minutes) + "M")
                    .Replace("S", Convert.ToString(seconds) + "S");
            LeftTimeLabel.Content += timeToShow;
            


        }

        public void ShowTimerTitle(string title) {
            TimerTitleLable.Content = "Timer Title: " + title;
        }

        private void TimersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TimersComboBox.SelectedIndex != -1) {
                TimerTitleLable.Content = "Timer Title: " + TimersComboBox.SelectedItem;
                TimerForUpdateRestTime = new System.Windows.Threading.DispatcherTimer();
                TimerForUpdateRestTime.Interval = new TimeSpan(0, 0, 0, 1);
                TimerForUpdateRestTime.Start();
                TimerForUpdateRestTime.Tick += new EventHandler(TimerForUpdateRestTime_Tick);

                string currentTimerTitle = TimersComboBox.SelectedItem.ToString();
                ShowTimerTimeLeft(Timers[currentTimerTitle]);
            } else {
                TimerForUpdateRestTime.Stop();
            }
            

        }

        private void TimerForUpdateRestTime_Tick(object sender, EventArgs e) {
            if (TimersComboBox.SelectedIndex != -1) {
                string currentTimerTitle = TimersComboBox.SelectedItem.ToString();

                ShowTimerTimeLeft(Timers[currentTimerTitle]);
            }


        }

        private void Btn_AddTimer_Click(object sender, RoutedEventArgs e) {
            AddTimerWindow addTimerWindow = new AddTimerWindow();
            if (addTimerWindow.ShowDialog() == true) {
                string SelectedTimerTitle = addTimerWindow.SelectedTimerTitle;
                DateTime SelectedTimerDate = addTimerWindow.SelectedTimerDate;
                Timers.Add(SelectedTimerTitle, SelectedTimerDate);
                TimersComboBox.Items.Add(SelectedTimerTitle);
                TimersComboBox.SelectedItem = SelectedTimerTitle;
            } else {

            }
        }

        private void Btn_RemoveTimer_Click(object sender, RoutedEventArgs e) {
            if (TimersComboBox.SelectedIndex != -1) {
                TimerForUpdateRestTime.Stop();
                string currentTimerTitle = TimersComboBox.SelectedItem.ToString();
                TimerTitleLable.Content = "Timer Title: ";
                LeftTimeLabel.Content = "Time left: ";
                TimersComboBox.Items.Remove(currentTimerTitle);
                Timers.Remove(currentTimerTitle);
                if (TimersComboBox.Items.Count > 0) {
                    TimersComboBox.SelectedIndex = 0;
                    string newTitle = TimersComboBox.SelectedItem.ToString();
                    ShowTimerTitle(newTitle);
                }
                
                
            }
        }

        private void menuItemSave_Click(object sender, RoutedEventArgs e) {
            
            // MessageBox.Show(timersJson);


            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = "Timers";
            sfd.DefaultExt = ".json";
            sfd.Filter = "JSON data (.json)|*json";

            Nullable<bool> result = sfd.ShowDialog();
            if (result == true) {
                string timersJson = JsonSerializer.Serialize(Timers);
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8)) {
                    sw.WriteLine(timersJson);
                }
            }
        }

        private void menuItemOpen_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            
            ofd.DefaultExt = ".json";
            ofd.Filter = "JSON data (.json)|*json";
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true) {
                string line;
                StreamReader sr = new StreamReader(ofd.FileName);
                string jsonData = sr.ReadLine();

                
                Timers = JsonSerializer.Deserialize<Dictionary<string, DateTime>>(jsonData);
                TimersComboBox.SelectedIndex = -1;
                TimersComboBox.Items.Clear();
                foreach (KeyValuePair<string, DateTime> timer in Timers) {
                    TimersComboBox.Items.Add(timer.Key);
                }

                if (TimersComboBox.Items.Count != 0) {
                    TimersComboBox.SelectedIndex = 0;
                }

            }
        }
    }
}

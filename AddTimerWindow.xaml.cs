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
using System.Windows.Shapes;

namespace POGI_Lab3 {
    /// <summary>
    /// Interaction logic for AddTimerWindow.xaml
    /// </summary>
    public partial class AddTimerWindow : Window {

        public DateTime SelectedTimerDate;
        public string SelectedTimerTitle;
        public AddTimerWindow() {
            InitializeComponent();
            CalendarDatePicker.DisplayDateStart = DateTime.Now;
            CalendarDatePicker.SelectedDate = DateTime.Now;
            CalendarDatePicker.IsTodayHighlighted = true;

        }

        private void Btn_Confirm_Click(object sender, RoutedEventArgs e) {

            SelectedTimerTitle = TitleTextBox.Text;
            SelectedTimerDate = Convert.ToDateTime(CalendarDatePicker.SelectedDate.Value.Date)
                .AddHours(Int32.Parse(HoursTextBox.Text))
                .AddMinutes(Int32.Parse(MinutesTextBox.Text))
                .AddSeconds(Int32.Parse(SecondsTextBox.Text));
            
            this.DialogResult = true;
            


        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }
    }
}

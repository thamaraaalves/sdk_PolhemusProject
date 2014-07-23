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
using Prj_NotificationOficial.Interfaces.Model1;
using Prj_NotificationOficial.Interfaces.Model2;
using Prj_NotificationOficial.Interfaces.Model3;
using Prj_NotificationOficial.Interfaces.Model4;
using System.Xaml;

namespace Prj_NotificationOficial
{
    /// <summary>
    /// Interaction logic for menu.xaml
    /// </summary>
    public partial class menu : Window
    {
        public menu()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnNot1_Click(object sender, RoutedEventArgs e)
        {
            //show the notification one
           // this.AddChild();

            m1 M1 = new m1();
            this.Content = M1;

           
        }
    }
}

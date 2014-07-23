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

namespace Prj_NotificationOficial.Interfaces.Model1
{
    /// <summary>
    /// Interaction logic for m1.xaml
    /// </summary>
    public partial class m1 : UserControl
    {
        public m1()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Canvas).Children.Remove(this);
        }
    }

   
}

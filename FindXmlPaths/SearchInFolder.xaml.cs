using System.Windows;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace FindXmlPathsWpf
{
    /// <summary>
    /// Logique d'interaction pour SearchInFolder.xaml
    /// </summary>
    public partial class SearchInFolder : Window
    {
        public SearchInFolder()
        {
            InitializeComponent();
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Get wrecked !!",
                    "Error: Owned !", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}

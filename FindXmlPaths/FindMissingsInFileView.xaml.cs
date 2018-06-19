using System;
using System.Windows;
using FindXmlPathsServices.Services;
using Common.Services;
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
    public partial class FindMissingsInFileView : Window
    {
        // TODO BEGIN - Refactor - should be an Interface here
        FindMissingsInFileService processingService;
        // TODO END - Refactor
        XmlPathsFileService xmlPathsFileService;
        XmlFileService xmlFileService;

        public FindMissingsInFileView()
        {
            InitializeComponent();
        }

        XmlPathsFileService GetXmlPathsFileServiceInstance()
        {
            if (xmlPathsFileService == null)
                xmlPathsFileService = new XmlPathsFileService();

            return xmlPathsFileService;
        }

        XmlFileService GetXmlFileServiceInstance()
        {
            if (xmlFileService == null)
                xmlFileService = new XmlFileService();

            return xmlFileService;
        }

        FindMissingsInFileService GetModelInstance()
        {
            if (processingService != null)
                return processingService;

            return processingService = new FindMissingsInFileService(GetXmlPathsFileServiceInstance(), GetXmlFileServiceInstance());
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            if (!GetModelInstance().AllInputsAreFilled(txtBxXmlPathsFileName.Text, txtBxXmlFileName.Text, txtBxOutputFolder.Text))
            {
                MessageBox.Show(GetModelInstance().ErrorMessage,
                    "Uncomplete input provided", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!GetModelInstance().AllInputsAreValid(txtBxXmlPathsFileName.Text, txtBxXmlFileName.Text, txtBxOutputFolder.Text))
            {
                MessageBox.Show(GetModelInstance().ErrorMessage,
                    "Invalid input(s) provided", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var missingsInFile = processingService.GetListOfMissingsInFile(txtBxXmlPathsFileName.Text, txtBxXmlFileName.Text);

            try
            {
                btnProcess.IsEnabled = false;
                btnProcess.Content = "Processing...";
                var outputFileCreated =
                    GetXmlFileServiceInstance()
                    .CreateOutputMissingInSourceFile(missingsInFile, txtBxOutputFolder.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured while trying to generate the result file.\n{ex.Message}",
                    "Output creation failed !", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            btnProcess.IsEnabled = true;
            btnProcess.Content = "Process";
        }
    }
}

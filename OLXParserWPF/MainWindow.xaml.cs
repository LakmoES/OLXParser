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
using GalaSoft.MvvmLight.Messaging;
using OLXParserWPF.Messages;
using OLXParserWPF.ViewModel;

namespace OLXParserWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var modelView = new MainWindowViewModel();
            this.DataContext = modelView;
            modelView.AlarmToUI += (o, s) => { MessageBox.Show(this, s, "Внимание", MessageBoxButton.OK); };
            modelView.QuestionYesNo += (o, s) => { SendMessage(s); };
        }

        public void SendMessage(string title)
        {
            var yesNoMessage = new YesNoMessage();
            {
                var result = MessageBox.Show(this, title, "Подтверждение", MessageBoxButton.YesNo);
                yesNoMessage.Message = result == MessageBoxResult.Yes;
            };
            Messenger.Default.Send(yesNoMessage);
        }
    }
}

using System.Windows;

namespace TestWpfApp
{
    public partial class RestApiTestMainWindow
        : Window
    {
        public RestApiTestMainWindow()
        {
            InitializeComponent();
            DataContext = new RestApiTestViewModel(new XmlReader());
        }
    }
}

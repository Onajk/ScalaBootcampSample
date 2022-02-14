using System.Windows;
using TrainingApp.MVVM.ViewModel;

namespace TrainingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Bind MainWindow with MainPageViewModel
            DataContext = new MainPageViewModel();
        }
    }
}

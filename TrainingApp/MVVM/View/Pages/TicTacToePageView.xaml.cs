using System.Windows.Controls;
using TrainingApp.MVVM.ViewModel;

namespace TrainingApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for TicTacToeView.xaml
    /// </summary>
    public partial class TicTacToePageView : UserControl
    {
        public TicTacToePageView()
        {
            InitializeComponent();

            DataContext = new TicTacToePageViewModel();
        }
    }
}

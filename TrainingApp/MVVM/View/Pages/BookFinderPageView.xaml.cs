using System.Windows.Controls;
using TrainingApp.MVVM.ViewModel;

namespace TrainingApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for BookFinderPageView.xaml
    /// </summary>
    public partial class BookFinderPageView : UserControl
    {
        public BookFinderPageView()
        {
            InitializeComponent();
            DataContext = new BookFinderPageViewModel();
        }
    }
}

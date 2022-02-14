using System.Windows.Controls;
using TrainingApp.MVVM.ViewModel;

namespace TrainingApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for NotesView.xaml
    /// </summary>
    public partial class NotesPageView : UserControl
    {
        public NotesPageView()
        {
            InitializeComponent();
            
            DataContext = new NotesPageViewModel();
        }
    }
}

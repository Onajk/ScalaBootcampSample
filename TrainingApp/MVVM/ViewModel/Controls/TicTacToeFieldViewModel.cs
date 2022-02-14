using System.Windows.Media;
using TrainingApp.Core;

namespace TrainingApp.MVVM.ViewModel
{
    // View model of a Tic Tac Toe game field
    class TicTacToeFieldViewModel : ObservableObject
    {
        private string _content;
        private SolidColorBrush _background;

        public int GridRow { get; set; }

        public int GridColumn { get; set; }

        public uint Value { get; set; }

        public RelayCommand FieldCommand { get; set; }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged();
            }
        }
    }
}
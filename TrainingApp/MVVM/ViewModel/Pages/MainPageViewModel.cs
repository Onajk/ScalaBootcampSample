using TrainingApp.Core;

namespace TrainingApp.MVVM.ViewModel
{
    // View model of displayed page. It handles page changing.
    class MainPageViewModel : ObservableObject
    {
        private object _currentView;

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand TicTacToeViewCommand { get; set; }

        public RelayCommand NotesViewCommand { get; set; }

        public RelayCommand BookFinderViewCommand { get; set; }

        public RelayCommand AppCloseCommand { get; set; }

        public HomePageViewModel HomeVm { get; set; }

        public TicTacToePageViewModel TicTacToeVm { get; set; }

        public NotesPageViewModel NotesVm { get; set; }

        public BookFinderPageViewModel BookFinderVm { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            HomeVm = new HomePageViewModel();
            TicTacToeVm = new TicTacToePageViewModel();
            NotesVm = new NotesPageViewModel();
            BookFinderVm = new BookFinderPageViewModel();
            CurrentView = HomeVm;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVm;
            });

            TicTacToeViewCommand = new RelayCommand(o =>
            {
                CurrentView = TicTacToeVm;
            });

            NotesViewCommand = new RelayCommand(o =>
            {
                CurrentView = NotesVm;
            });

            BookFinderViewCommand = new RelayCommand(o =>
            {
                CurrentView = BookFinderVm;
            });

            AppCloseCommand = new RelayCommand(o =>
            {
                System.Windows.Application.Current.Shutdown();
            });
        }
    }
}

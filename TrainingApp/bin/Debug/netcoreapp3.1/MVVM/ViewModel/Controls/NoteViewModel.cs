using TrainingApp.Core;

namespace TrainingApp.MVVM.ViewModel
{
    // View model of a new note
    class NoteViewModel : ObservableObject
    {
        private string _timestamp;
        private bool _cantInput = true;
        private bool _canClickSaveButton = false;
        private bool _canClickEditButton = true;

        public uint Id { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public RelayCommand RemoveNoteCommand { get; set; }

        public RelayCommand EditNoteCommand { get; set; }

        public RelayCommand SaveNoteCommand { get; set; }

        public string Timestamp
        {
            get { return _timestamp; }
            set
            {
                _timestamp = value;
                OnPropertyChanged();
            }
        }

        public bool CantInput
        {
            get { return _cantInput; }
            set
            {
                _cantInput = value;
                OnPropertyChanged();
            }
        }
        public bool CanClickSaveButton
        {
            get { return _canClickSaveButton; }
            set
            {
                _canClickSaveButton = value;
                OnPropertyChanged();
            }
        }
        public bool CanClickEditButton
        {
            get { return _canClickEditButton; }
            set
            {
                _canClickEditButton = value;
                OnPropertyChanged();
            }
        }
    }
}

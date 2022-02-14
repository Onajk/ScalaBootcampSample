using SQLite; // Database
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrainingApp.Core;
using TrainingApp.MVVM.Model;

namespace TrainingApp.MVVM.ViewModel
{
    // View model of notes page
    class NotesPageViewModel : ObservableObject
    {
        private string _newNoteContent;
        private string _newNoteTitle;

        public string DatabasePath { get; } = System.IO.Path.Combine(Environment.CurrentDirectory, "Notes.db");

        public uint NewNoteId { get; set; } = 0;

        public string NewNoteTimestamp { get; set; }

        public ObservableCollection<NoteViewModel> NoteList { get; set; } = new ObservableCollection<NoteViewModel>();

        public List<NoteSQLiteModel> NoteSQLiteList { get; set; } = new List<NoteSQLiteModel>();

        public RelayCommand NewNoteCommand { get; set; }

        public string NewNoteContent
        {
            get { return _newNoteContent; }
            set
            {
                _newNoteContent = value;
                OnPropertyChanged();
            }
        }

        public string NewNoteTitle
        {
            get { return _newNoteTitle; }
            set
            {
                _newNoteTitle = value;
                OnPropertyChanged();
            }
        }
        // add new note to note list and display it on the screen
        private void AddNewNote(uint noteId)
        {
            var newNote = new NoteViewModel
            {
                Content = NewNoteContent,
                Timestamp = NewNoteTimestamp,
                Title = NewNoteTitle,
                // '-' button clicked
                RemoveNoteCommand = new RelayCommand(o =>
                {
                    NoteList.Remove(o as NoteViewModel);

                    using (SQLiteConnection connection = new SQLiteConnection(DatabasePath))
                    {
                        connection.Query<NoteSQLiteModel>("DELETE FROM NoteSQLiteModel WHERE Id='"+ noteId + "'");
                    }
                }),
                // button with pencil cliecked
                EditNoteCommand = new RelayCommand(o =>
                {
                    (o as NoteViewModel).CantInput = false;
                    (o as NoteViewModel).CanClickEditButton = false;
                    (o as NoteViewModel).CanClickSaveButton = true;
                }),
                // button with floppy disk clicked
                SaveNoteCommand = new RelayCommand(o =>
                {
                    (o as NoteViewModel).CantInput = true;
                    (o as NoteViewModel).CanClickEditButton = true;
                    (o as NoteViewModel).CanClickSaveButton = false;

                    using (SQLiteConnection connection = new SQLiteConnection(DatabasePath))
                    {
                        var editedNote = connection.Query<NoteSQLiteModel>("SELECT * FROM NoteSQLiteModel WHERE Id='" + noteId + "'");
                        (o as NoteViewModel).Timestamp = DateTime.Now.ToString("dddd, dd.MM.yyyy, HH:mm:ss");
                        editedNote[0].Content = (o as NoteViewModel).Content;
                        editedNote[0].Title = (o as NoteViewModel).Title;
                        editedNote[0].Timestamp = (o as NoteViewModel).Timestamp;
                        connection.Update(editedNote[0]);
                    }
                })
            };
            NoteList.Add(newNote);
        }
        // add new note to SQLite database
        private void AddNoteToSQLite()
        {
            var newSQLiteNote = new NoteSQLiteModel
            {
                Id = NewNoteId++,
                Content = NewNoteContent,
                Timestamp = NewNoteTimestamp,
                Title = NewNoteTitle
            };
            NoteSQLiteList.Add(newSQLiteNote);

            using (SQLiteConnection connection = new SQLiteConnection(DatabasePath))
            {
                connection.CreateTable<NoteSQLiteModel>();
                connection.Insert(newSQLiteNote);
            }

            NewNoteContent = string.Empty;
            NewNoteTitle = string.Empty;
        }
        // Database read method. Add old notes to note list.
        private void ReadDataBase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabasePath))
            {
                connection.CreateTable<NoteSQLiteModel>();
                NoteSQLiteList = connection.Table<NoteSQLiteModel>().ToList();
            }
            // for all notes from database - if note is not on the current note list then add it
            foreach (var itemFromSQLite in NoteSQLiteList)
            {
                bool duplicationFlag = false;
                foreach (var itemFromNoteList in NoteList)
                {
                    if (itemFromSQLite.Id == itemFromNoteList.Id)
                    {
                        duplicationFlag = true;
                        break;
                    }
                }

                if (!duplicationFlag)
                {
                    if (itemFromSQLite.Id >= NewNoteId) NewNoteId = itemFromSQLite.Id + 1;
                    NewNoteContent = itemFromSQLite.Content;
                    NewNoteTitle = itemFromSQLite.Title;
                    NewNoteTimestamp = itemFromSQLite.Timestamp;
                    AddNewNote(itemFromSQLite.Id);
                }
            }

            NewNoteContent = string.Empty;
            NewNoteTitle = string.Empty;
        }

        public NotesPageViewModel()
        {
            // look for already stored notes in database at start
            ReadDataBase();
            // '+' button clicked
            NewNoteCommand = new RelayCommand(o =>
            {
                NewNoteTimestamp = DateTime.Now.ToString("dddd, dd.MM.yyyy, HH:mm:ss");
                AddNewNote(NewNoteId);
                AddNoteToSQLite();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using TrainingApp.Core;
using TrainingApp.MVVM.Model;
using Google.Apis.Books.v1;
using Google.Apis.Services;
using System.Linq;

namespace TrainingApp.MVVM.ViewModel
{
    // View model of book finder page.
    class BookFinderPageViewModel : ObservableObject
    {
        private string _searchingPhrase;
        private string _booksList = string.Empty;

        public BooksService BooksService { get; set; }

        public RelayCommand SearchCommand { get; set; }

        public string SearchingPhrase
        {
            get { return _searchingPhrase; }
            set
            {
                _searchingPhrase = value;
                OnPropertyChanged();
            }
        }

        public string BooksList
        {
            get { return _booksList; }
            set
            {
                _booksList = value;
                OnPropertyChanged();
            }
        }
        // Gooogle books query method
        public List<BookModel> Search(string searchingPhrase, int offset, int count)
        {
            var query = BooksService.Volumes.List(searchingPhrase);
            query.MaxResults = count;
            query.StartIndex = offset;
            try
            {
                var res = query.Execute();
                if (res.Items == null)
                {
                    BooksList = "Didn't find any book :(\n";
                    return new List<BookModel>();
                }
                var books = res.Items.Select(book => new BookModel
                {
                    Id = book.Id,
                    Title = book.VolumeInfo.Title,
                    Subtitle = book.VolumeInfo.Subtitle,
                    Description = book.VolumeInfo.Description,
                    PageCount = book.VolumeInfo.PageCount,
                }).ToList();
                return new List<BookModel>(books);
            }
            catch (Exception)
            {
                BooksList = "API key not valid. Please pass a valid API key.";
                return new List<BookModel>();
            }
        }

        public BookFinderPageViewModel()
        {
            string apiKey = "AIzaSyAs9eBzmpLe9ifeDHefWXjoaXr3qufaCPc";
            // Connection to google books service with apiKey
            BooksService = new BooksService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = this.GetType().ToString()
            });
            // Search button clicked
            SearchCommand = new RelayCommand(o =>
            {
                BooksList = string.Empty;
                List<BookModel> foundBooks = Search(SearchingPhrase, 0, 20);

                int counter = 1;
                foreach (BookModel book in foundBooks)
                {
                    // Append output textbox with found books
                    BooksList += counter++ + ". " + book.Title + ", " + book.Subtitle + ", " + book.PageCount + "\n";
                }
            });
        }
    }
}

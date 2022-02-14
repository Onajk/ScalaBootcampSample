using System.Collections;

namespace TrainingApp.MVVM.Model
{
    // Model of the book for book searching
    class BookModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public IList Authors { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }

        public int? PageCount { get; set; }
    }
}

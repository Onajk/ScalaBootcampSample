using SQLite;

namespace TrainingApp.MVVM.Model
{
    // Model of the note for SQLite database
    class NoteSQLiteModel
    {
        [PrimaryKey]
        public uint Id { get; set; }

        public string Content { get; set; }

        public string Timestamp { get; set; }

        public string Title { get; set; }
    }
}

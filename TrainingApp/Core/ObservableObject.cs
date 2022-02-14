using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TrainingApp.Core
{
    // This interface defines the contract for behaviour when the value of a property changes. It raises PropertyChanged event when the value changes.
    class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

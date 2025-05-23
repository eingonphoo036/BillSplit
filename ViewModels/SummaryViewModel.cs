using BillSplit.Models;
using BillSplit.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BillSplit.ViewModels
{
    public class SummaryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly FirebaseDbService _firebaseDbService = new FirebaseDbService();
        public ObservableCollection<Event> Events { get; } = new ObservableCollection<Event>();

        public ICommand LoadEventsCommand { get; }
        public ICommand DeleteEventCommand { get; }
        public ICommand DeleteEventByNameCommand { get; }
        public ICommand NavigateHomeCommand { get; }

        private string _eventNameToDelete;
        public string EventNameToDelete
        {
            get => _eventNameToDelete;
            set
            {
                _eventNameToDelete = value;
                OnPropertyChanged(nameof(EventNameToDelete));
            }
        }

        public SummaryViewModel()
        {
            LoadEventsCommand = new Command(async () => await LoadEvents());
            DeleteEventCommand = new Command<Event>(async (e) => await DeleteEvent(e));
            DeleteEventByNameCommand = new Command(async () => await DeleteEventByName());
            NavigateHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));
        }

        public async Task InitializeAsync()
        {
            await LoadEvents();
        }

        private async Task LoadEvents()
        {
            try
            {
                var events = await _firebaseDbService.GetEventsWithParticipants();
                Events.Clear();
                foreach (var eventItem in events)
                {
                    Events.Add(eventItem);
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading events: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to load events. Please try again.", "OK");
            }
        }

        private async Task DeleteEvent(Event eventItem)
        {
            try
            {
                await _firebaseDbService.DeleteEvent(eventItem.Id);
                Events.Remove(eventItem);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting event: {ex.Message}");
            }
        }

        private async Task DeleteEventByName()
        {
            if (string.IsNullOrWhiteSpace(EventNameToDelete))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter the event name to delete.", "OK");
                return;
            }

            try
            {
                var eventToDelete = Events.FirstOrDefault(e => e.Name.Equals(EventNameToDelete, StringComparison.OrdinalIgnoreCase));

                if (eventToDelete != null)
                {
                    await _firebaseDbService.DeleteEventByName(EventNameToDelete);
                    EventNameToDelete = string.Empty;
                    await LoadEvents(); // Reload events to reflect changes
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Event not found.", "OK");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting event by name: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "An error occurred while deleting the event.", "OK");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using BillSplit.Services;
using BillSplit.Models;
using System;
using System.Linq;
using System.Net.Mail;

namespace BillSplit.ViewModels
{
    public class EventViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseDbService _firebaseDbService = new FirebaseDbService();

        private string _eventName;
        private string _totalCostText;
        private int _participantCount;
        private string _participantsText;
        private string _payerEmail;

        public ObservableCollection<string> Participants { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public string EventName
        {
            get => _eventName;
            set { _eventName = value; OnPropertyChanged(); }
        }

        public string TotalCostText
        {
            get => _totalCostText;
            set { _totalCostText = value; OnPropertyChanged(); }
        }

        public int ParticipantCount
        {
            get => _participantCount;
            set { _participantCount = value; OnPropertyChanged(); }
        }

        public string ParticipantsText
        {
            get => _participantsText;
            set
            {
                _participantsText = value;
                OnPropertyChanged();

                Participants.Clear();
                foreach (var email in value.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    Participants.Add(email.Trim());
                }
            }
        }

        public string PayerEmail
        {
            get => _payerEmail;
            set { _payerEmail = value; OnPropertyChanged(); }
        }

        public ICommand SaveEventCommand { get; }
        public ICommand NavigateHomeCommand { get; }

        public EventViewModel()
        {
            SaveEventCommand = new Command(async () => await OnSaveEvent());
            NavigateHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async Task OnSaveEvent()
        {
            if (string.IsNullOrWhiteSpace(EventName) || string.IsNullOrWhiteSpace(TotalCostText) ||
                ParticipantCount <= 0 || string.IsNullOrWhiteSpace(ParticipantsText) || string.IsNullOrWhiteSpace(PayerEmail))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill all fields correctly.", "OK");
                return;
            }

            if (!decimal.TryParse(TotalCostText, out decimal totalCost))
            {
                await Shell.Current.DisplayAlert("Error", "Total cost must be a number.", "OK");
                return;
            }

            // Null check for PayerEmail and ParticipantsText
            if (string.IsNullOrWhiteSpace(PayerEmail))
            {
                await Shell.Current.DisplayAlert("Error", "Payer email cannot be empty.", "OK");
                return;
            }

            // Validate emails
            var emails = ParticipantsText.Split(',').Select(e => e.Trim()).ToList();
            foreach (var email in emails)
            {
                if (!IsValidEmail(email))
                {
                    await Shell.Current.DisplayAlert("Error", $"Invalid email address: {email}", "OK");
                    return;
                }
            }

            try
            {
                var eventId = await _firebaseDbService.AddEvent(new Event
                {
                    Name = EventName ?? "Untitled Event",
                    TotalCost = totalCost,
                    Date = DateTime.Now,
                    CreatorEmail = AppSession.Email ?? "unknown"
                });

                decimal amountPerPerson = totalCost / ParticipantCount;

                foreach (var email in emails)
                {
                    await _firebaseDbService.AddParticipant(eventId, new Participant
                    {
                        Email = email ?? "unknown@domain.com",
                        AmountDue = amountPerPerson,
                        HasPaid = email == PayerEmail
                    });
                }

                await Shell.Current.DisplayAlert("Event Created", $"Event '{EventName}' created successfully!", "OK");
                await Shell.Current.GoToAsync("//HomePage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred while saving the event: {ex.Message}", "OK");
            }
        }


        private void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

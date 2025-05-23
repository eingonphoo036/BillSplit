using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using BillSplit.Models;
using System.Collections.ObjectModel;

namespace BillSplit.Services
{
    public class FirebaseDbService
    {
        private readonly FirebaseClient _client;

        public FirebaseDbService()
        {
            _client = new FirebaseClient("https://billsplit-c8012-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        public async Task<string> AddEvent(Event eventInfo)
        {
            var result = await _client.Child("events").PostAsync(eventInfo);
            return result.Key; // This Key is the Id of the newly created event
        }

        public async Task AddParticipant(string eventId, Participant participant)
        {
            await _client
                .Child("events")
                .Child(eventId)
                .Child("participants")
                .Child(participant.Email.Replace(".", "_"))
                .PutAsync(participant);
        }

        public async Task AddFriend(string userId, Friend friend)
        {
            await _client
                .Child("users")
                .Child(userId)
                .Child("friends")
                .PostAsync(friend);
        }

        public async Task<List<Friend>> GetFriends(string userId)
        {
            try
            {
                var friendsSnapshot = await _client.Child("users").Child(userId).Child("friends").OnceAsync<Friend>();
                return friendsSnapshot?.Select(f => f.Object).ToList() ?? new List<Friend>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching friends: {ex.Message}");
                return new List<Friend>();
            }
        }

        public async Task SaveUserAsync(string userId, string username, string email)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("UserId, Username, and Email cannot be null or empty.");
            }

            await _client
                .Child("users")
                .Child(userId)
                .PutAsync(new { Username = username, Email = email });
        }

        public async Task<(string Username, string Email)> GetUserAsync(string userId)
        {
            var user = await _client
                .Child("users")
                .Child(userId)
                .OnceSingleAsync<dynamic>();

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            return (user.Username ?? string.Empty, user.Email ?? string.Empty);
        }


        public async Task<List<Event>> GetEventsWithParticipants()
        {
            var eventsSnapshot = await _client.Child("events").OnceAsync<Event>();
            var events = new List<Event>();

            foreach (var e in eventsSnapshot)
            {
                var participantsSnapshot = await _client.Child("events").Child(e.Key).Child("participants").OnceAsync<Participant>();
                var participants = participantsSnapshot.Select(p => p.Object).ToList();

                var eventItem = new Event
                {
                    Id = e.Key,
                    Name = e.Object.Name,
                    TotalCost = e.Object.TotalCost,
                    Date = e.Object.Date,
                    CreatorEmail = e.Object.CreatorEmail,
                    Participants = new ObservableCollection<Participant>(participants)
                };

                events.Add(eventItem);
            }

            return events;
        }

        public async Task DeleteEvent(string eventId)
        {
            await _client.Child("events").Child(eventId).DeleteAsync();
        }

        // New method to delete event by name
        public async Task DeleteEventByName(string eventName)
        {
            try
            {
                // Fetch all events
                var eventsSnapshot = await _client.Child("events").OnceAsync<Event>();

                // Find the event that matches the given name
                var matchingEvent = eventsSnapshot
                    .FirstOrDefault(e => string.Equals(e.Object.Name, eventName, StringComparison.OrdinalIgnoreCase));

                if (matchingEvent != null)
                {
                    // Delete the event by its key (ID)
                    await _client.Child("events").Child(matchingEvent.Key).DeleteAsync();
                    Debug.WriteLine($"Event '{eventName}' has been deleted.");
                }
                else
                {
                    Debug.WriteLine($"Event '{eventName}' not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting event by name: {ex.Message}");
            }
        }
    }
}

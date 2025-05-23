using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BillSplit.Models;
using System.Diagnostics;

namespace BillSplit.Services
{
    public class FirebaseAuthService
    {
        private const string ApiKey = "AIzaSyD4HfoatUCC0u8sa37XnT7qH-dFFHb5yk8";
        private readonly HttpClient _client = new();

        public async Task<AuthResult> Register(string email, string password, string username)
        {
            var payload = new { email, password, returnSecureToken = true };
            var response = await _client.PostAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            );
            var resultJson = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonDocument.Parse(resultJson).RootElement;
                var authResult = new AuthResult
                {
                    Success = true,
                    UserId = result.TryGetProperty("localId", out var localId) ? localId.GetString() : null,
                    IdToken = result.TryGetProperty("idToken", out var idToken) ? idToken.GetString() : null,
                    Email = result.TryGetProperty("email", out var emailProperty) ? emailProperty.GetString() : null,
                    DisplayName = username
                };

                await new FirebaseDbService().SaveUserAsync(authResult.UserId, username, authResult.Email);

                return authResult;
            }

            var error = JsonDocument.Parse(resultJson).RootElement
                           .TryGetProperty("error", out var errorElement) && errorElement
                           .TryGetProperty("message", out var errorMessage)
                           ? errorMessage.GetString()
                           : "Unknown error";
            return new AuthResult { Success = false, ErrorMessage = error };
        }

        public async Task<AuthResult> Login(string email, string password)
        {
            var payload = new { email, password, returnSecureToken = true };

            var response = await _client.PostAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            );

            var resultJson = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonDocument.Parse(resultJson).RootElement;

                // Fallback logic to ensure DisplayName is always set
                var emailValue = result.TryGetProperty("email", out var emailProp) ? emailProp.GetString() : null;
                var displayNameValue = result.TryGetProperty("displayName", out var nameProp) ? nameProp.GetString() : null;

                var authResult = new AuthResult
                {
                    Success = true,
                    UserId = result.TryGetProperty("localId", out var localId) ? localId.GetString() : null,
                    IdToken = result.TryGetProperty("idToken", out var idToken) ? idToken.GetString() : null,
                    Email = emailValue,
                    DisplayName = string.IsNullOrEmpty(displayNameValue) ? emailValue : displayNameValue
                };

                return authResult;
            }

            // Extract and return error message
            var error = JsonDocument.Parse(resultJson).RootElement
                           .TryGetProperty("error", out var errorElement) && errorElement
                           .TryGetProperty("message", out var errorMessage)
                           ? errorMessage.GetString()
                           : "Unknown error";

            return new AuthResult { Success = false, ErrorMessage = error };
        }


        public async Task<bool> ChangePassword(string newPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(AppSession.IdToken))
                {
                    Debug.WriteLine("No valid IdToken in AppSession.");
                    return false;
                }

                var userToken = AppSession.IdToken;  // Assuming IdToken is stored in AppSession after login
                var content = new StringContent(JsonSerializer.Serialize(new { idToken = userToken, password = newPassword, returnSecureToken = true }), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:update?key={ApiKey}", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error changing password: {ex.Message}");
                return false;
            }
        }
    }
}

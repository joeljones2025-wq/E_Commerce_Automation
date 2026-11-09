using System.Text.Json;

namespace E_Commerce_Automation.Pages
{
    internal sealed class TestData
    {
        public UserData ValidUser { get; set; } = new();
        public CheckoutData Checkout { get; set; } = new();
    }

    internal sealed class UserData
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    internal sealed class CheckoutData
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }

    internal static class TestDataLoader
    {
        public static TestData Load()
        {
            var path = Path.Combine(
                AppContext.BaseDirectory,
                "TestData",
                "testdata.json");
            var json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<TestData>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Could not read test data.");
        }
    }
}
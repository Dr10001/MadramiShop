using CommunityToolkit.Maui.Alerts;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MadramiShop.Services;

public static class MauiInterop
{
    private readonly static Page _page = App.Current.Windows[0].Page;
    public static async Task ToastAsync(string message) => 
        await Toast.Make(message).Show();
    public static async Task AlertAsync(string message, string title="Alert") => 
        await _page.DisplayAlert(title, message, "OK");
    public static async Task<bool> ConfirmAsync(string message, string title="Confirm") =>
        await _page.DisplayAlert(title, message, "Yes", "No");
    public static void SaveToStorage<TValue>(string key, TValue value)
    {
        var serialized = JsonSerializer.Serialize(value);
        Preferences.Default.Set(key, serialized);
    }
    public static TValue GetFromStorage<TValue>(string key, TValue defaultValue)
    {
        if (Preferences.Default.ContainsKey(key))
        {
            var serializedValue = Preferences.Default.Get<string?>(key, null);
            if(!string.IsNullOrWhiteSpace(serializedValue))
            {
                return JsonSerializer.Deserialize<TValue>(serializedValue)!;
            }
        }
        return defaultValue;
    }
    public static void RemoveFromStorage(string key) => Preferences.Default.Remove(key);

}

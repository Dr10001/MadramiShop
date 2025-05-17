using CommunityToolkit.Maui;
using MadramiShop.Apis;
using MadramiShop.Services;
using Microsoft.Extensions.Logging;
using Refit;

namespace MadramiShop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                }).UseMauiCommunityToolkit();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<CartService>()
                .AddSingleton<AppState>()
                .AddSingleton<AuthState>();
                ;
            ConfigureRefit(builder.Services);


            return builder.Build();
        }
        private static void ConfigureRefit(IServiceCollection services)
        {
            //here we gone use a global dev tunnel to test the app// it should be other to  deploy
            const string baseApiUrl = "https://0jw3b37j-7123.euw.devtunnels.ms";

            services.AddRefitClient<IProductApi>().ConfigureHttpClient(SetHttpClient);
            services.AddRefitClient<IAuthApi>().ConfigureHttpClient(SetHttpClient);
            services.AddRefitClient<IOrderApi>(GetRefitSettings).ConfigureHttpClient(SetHttpClient);
            services.AddRefitClient<IUserApi>(GetRefitSettings).ConfigureHttpClient(SetHttpClient);

            static void SetHttpClient(HttpClient httpClient) =>
                httpClient.BaseAddress = new Uri(baseApiUrl);
            static RefitSettings GetRefitSettings(IServiceProvider sp)
            {
                //de modificat
                //var authService = sp.GetRequiredService<AuthState>();asta
                var settings = new RefitSettings();
                //var settings = new RefitSettings   asta
                //{

                //AuthorizationHeaderValueGetter = (_, __) =>
                //Task.FromResult(authService.IsLoggedIn ? authService.User!.Token : "")
                //};
                settings.AuthorizationHeaderValueGetter = (_, __) => Task.FromResult("TOKEN");
                return settings;
            }
        }
    }
}

using MadramiShop.Services;
namespace MadramiShop
{
    public partial class MainPage : ContentPage
    {
        public MainPage(AppState appState)
        {
            InitializeComponent();
            BindingContext = appState;
        }
    }
}

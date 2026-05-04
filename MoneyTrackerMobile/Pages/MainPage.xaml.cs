using MoneyTrackerMobile.Models;
using MoneyTrackerMobile.PageModels;

namespace MoneyTrackerMobile.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
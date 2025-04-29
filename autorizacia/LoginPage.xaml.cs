using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace autorizacia
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public LoginPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();

            var savedUser = _databaseService.GetSavedUser();
            if (savedUser != null)
            {
                EmailEntry.Text = savedUser.Email;
                PasswordEntry.Text = savedUser.Password;
                RememberMeCheckBox.IsChecked = true;
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите почту", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите пароль", "OK");
                return;
            }

            var user = _databaseService.GetUser(email, password);

            if (user != null)
            {
                if (RememberMeCheckBox.IsChecked)
                {
                    _databaseService.SaveUserCredentials(email, password);
                }
                else
                {
                    _databaseService.ClearSavedUser();
                }

                await DisplayAlert("Успех", "Вход успешно!", "OK");

                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Ошибка", "Угадай что неправильно", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }
    }
}

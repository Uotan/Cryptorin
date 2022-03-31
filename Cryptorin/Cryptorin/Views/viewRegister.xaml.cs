using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRegister : ContentPage
    {
        FileResult file;
        public ViewRegister()
        {
            InitializeComponent();
        }


        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                file = await FilePicker.PickAsync(new PickOptions { FileTypes = FilePickerFileType.Images, PickerTitle = "Please pick the image" });
                if (file == null)
                {
                    return;
                }
                imagePicker.Source = file.FullPath;
            }
            catch (Exception ex)
            {
                await DisplayAlert("error", ex.Message, "close");
            }
        }

        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void tbPassw_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbRepPassw_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
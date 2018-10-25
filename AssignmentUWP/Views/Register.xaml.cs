using AssignmentUWP.Entity;
using AssignmentUWP.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AssignmentUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        private Member currentMember;
        private static StorageFile file;
        private static string ImageUploadUrl;

        public Register()
        {
            GetUploadUrl();
            this.currentMember = new Member();
            this.InitializeComponent();
        }

        private void BackToLogin(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Views.Login));
        }

        private async void Handle_SignUp(object sender, RoutedEventArgs e)
        {            
            CheckValid valid = new CheckValid();
            currentMember.firstName = this.FirstName.Text;
            currentMember.lastName = this.LastName.Text;
            currentMember.email = this.Email.Text;
            currentMember.password = this.Passwrd.Password.ToString();
            currentMember.avatar = this.Avatar_Url.Text;
            currentMember.phone = this.Phone.Text;
            currentMember.address = this.Address.Text;
            Debug.WriteLine("First Name" + this.FirstName.Text);
            Debug.WriteLine(this.LastName.Text);
            Debug.WriteLine(this.Address.Text);
            if (!valid.CheckValidAll(currentMember))
            {
                List<TextBlock> listMessage = new List<TextBlock>();
                foreach (KeyValuePair<string, string> entry in valid.listError)
                {
                    var message = this.FindName(entry.Key);
                    TextBlock textBlock = message as TextBlock;
                    textBlock.Text = entry.Value;                    
                    textBlock.Visibility = Visibility.Visible;
                }
            }
            else
            {
                string response = await ApiHandle.Sign_Up(currentMember);
                Debug.WriteLine(response);
                Debug.WriteLine("OK");
            }            
        }

        private static async void GetUploadUrl()
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("https://1-dot-backup-server-002.appspot.com/get-upload-token");
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";
            try
            {
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
            Debug.WriteLine(httpResponseBody);
            ImageUploadUrl = httpResponseBody;
        }

        private void Select_Gender(object sender, RoutedEventArgs e)
        {
            RadioButton radioGender = sender as RadioButton;
            this.currentMember.gender = Int32.Parse(radioGender.Tag.ToString());
            Debug.WriteLine(this.currentMember.gender);
        }

        private void Change_Birthday(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            this.currentMember.birthday = sender.Date.Value.ToString("yyyy-MM-dd");
        }

        private void Typing(object sender, KeyRoutedEventArgs e)
        {
            this.FirstName_Message.Visibility = Visibility.Collapsed;
            this.LastName_Message.Visibility = Visibility.Collapsed;
            this.Email_Message.Visibility = Visibility.Collapsed;
            this.Password_Message.Visibility = Visibility.Collapsed;
            this.Phone_Message.Visibility = Visibility.Collapsed;
            this.Address_Message.Visibility = Visibility.Collapsed;
        }
    }
}

﻿using AssignmentUWP.Entity;
using AssignmentUWP.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
    public sealed partial class Login : Page
    {
        private static string API_LOGIN = "http://2-dot-backup-server-002.appspot.com/_api/v2/members/authentication";

        public Login()
        {
            this.InitializeComponent();
        }

        private void LoadRegister(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Views.Register));
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            CheckValid valid = new CheckValid();
            string email = this.Email.Text;
            string password = this.Password.Password;
            if(valid.CheckValidEmail(email) == false)
            {
                this.Email_Message.Text = valid.Email_Message;                                
                this.Email_Message.Visibility = Visibility.Visible;                
            }
            if(valid.CheckValidPassword(password) == false)
            {
                this.Password_Message.Text = valid.Password_Message;
                this.Password_Message.Visibility = Visibility.Visible;
            }
            else
            {
                Dictionary<string, string> LoginInfo = new Dictionary<string, string>();
                LoginInfo.Add("email", this.Email.Text);
                LoginInfo.Add("password", this.Password.Password);

                Debug.WriteLine(this.Email.Text);
                Debug.WriteLine(this.Password.Password);

                // Lay token
                HttpClient httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(LoginInfo), System.Text.Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync(API_LOGIN, content).Result;
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    // save file...
                    Debug.WriteLine(responseContent);
                    // Doc token
                    TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                    // Luu token
                    StorageFolder folder = ApplicationData.Current.LocalFolder;
                    StorageFile file = await folder.CreateFileAsync("token.txt", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file, responseContent);

                    // Lay thong tin ca nhan bang token.
                    HttpClient client2 = new HttpClient();
                    client2.DefaultRequestHeaders.Add("Authorization", "Basic " + token.token);
                    var resp = client2.GetAsync("http://2-dot-backup-server-002.appspot.com/_api/v2/members/information").Result;
                    Debug.WriteLine(await resp.Content.ReadAsStringAsync());
                    (Window.Current.Content as Frame).Navigate(typeof(Views.Main));
                }
                else
                {
                    // Xu ly loi.
                    ErrorResponse errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                    Debug.WriteLine(responseContent);
                    if (errorObject != null && errorObject.error.Count > 0)
                    {
                        foreach (var key in errorObject.error.Keys)
                        {
                            var textMessage = this.FindName(key);
                            if (textMessage == null)
                            {
                                continue;
                            }
                            TextBlock textBlock = textMessage as TextBlock;
                            textBlock.Text = errorObject.error[key.ToLower() +"_Message"];
                            textBlock.Visibility = Visibility.Visible;
                        }
                    }
                }
            }            
        }
    }
}

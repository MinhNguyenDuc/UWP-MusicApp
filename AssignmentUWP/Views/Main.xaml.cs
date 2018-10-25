using AssignmentUWP.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
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
    public sealed partial class Main : Page
    {
        private ObservableCollection<Song> listSong;
        private ObservableCollection<Song> listMySong;

        private static string SONG_API_URL = "https://2-dot-backup-server-002.appspot.com/_api/v2/songs";

        internal ObservableCollection<Song> ListSong { get => listSong; set => listSong = value; }

        internal ObservableCollection<Song> ListMySong { get => listMySong; set => listMySong = value; }

        public Main()
        {
            GetListMySongAsync();
            GetListSongAsync();
            
            this.InitializeComponent();            
        }

        public async void GetListSongAsync()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("token.txt");
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            Debug.WriteLine("new Token " + text);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(text);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token.token);             
            var response = client.GetAsync("https://2-dot-backup-server-002.appspot.com/_api/v2/songs").Result;
            var arraysong = await response.Content.ReadAsStringAsync();
            ListSong = JsonConvert.DeserializeObject<ObservableCollection<Song>>(arraysong.ToString());            
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel panel = sender as StackPanel;
            Song selectedSong = panel.Tag as Song;
            Debug.WriteLine(selectedSong.link);
            Uri path = new Uri(selectedSong.link);
            SongPlayer.Source = path;
            rootPivot.Title = selectedSong.name;
        }

        private async void UpLoadSong(object sender, RoutedEventArgs e)
        {
            Song song = new Song();
            song.name = SongName.Text;
            song.author = SongAuthor.Text;
            song.singer = SongSinger.Text;
            song.thumbnail = SongThumbnail.Text;
            song.link = SongUrl.Text;
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("token.txt");
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(text);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", token.token);
            var content = new StringContent(JsonConvert.SerializeObject(song), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(SONG_API_URL, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(contents);
            GetListMySongAsync();
            GetListMySongAsync();
            rootPivot.SelectedIndex = 0;
        }

        public async void GetListMySongAsync()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("token.txt");
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(text);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token.token);
            var response = client.GetAsync("https://2-dot-backup-server-002.appspot.com/_api/v2/songs/get-mine").Result;
            var arraysong = await response.Content.ReadAsStringAsync();
            ListMySong = JsonConvert.DeserializeObject<ObservableCollection<Song>>(arraysong.ToString());
        }

        

        //private void MySong_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    StackPanel panel = sender as StackPanel;
        //    Song selectedSong = panel.Tag as Song;
        //    Debug.WriteLine(selectedSong.link);
        //    Uri path = new Uri(selectedSong.link);
        //    MySongPlayer.Source = path;
        //    rootPivot.Title = selectedSong.name;
        //}
    }
}

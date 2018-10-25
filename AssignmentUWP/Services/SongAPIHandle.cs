using AssignmentUWP.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentUWP.Services
{
    class SongAPIHandle
    {
        private static string SONG_API_URL = "http://1-dot-backup-server-002.appspot.com/song/create";
        private static string GET_LIST_SONG_API = "https://2-dot-backup-server-002.appspot.com/_api/v2/songs";

        public async void GetListSongAsync()
        {

        }

        public async static Task<string> Create_Song(Song song)
        {
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(song), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(SONG_API_URL, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(contents);
            return contents;
        }
    }
}

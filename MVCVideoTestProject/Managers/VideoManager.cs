using CampStayTechnicalAssesment.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CampStayTechnicalAssesment.Managers
{
    public class VideoManager
    {
        private HttpClient _client;

        public VideoManager(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<VideoModel>> GetVideos()
        {          
            var jsonVideos = await _client.GetStringAsync("https://my-json-server.typicode.com/Campstay/youtube-test/videos");
            List<VideoModel> videoList = JsonConvert.DeserializeObject<List<VideoModel>>(jsonVideos);
            return (videoList);
        }

        public async Task<List<VideoModel>> AddUploaderToVideos(List<VideoModel> videos)
        {
            List<string> urls = new List<string>();
            foreach (var video in videos)
            {
                urls.Add("https://my-json-server.typicode.com/Campstay/youtube-test/users/" + video.UserId.ToString());
            }

            // I researched how to make the calls in parallel, as it is bad practise to be sending requests within a foreach loop. https://stackoverflow.com/questions/12337671/using-async-await-for-multiple-tasks
            var jsons = await Task.WhenAll(urls.Select(x => _client.GetStringAsync(x)));

            List<UserModel> users = this.ParsingJsonArrayToUserList(jsons);

            foreach (var video in videos)
            {
                var user = users.FirstOrDefault(x => x.Id == video.UserId);
                video.Uploader = user.Name;
            }

            return (videos);
        }

        public async Task<VideoModel> GetSelectedVideo(int id)
        {
            string url = "https://my-json-server.typicode.com/Campstay/youtube-test/videos/" + id.ToString();
            var jsonVideo = await _client.GetStringAsync(url);
            VideoModel video = JsonConvert.DeserializeObject<VideoModel>(jsonVideo);

            return(video);
        }

        public async Task<List<CommentModel>> GetSelectedVideoComments(int id)
        {
            string url = "https://my-json-server.typicode.com/Campstay/youtube-test/videos/" + id.ToString() + "/comments";
            var jsonComments = await _client.GetStringAsync(url);
            List<CommentModel> commentList = JsonConvert.DeserializeObject<List<CommentModel>>(jsonComments);

            return (commentList);
        }

        public async Task<List<CommentModel>> AddUserToComments(List<CommentModel> commentList)
        {
            List<string> urls = new List<string>();
            List<int> userIds = commentList.Select(x => x.UserId).Distinct().ToList();
            foreach (var userId in userIds)
            {
                urls.Add("https://my-json-server.typicode.com/Campstay/youtube-test/users/" + userId);
            }

            // I researched how to make the calls in parallel, as it is bad practise to be sending requests within a foreach loop. https://stackoverflow.com/questions/12337671/using-async-await-for-multiple-tasks
            var jsons = await Task.WhenAll(urls.Select(x => _client.GetStringAsync(x)));

            List<UserModel> users = this.ParsingJsonArrayToUserList(jsons);

            foreach (var comment in commentList)
            {
                var user = users.FirstOrDefault(x => x.Id == comment.UserId);
                comment.CommentName = user.Name;
            }

            return (commentList);
        }

        private List<UserModel> ParsingJsonArrayToUserList(String[] jsons)
        {
            List<UserModel> users = new List<UserModel>();
            foreach (var json in jsons.ToList())
            {
                users.Add(JsonConvert.DeserializeObject<UserModel>(json));
            }

            return (users);
        }
    }
}

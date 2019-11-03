using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CampStayTechnicalAssesment.Models;
using System.Net.Http;
using CampStayTechnicalAssesment.Managers;

namespace CampStayTechnicalAssesment.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            VideoManager manager = new VideoManager(client);
            List<VideoModel> videos = await manager.GetVideos();
            List<VideoModel> updatedVideos = await manager.AddUploaderToVideos(videos);
            return View(updatedVideos);
        }

        public async Task<IActionResult> Video(int id)
        {
            var client = _httpClientFactory.CreateClient();
            VideoManager manager = new VideoManager(client);

            VideoModel video = await manager.GetSelectedVideo(id);
            List<CommentModel> commentList = await manager.GetSelectedVideoComments(id);
            List<CommentModel> updatedCommentList = await manager.AddUserToComments(commentList);

            SelectedVideoModel selectedVideo = new SelectedVideoModel(video, updatedCommentList);

            return View(selectedVideo);
        }
    }
}

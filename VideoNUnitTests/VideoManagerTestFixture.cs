
using CampStayTechnicalAssesment.Managers;
using CampStayTechnicalAssesment.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests
{
    public class VideoManagerTestFixture
    
    {
        private VideoManager _manager;

        [SetUp]
        public void Setup()
        {
            HttpClient client = new HttpClient();
            _manager = new VideoManager(client);
        }

        [Test]
        public async Task TestVideoMangerGetVideoList()
        {
            List<VideoModel> videos = await _manager.GetVideos();            
            Assert.AreEqual(videos.Count, 3);
        }

        [Test]
        public async Task TestVideoManagerAddUploaderToVideos()
        {
           
            List<VideoModel> videos = await _manager.GetVideos();
            Assert.AreEqual(videos.Count, 3);

            List<VideoModel> UpdatedVideos = await _manager.AddUploaderToVideos(videos);
            Assert.NotNull(UpdatedVideos[0].Uploader);
        }

        [Test]
        public async Task TestVideoManagerGetSelectedVideo()
        {
            VideoModel video = await _manager.GetSelectedVideo(2);
            Assert.AreEqual(video.Id, 2);
        }

        [Test]
        public async Task TestVideoManagerGetSelectedVideoComments()
        {
            List<CommentModel> commentList = await _manager.GetSelectedVideoComments(2);
            Assert.AreEqual(commentList.Count, 5);
        }

        [Test]
        public async Task TestVideoManagerAddUserToComments()
        {
            List<CommentModel> commentList = await _manager.GetSelectedVideoComments(2);
            List<CommentModel> updatedCommentList = await _manager.AddUserToComments(commentList);
            Assert.NotNull(updatedCommentList[0].CommentName);
        }
  
    }
}
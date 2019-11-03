using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampStayTechnicalAssesment.Models
{
    public class SelectedVideoModel
    {
       
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime UploadedAt { get; set; }

        public string Url { get; set; }

        public string Size { get; set; }

        public List<CommentModel> Comments { get; set; }

        public SelectedVideoModel(VideoModel video, List<CommentModel> commentList)
        {
            Id = video.Id;
            UserId = video.UserId;
            Title = video.Title;
            Description = video.Description;
            UploadedAt = video.UploadedAt;
            Url = video.Url;
            Size = video.Size;
            Comments = commentList;
        }

    }
}

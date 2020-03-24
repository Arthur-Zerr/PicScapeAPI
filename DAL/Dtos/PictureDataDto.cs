using System;

namespace PicScapeAPI.DAL.Dtos
{
    public class PictureDataDto
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public string UploadDate { get; set; }
    }
}
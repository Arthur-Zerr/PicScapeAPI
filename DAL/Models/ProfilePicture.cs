using System;

namespace PicScapeAPI.DAL.Models
{
    public class ProfilePicture : BaseModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public DateTime UploadDate { get; set; }
        public byte[] Img{ get; set; }
        public string ImgType { get; set; } 
        public bool isCurrentPicture { get; set; }
    }
}
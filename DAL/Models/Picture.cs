using System;

namespace PicScapeAPI.DAL.Models
{
    public class Picture : BaseModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public DateTime UploadDate { get; set; }
        public byte[] Img{ get; set; }
        public string ImgType { get; set; } 
    }
}
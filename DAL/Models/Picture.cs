using System;

namespace PicScapeAPI.DAL.Models
{
    public class Picture
    {
        public int ID { get; set; }
        //public int UserID { get; set; }
        public User User { get; set; }

        public string Url { get; set; }
        public string Title { get; set; }
        public DateTime UploadDate { get; set; }    
    }
}
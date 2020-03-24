namespace PicScapeAPI.DAL.Dtos
{
    public class PictureDto
    {
        public int id { get; set; }
        public string UserID { get; set; }
        public byte[] Img{ get; set; }
    }
}
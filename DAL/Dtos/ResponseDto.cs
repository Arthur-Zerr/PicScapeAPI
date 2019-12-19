namespace PicScapeAPI.DAL.Dtos
{
    public class ResponseDto
    {
        public string Message { get; set; }
        public bool Show { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }
    }
}
using System;
namespace PicScapeAPI.Helper
{
    public class UploadHelper
    {
        public string GetUploadId(string type, int id)
        {
            return $"{DateTime.Now.ToLongDateString()}_{type}_{id}";
        }
    }
}
using System;
using PicScapeAPI.DAL.Dtos;
using PicScapeAPI.Localization;
using Newtonsoft.Json;
using System.Text.Json;

namespace PicScapeAPI.Helper {
    public class GenericResponse {
        private readonly ResponseLocalization responseLocalization;

        public GenericResponse (ResponseLocalization responseLocalization) {
            this.responseLocalization = responseLocalization;
        }

        //Needed ? Find better waay to send Result
        public ResponseDto GetResponseWithData (string key, bool show, bool success, object data, string lang = "EN") 
        {
            var message = responseLocalization.getRessource(key, lang);
            string jsonData = JsonConvert.SerializeObject(data);
            //string jsonData = JsonSerializer.Serialize(data);
            var temp = new ResponseDto {Message = message, Show = show ,Success = success, Data = jsonData};

            return temp;
        }

            public ResponseDto GetResponseWithDataString (string key, bool show, bool success, string data, string lang = "EN") 
            {
                var message = responseLocalization.getRessource(key, lang);
                //string jsonData = JsonSerializer.Serialize(data);
                var temp = new ResponseDto {Message = message, Show = show ,Success = success, Data = data};

                return temp;
            }

        public ResponseDto GetResponse(string key, bool show, bool success, string lang = "EN")
        {
            var message = responseLocalization.getRessource(key, lang);
            var temp = new ResponseDto {Message = message,Show = show,  Success = success, Data = "null" };

            return temp;
        }
    }
}
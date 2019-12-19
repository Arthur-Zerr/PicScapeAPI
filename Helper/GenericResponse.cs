using System;
using PicScapeAPI.DAL.Dtos;
using PicScapeAPI.Localization;

namespace PicScapeAPI.Helper {
    public class GenericResponse {
        private readonly ResponseLocalization responseLocalization;

        public GenericResponse (ResponseLocalization responseLocalization) {
            this.responseLocalization = responseLocalization;
        }
        public ResponseDto GetResponseWithData (string key, bool show, bool success, object data, string lang = "EN") {
            var message = responseLocalization.getRessource(key, lang);
            var temp = new ResponseDto {Message = message, Show = show ,Success = success, Data = data };

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
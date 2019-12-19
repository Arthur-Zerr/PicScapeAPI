using System.Xml;
namespace PicScapeAPI.Localization
{
    public class ResponseLocalization
    {
        public static XmlDocument genericResponseDocument;

        public ResponseLocalization()
        {
            if(genericResponseDocument == null)
                genericResponseDocument = loadDocument();
            
        }

        public XmlDocument loadDocument()
        {
            var documnet = new XmlDocument();
            try
            {
                documnet.Load("./Localization/response.xml");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return documnet;
        }

        public string getRessource(string key, string language)
        {
            string Message = "";
            XmlNodeList respones = genericResponseDocument.SelectNodes($"/GenericRespones/Response[@key='{key}']");
            foreach (XmlNode responseNode in respones)
            {
                var translationList = responseNode.ChildNodes;
                foreach (XmlNode translation in translationList)
                {
                    if(translation.Attributes["lang"].Value == language)
                    {
                        Message = translation.InnerText;
                    }
                }
            }

            if(string.IsNullOrEmpty(Message))Message = $"Response not Found with Key: {key} and Language: {language}";
            return Message;
        }
    }
}
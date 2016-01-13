using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using System.Web.Services;
using System.Runtime.Serialization.Json;

namespace Task1PartB
{
    public partial class bingTranslator : System.Web.UI.Page
    {
        public class AdmAccessToken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string scope { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            tokentxt.Text = getToken().access_token;
        }

        protected AdmAccessToken getToken()

        {
            try { 
                //retrieve the clientID and clientSecret from web.config

                string clientID = ConfigurationManager.AppSettings["ClientID"].ToString();
                string clientSecret = ConfigurationManager.AppSettings["ClientSecret"].ToString();
                String strTranslatorAccessURI = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
                String strRequestDetails = string.Format("grant_type=client_credentials&client_id={0}&client_secret ={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientID), HttpUtility.UrlEncode(clientSecret));

                WebRequest webRequest = WebRequest.Create(strTranslatorAccessURI);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";

                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strRequestDetails);
                webRequest.ContentLength = bytes.Length;

                using (System.IO.Stream outputStream = webRequest.GetRequestStream())
                {
                    outputStream.Write(bytes, 0, bytes.Length);
                }

                //send request to get authentication token
                WebResponse webResponse = webRequest.GetResponse();

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));

                //Get deserialized object from JSON stream
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());

                return token;
            }catch (Exception e)
            {
                //lbl1.Text = "" + e;
                return null;
            }
        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Xml;
using System.IO;

namespace WebApplication1
{
    public partial class WeatherService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XmlDocument wsResponseXmlDoc = new XmlDocument();

            //http://api.worldweatheronline.com/free/v2/weather.ashx?q=china&format=xml&num_of_days=5&key=x35ahuadjhmdp5rb75ddw2ha
            //id=jipx(spacetime0)
            UriBuilder url = new UriBuilder();
            url.Scheme = "http";// Same as "http://"

            String key = System.Configuration.ConfigurationManager.AppSettings["key"];

            url.Host = "api.worldweatheronline.com";
            url.Path = "free/v1/weather.ashx";
            url.Query = "q=china&format=xml&num_of_days=5&key=" + key;

            String cacheurl = File.ReadAllText("D:/New folder/Visual Studio 2015/Projects/WebApplication1/WebApplication1/cache.txt");

            if (String.Compare(cacheurl, url.ToString()) == 0)
            {
                wsResponseXmlDoc = new XmlDocument();
                wsResponseXmlDoc.Load("D:/New folder/Visual Studio 2015/Projects/WebApplication1/WebApplication1/WeatherServiceCache.xml");

                //display the XML response 
                String xmlString = wsResponseXmlDoc.InnerXml;
                Response.ContentType = "text/xml";
                Response.Write(xmlString);
            }
            else
            {

                //Make a HTTP request to the global weather web service
                wsResponseXmlDoc = MakeRequest(url.ToString());

                int count = 0;

                while (count != 3 && wsResponseXmlDoc == null)
                {
                    wsResponseXmlDoc = MakeRequest(url.ToString());
                    count++;
                }

                if (wsResponseXmlDoc != null)
                {
                    //display the XML response 
                    String xmlString = wsResponseXmlDoc.InnerXml;
                    Response.ContentType = "text/xml";
                    Response.Write(xmlString);

                    String folder = "D:/New folder/Visual Studio 2015/Projects/WebApplication1/WebApplication1";
                    File.WriteAllText(folder + @"/cache.txt", url.ToString());
                    wsResponseXmlDoc.Save("D:/New folder/Visual Studio 2015/Projects/WebApplication1/WebApplication1/WeatherServiceCache.xml");
                }
                else
                {
                    Response.ContentType = "text/html";
                    Response.Write("<h2> error  accessing web service </h2>");

                }
            }
        }

        public static XmlDocument MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                //set timeout to 15 seconds
                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);
            }catch (Exception e)
            {
                return null;
            }
        }
    }
}
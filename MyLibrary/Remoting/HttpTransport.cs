using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using JSegarra.Core;

namespace JSegarra.Remote
{
    class HttpTransport
    {

        // https://github.com/mspnp/performance-optimization/blob/master/ImproperInstantiation/docs/ImproperInstantiation.md
        private static readonly HttpClient client = new HttpClient();


        private System.IO.Stream Upload(string actionUrl, string paramString, Stream paramFileStream, byte[] paramFileBytes)
        {
            HttpContent stringContent = new StringContent(paramString);
            HttpContent fileStreamContent = new StreamContent(paramFileStream);
            HttpContent bytesContent = new ByteArrayContent(paramFileBytes);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(stringContent, "param1", "param1");
                formData.Add(fileStreamContent, "file1", "file1");
                formData.Add(bytesContent, "file2", "file2");
                var response = client.PostAsync(actionUrl, formData).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return response.Content.ReadAsStreamAsync().Result;
            }
        }

        https://code.msdn.microsoft.com/windowsapps/How-to-use-HttpClient-to-b9289836

        static internal byte[] SendMessage(Uri where,byte[] message)
        {
            Logger.Green("Hello");
            var response = await client.PostAsync(where,message);

            HttpClient client = new HttpClient();
            //Preparing to have something to read
            var stringContent = new StringContent("someHardCodedStringToTellTheServerToPublishTheDataTheAppWillConsume");
            var sending = await client.PostAsync(url, stringContent);

            //Reading data
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
            catch
            {
                return default(T);
            }   

            return null;
        }


    }
}

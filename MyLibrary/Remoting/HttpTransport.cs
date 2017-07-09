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

        //https://code.msdn.microsoft.com/windowsapps/How-to-use-HttpClient-to-b9289836

        static async internal Task<string> SendMessage(Uri where,string message)
        {
            try
            { 
                Logger.Green("Sending message to "+where);

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(where, new StringContent(message));                               // Posts message
                response.EnsureSuccessStatusCode();                                                                                     // Throws an exception if failed
                string responseBody = await response.Content.ReadAsStringAsync();                                                       // Read response
                return responseBody;                                                                                                    // And return it
            }
            catch
            {
                return null;
            }   
        }

    }
}

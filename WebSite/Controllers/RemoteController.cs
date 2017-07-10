using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using JSegarra.JSON;
using JSegarra.RemoteHost;

// http://www.hanselman.com/blog/HowToRunBackgroundTasksInASPNET.aspx
// http://haacked.com/archive/2011/10/16/the-dangers-of-implementing-recurring-background-tasks-in-asp-net.aspx/
// https://www.hangfire.io/
// https://msdn.microsoft.com/en-us/library/ms227673.aspx


public class RemoteController : ApiController
{

    delegate Json ExecuteAction(Json v);
    static Sessions sessions = new Sessions();

    static Dictionary<string, ExecuteAction> actions = new Dictionary<string, ExecuteAction>()
    {
         {"login", Login },
         {"chunk",Chunk }
    };

    static Json GetPublicKey(Json v)
    {
        return null;
    }


    static Json Login(Json jsonIn)
    {                                                                                                               //
        Session s = sessions.Add(new Session(jsonIn));
        Json jsonOut= new Json(JsonKind.Object);
        jsonOut["id"] = s.id.ToString();
        return jsonOut;
    }

    static Json Chunk(Json jsonIn)
    {
        string sessionId = jsonIn.Get("id","");
        int chunk = jsonIn.GetI("chunk",-1);
        int total= jsonIn.GetI("total",-1);
        byte[] data = (byte[])jsonIn["data"];
        if (sessionId=="" || chunk==-1 || total==-1 || data==null) return JSegarra.JSON.Json.Parse("{ ok: false, msg:'Bad params for CHUNK action '}");

        Json jsonOut = new Json(JsonKind.Object);
            
        jsonOut["id"] = sessionId;
        jsonOut["id1"] = chunk;
        jsonOut["id2"] = total;
        jsonOut["id3"] = data.Length;

        return jsonOut;
    }


    HttpResponseMessage Build(bool ok,Json c)
    {
        var response = this.Request.CreateResponse(HttpStatusCode.OK);
        if (c["ok"] == null) c["ok"] = ok;
        response.Content = new StringContent(c.ToString(), Encoding.UTF8, "application/json");
        return response;

    }



    [HttpPost]
    public HttpResponseMessage Action()
    {
        HttpContent requestContent = Request.Content;                                                               // Get the request content (this is POST)
        string jsonContent = requestContent.ReadAsStringAsync().Result;                                             // Get all request content

        string ip = System.Web.HttpContext.Current.Request.UserHostAddress;                                         // Get ip
        Json m=JSegarra.JSON.Json.Parse(HttpUtility.UrlDecode(jsonContent));                                        // Parse the request
        string action = m.Get("action", "");                                                                        // Get action or default
        
        Helper.Log("RemoteController.Action");
        Helper.Log2("ip",ip);
        Helper.Log2("action",action);

        ExecuteAction ec;
        if (!actions.TryGetValue(action,out ec))
        {
            Helper.Error("BAD ACTION: "+action);
            return Build(false, JSegarra.JSON.Json.Parse("{ msg:'Bad action " + action + "'}"));
        }
        return Build(true,ec(m));
    }
}
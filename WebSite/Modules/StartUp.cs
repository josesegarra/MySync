using System;
using System.Web;


public class Helper
{
    static int i = 10;
    static object myLock = new object();
    static string LogFolder = @"C:\temp\";


    public static void Log(string a, bool clear = false)
    {
        lock (myLock)
        {
            if (System.IO.Directory.Exists(LogFolder))
            {
                if (clear) System.IO.File.WriteAllText(LogFolder+"myremote.txt", "Clearing log\n\n");
                System.IO.File.AppendAllText(LogFolder + "myremote.txt", i.ToString().PadLeft(8) + " " + a.Trim() + "\n");
                i++;
            }
        }
    }

    public static void Error(string a)
    {
        lock (myLock)
        {
            if (System.IO.Directory.Exists(LogFolder))
            {
                System.IO.File.AppendAllText(LogFolder + "myremote.txt", "*********** " + a.Trim() + "\n");
            }
        }
    }


    public static void Log2(string a,string b)
    {
        lock (myLock)
        {
            if (System.IO.Directory.Exists(LogFolder))
            {
                a = a.Trim().PadLeft(15);
                System.IO.File.AppendAllText(LogFolder + "myremote.txt", "                     " + a + " " + b + "\n");
            }
        }
    }
    
}





// https://support.microsoft.com/es-es/help/2666571/cookies-added-by-a-managed-httpmodule-are-not-available-to-native-ihtt

public class MyRemoteHostingModule : IHttpModule
{
  
   
    public MyRemoteHostingModule()
    {
        Helper.Log("Creating MyRemoteHostingModule",true);
    }


    public String ModuleName
    {
        get { return "MyRemoteHostingModule "; }
    }

    public void Init(HttpApplication application)
    {
        Helper.Log("Init MyRemoteHostingModule");
        application.BeginRequest += (new EventHandler(this.Application_BeginRequest));
        application.EndRequest += (new EventHandler(this.Application_EndRequest));
    }



    private void Application_BeginRequest(Object source,EventArgs e)
    {
        
        // Create HttpApplication and HttpContext objects to access request and response properties.
        HttpApplication application = (HttpApplication)source;
        HttpContext context = application.Context;
        string name = DateTime.Now.ToString("Session_yyy.MM.dd_HH:mm:ss");
        string filePath = context.Request.FilePath;
        Helper.Log("Starting a request for filePath: " + filePath);
        //Log("In session                       " + name);
        // Beware here context.Session is null
    }

    private void Application_EndRequest(Object source, EventArgs e)
    {
        HttpApplication application = (HttpApplication)source;
        HttpContext context = application.Context;

        Helper.Log("End request");
    }

    public void Dispose() { }
}
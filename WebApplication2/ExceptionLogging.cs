using System;
using System.Configuration;
using System.IO;
using context = System.Web.HttpContext;

/// <summary>  
/// Summary description for ExceptionLogging  
/// </summary>  
public static class ExceptionLogging
{

    private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

    public static void SendErrorToText(Exception ex)
    {
        var line = Environment.NewLine + Environment.NewLine;

        ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
        Errormsg = ex.GetType().Name.ToString();
        extype = ex.GetType().ToString();
        exurl = context.Current.Request.Url.ToString();
        ErrorLocation = ex.Message.ToString();

        try
        {
            string filepath = @"\\ephmc.com\corp-filesrv\AppLogs\UWPriorityRedirect\";
            string server = ConfigurationManager.AppSettings["testEmail"].ToString() == "Y" ?
                "devv-web01" :
                "PRDV-WEBEXT01";
            filepath = filepath + "uwpriorityredirect_" + server + "_" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";
            
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(filepath))
            {
                string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                sw.WriteLine("-------------------------------------------------------------------------------------");
                sw.WriteLine(line);
                sw.WriteLine(error);
                sw.WriteLine("--------------------------------*End*------------------------------------------");
                sw.WriteLine(line);
                sw.Flush();
                sw.Close();
            }
        }
        catch (Exception e)
        {
            e.ToString();
        }
    }

    public static void SendInfoToText(string user, string loan1, string loan2)
    {
        var line = user + " has sent a request to swap these loans: " + loan1 + " and " + loan2;

        try
        {
            string filepath = @"\\ephmc.com\corp-filesrv\AppLogs\UWPriorityRedirect\";
            string server = ConfigurationManager.AppSettings["testEmail"].ToString() == "Y" ?
                "devv-web01" :
                "PRDV-WEBEXT01";
            filepath = filepath + "uwpriorityredirect_" + server + "_" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";
            
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(filepath))
            {
                string error = "Log Written Date:" + " " + DateTime.Now.ToString();
                sw.WriteLine("-----------Info Details on " + " " + DateTime.Now.ToString() + "-----------------");
                sw.WriteLine("-------------------------------------------------------------------------------------");
                sw.WriteLine(line);
                sw.WriteLine("--------------------------------*End*------------------------------------------");
                sw.Flush();
                sw.Close();
            }
        }
        catch (Exception e)
        {
            e.ToString();
        }
    }

}
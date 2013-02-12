using System.Text;
using UnityEngine;
using Mixpanel.NET;

public class UnityMixpanelHttp : IMixpanelHttp
{
    public string Get(string uri, string query)
    {
        Debug.Log(uri + "?" + query);
        new WWW(uri + "?" + query);
        return "1"; //TODO: Hack, return success code immediately. Changes need to be made to Mixpanel.Net to support Async requests
    }

    public string Post(string uri, string body)
    {
        Debug.Log(uri + "?" + body);
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        new WWW(uri, bodyBytes);
        return "1"; //TODO: Hack, return success code immediately. Changes need to be made to Mixpanel.Net to support Async requests
    }
}

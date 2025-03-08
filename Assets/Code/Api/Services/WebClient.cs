using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebClient : MonoBehaviour
{
    public string baseUrl;
    private string token;

    public void SetToken(string token)
    {
        this.token = token;
    }


    public async Awaitable<IWebRequestReponse> SendGetRequest(string route, Dictionary<string, string> headers)
    {
        UnityWebRequest webRequest = CreateWebRequest("GET", route, "");

        foreach (var header in headers)
        {
            webRequest.SetRequestHeader(header.Key, header.Value);
        }

        return await SendWebRequest(webRequest);
    }


    public async Awaitable<IWebRequestReponse> SendGetRequest(string route)
    {
        UnityWebRequest webRequest = CreateWebRequest("GET", route, "");
        return await SendWebRequest(webRequest);
    }

    public async Awaitable<IWebRequestReponse> SendPostRequest(string route, string data)
    {
        UnityWebRequest webRequest = CreateWebRequest("POST", route, data);
        return await SendWebRequest(webRequest);
    }

    public async Awaitable<IWebRequestReponse> SendPostRequest(string route, string data, Dictionary<string, string> headers)
    {
        UnityWebRequest webRequest = CreateWebRequest("POST", route, data);

        foreach (var header in headers)
        {
            webRequest.SetRequestHeader(header.Key, header.Value);
        }

        return await SendWebRequest(webRequest);
    }


    public async Awaitable<IWebRequestReponse> SendDeleteRequest(string route, Dictionary<string, string> headers)
    {
        UnityWebRequest webRequest = CreateWebRequest("DELETE", route, "");
        foreach (var header in headers)
        {
            webRequest.SetRequestHeader(header.Key, header.Value);
        }

        return await SendWebRequest(webRequest);
    }


    public async Awaitable<IWebRequestReponse> SendPutRequest(string route, string data)
    {
        UnityWebRequest webRequest = CreateWebRequest("PUT", route, data);
        return await SendWebRequest(webRequest);
    }

    private UnityWebRequest CreateWebRequest(string type, string route, string data)
    {
        string url = baseUrl + route;
        //Debug.Log("Creating " + type + " request to " + url + " with data: " + data);

        data = RemoveIdFromJson(data); 
        var webRequest = new UnityWebRequest(url, type);
        byte[] dataInBytes = new UTF8Encoding().GetBytes(data);
        webRequest.uploadHandler = new UploadHandlerRaw(dataInBytes);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        AddToken(webRequest);
        return webRequest;
    }

    private async Awaitable<IWebRequestReponse> SendWebRequest(UnityWebRequest webRequest)
    {
        await webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                string responseData = webRequest.downloadHandler.text;
                return new WebRequestData<string>(responseData);
            default:
                return new WebRequestError(webRequest.error);
        }
    }
 
    private void AddToken(UnityWebRequest webRequest)
    {
        webRequest.SetRequestHeader("Authorization", "Bearer " + token);
    }

    private string RemoveIdFromJson(string json)
    {
        return json.Replace("\"id\":\"\",", "");
    }

}

[Serializable]
public class Token
{
    public string tokenType;
    public string accessToken;
}

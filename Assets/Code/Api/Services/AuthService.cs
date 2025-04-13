using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class AuthService
{
    private WebClient webClient;

    public AuthService()
    {
        webClient = ApiClientManager.Instance.GetClient();
    }

    public async Task<IWebRequestReponse> RegisterUser(string email, string password)
    {
        string data = JsonUtility.ToJson(new User(email, password));
        Debug.Log($"Sending register request with data: {data}");
        
        var headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" }
        };
        
        IWebRequestReponse response = await webClient.SendPostRequest("/account/register", data, headers);
        
        if (response is WebRequestError error)
        {
            Debug.LogError($"Register failed: {error}");
        }
        
        return response;
    }

    public async Task<IWebRequestReponse> LoginUser(string email, string password)
    {
        string data = JsonUtility.ToJson(new User(email, password));
        IWebRequestReponse response = await webClient.SendPostRequest("/account/login", data);

        if (response is WebRequestData<string> dataResponse)
        {
            Token token = JsonUtility.FromJson<Token>(dataResponse.Data);
            ApiClientManager.Instance.SetToken(token.accessToken);
        }

        return response;
    }
}
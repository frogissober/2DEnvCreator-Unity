using UnityEngine;
using System.Threading.Tasks;

public class AuthService
{
    private WebClient webClient;

    public AuthService()
    {
        webClient = new WebClient();
    }

    public async Task<IWebRequestReponse> RegisterUser(string email, string password)
    {
        string data = JsonUtility.ToJson(new User(email, password));
        IWebRequestReponse response = await webClient.SendPostRequest("/account/register", data);
        return response;
    }

    public async Task<IWebRequestReponse> LoginUser(string email, string password)
    {
        string data = JsonUtility.ToJson(new User(email, password));
        IWebRequestReponse response = await webClient.SendPostRequest("/account/login", data);

        if (response is WebRequestData<string> dataResponse)
        {
            Token token = JsonUtility.FromJson<Token>(dataResponse.Data);
            webClient.SetToken(token.accessToken);
        }

        return response;
    }
}
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


}
using UnityEngine;

public class ApiClientManager
{
    private static ApiClientManager instance;
    private WebClient webClient;
    private string token;

    public static ApiClientManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ApiClientManager();
            }
            return instance;
        }
    }

    private ApiClientManager()
    {
        webClient = new WebClient();
    }

    public void SetToken(string newToken)
    {
        token = newToken;
        webClient.SetToken(token);
    }

    public string GetToken()
    {
        return token;
    }

    public void ClearToken()
    {
        token = null;
        webClient.SetToken(null);
    }

    public WebClient GetClient()
    {
        return webClient;
    }
} 
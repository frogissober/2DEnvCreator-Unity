using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EnvironmentService
{
    private WebClient webClient;

    public EnvironmentService()
    {
        webClient = new WebClient();
    }

    public async Task<IEnumerable<Environment>> GetEnvironments()
    {
        var response = await webClient.SendGetRequest("/Environment");
        if (response is WebRequestData<string> dataResponse)
        {
            return JsonUtility.FromJson<List<Environment>>(dataResponse.Data);
        }
        return null;
    }

    public async Task<Environment> GetEnvironment(int id)
    {
        var response = await webClient.SendGetRequest($"/Environment/{id}");
        if (response is WebRequestData<string> dataResponse)
        {
            return JsonUtility.FromJson<Environment>(dataResponse.Data);
        }
        return null;
    }

    public async Task<Environment> CreateEnvironment(Environment environment)
    {
        string data = JsonUtility.ToJson(environment);
        var response = await webClient.SendPostRequest("/Environment", data);
        if (response is WebRequestData<string> dataResponse)
        {
            return JsonUtility.FromJson<Environment>(dataResponse.Data);
        }
        return null;
    }

    public async Task<bool> DeleteEnvironment(int id)
    {
        var response = await webClient.SendDeleteRequest($"/Environment/{id}", null);
        return response != null;
    }
}

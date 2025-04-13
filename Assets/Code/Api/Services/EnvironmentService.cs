using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class EnvironmentService
{
    private readonly WebClient webClient;

    public EnvironmentService() => webClient = ApiClientManager.Instance.GetClient();

    public async Task<IEnumerable<Environment>> GetEnvironments()
    {
        var response = await webClient.SendGetRequest("/Environment");
        
        if (response is WebRequestData<string> data)
        {
            try
            {
                return JsonHelper.ParseJsonArray<Environment>(data.Data) ?? new List<Environment>();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse environments: {e.Message}");
            }
        }
        return new List<Environment>();
    }

    public async Task<Environment> GetEnvironment(int id)
    {
        var response = await webClient.SendGetRequest($"/Environment/{id}");
        return response is WebRequestData<string> data ? JsonUtility.FromJson<Environment>(data.Data) : null;
    }

    public async Task<(Environment environment, string error)> CreateEnvironment(Environment environment)
    {
        if (string.IsNullOrEmpty(environment.name))
            return (null, "Name is required");

        var response = await webClient.SendPostRequest("/Environment", JsonUtility.ToJson(environment));
        
        if (response is WebRequestData<string> data)
        {
            try
            {
                return (JsonUtility.FromJson<Environment>(data.Data), null);
            }
            catch
            {
                return (null, "Failed to parse server response");
            }
        }
        return (null, "Failed to create environment");
    }

    public async Task<bool> DeleteEnvironment(int id)
    {
        var response = await webClient.SendDeleteRequest($"/Environment/{id}", null);
        return response is WebRequestData<string>;
    }
}









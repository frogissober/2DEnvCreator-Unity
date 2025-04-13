using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class ObjectService
{
    private readonly WebClient webClient;

    public ObjectService() => webClient = ApiClientManager.Instance.GetClient();

    public async Task<IEnumerable<Object2D>> GetObjects(int environmentId)
    {
        var response = await webClient.SendGetRequest($"/Object2D/environment/{environmentId}");
        
        if (response is WebRequestData<string> data)
        {
            try
            {
                return JsonHelper.ParseJsonArray<Object2D>(data.Data) ?? new List<Object2D>();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse objects: {e.Message}");
            }
        }
        return new List<Object2D>();
    }

    public async Task<Object2D> GetObject(int id)
    {
        var response = await webClient.SendGetRequest($"/Object2D/{id}");
        return response is WebRequestData<string> data ? JsonUtility.FromJson<Object2D>(data.Data) : null;
    }

    public async Task<(Object2D object2D, string error)> CreateObject(Object2D object2D)
    {
        var response = await webClient.SendPostRequest("/Object2D", JsonUtility.ToJson(object2D));
        
        if (response is WebRequestData<string> data)
        {
            try
            {
                return (JsonUtility.FromJson<Object2D>(data.Data), null);
            }
            catch
            {
                return (null, "Failed to parse server response");
            }
        }
        return (null, "Failed to create object");
    }

    public async Task<(Object2D object2D, string error)> UpdateObject(Object2D object2D)
    {
        var response = await webClient.SendPutRequest($"/Object2D/{object2D.id}", JsonUtility.ToJson(object2D));
        
        if (response is WebRequestData<string> data)
        {
            try
            {
                return (JsonUtility.FromJson<Object2D>(data.Data), null);
            }
            catch
            {
                return (null, "Failed to parse server response");
            }
        }
        return (null, "Failed to update object");
    }

    public async Task<bool> DeleteObject(int id)
    {
        var response = await webClient.SendDeleteRequest($"/Object2D/{id}", null);
        return response is WebRequestData<string>;
    }

    public async Task<bool> DeleteAllObjectsInEnvironment(int environmentId)
    {
        var response = await webClient.SendDeleteRequest($"/Object2D/environment/{environmentId}", null);
        return response is WebRequestData<string>;
    }
}
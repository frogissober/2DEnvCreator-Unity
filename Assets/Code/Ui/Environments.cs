using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Environments : MonoBehaviour
{
    public GameObject environmentPrefab; // Verander de naam naar environmentPrefab voor duidelijkheid
    public Transform environmentList;  // Dit moet je "Content" object van de Scroll View zijn

    private EnvironmentService environmentService;

    private async void Start()
    {
        environmentService = new EnvironmentService();

        // Get the environments from the EnvironmentService
        var environments = await GetEnvironments();

        // Add each environment to the list
        foreach (var environment in environments)
        {
            AddEnvironmentItem(environment.Name);
        }
    }

    private async Task<IEnumerable<Environment>> GetEnvironments()
    {
        return await environmentService.GetEnvironments();
    }

    void AddEnvironmentItem(string worldName)
    {
        // Controleer of prefab en lijst correct zijn ingesteld
        if (environmentPrefab == null || environmentList == null)
        {
            Debug.LogError("Prefab of environmentList niet ingesteld in de Inspector!");
            return;
        }

        // Instantieer de omgeving in de lijst
        GameObject item = Instantiate(environmentPrefab, environmentList);

        // Reset scale (soms worden objecten te groot of te klein gespawned)
        item.transform.localScale = Vector3.one;

        // Zoek het TMP_Text component en zet de wereldnaam
        TMP_Text textComponent = item.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = worldName;
        }
        else
        {
            Debug.LogError("TMP_Text component niet gevonden in de prefab!");
        }
    }
}

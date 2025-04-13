using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Environments : MonoBehaviour
{
    public GameObject environmentPrefab;
    public Transform environmentList;
    public string environmentEditorSceneName = "EnvironmentEditorScene";
    private EnvironmentService environmentService;

    private async void Start()
    {
        if (!environmentPrefab || !environmentList)
        {
            Debug.LogError("Environment prefab or list not set!");
            return;
        }

        environmentService = new EnvironmentService();
        foreach (var environment in await environmentService.GetEnvironments())
        {
            CreateEnvironmentButton(environment);
        }
    }

    void CreateEnvironmentButton(Environment environment)
    {
        var item = Instantiate(environmentPrefab, environmentList);
        item.GetComponentInChildren<TMP_Text>().text = environment.name;
        item.GetComponent<Button>().onClick.AddListener(() => OnEnvironmentSelected(environment.id));
    }

    async void OnEnvironmentSelected(int id)
    {
        PlayerPrefs.SetInt("SelectedEnvironmentId", id);
        SceneManager.LoadScene(environmentEditorSceneName);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class EditorEnvironment : MonoBehaviour
{
    public GameObject roomPrefab;
    public Camera mainCamera;
    public GameObject objects;
    public GameObject careyPrefab;
    public GameObject kobainPrefab;
    public GameObject presleyPrefab;
    public float cameraSpeed = 25f;
    public float spawnScale = 0.5f;

    private EnvironmentService environmentService;
    private ObjectService objectService;
    private Camera cam;
    private int currentEnvironmentId;
    private Dictionary<string, GameObject> prefabMap;
    private bool isInitialized = false;

    void Start()
    {
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        if (isInitialized) return;

        environmentService = new EnvironmentService();
        objectService = new ObjectService();
        cam = Camera.main;

        prefabMap = new Dictionary<string, GameObject>
        {
            { "carey_0", careyPrefab },
            { "kobain_0", kobainPrefab },
            { "presley_0", presleyPrefab }
        };

        currentEnvironmentId = PlayerPrefs.GetInt("SelectedEnvironmentId");

        try
        {
            var environment = await environmentService.GetEnvironment(currentEnvironmentId);
            if (this == null) return;

            if (environment != null)
            {
                CreateRoom(environment.width, environment.height);
                await LoadExistingObjects();
            }
        }
        catch (System.Exception e)
        {
        }

        isInitialized = true;
    }

    private async System.Threading.Tasks.Task LoadExistingObjects()
    {
        if (this == null) return;

        var existingObjects = await objectService.GetObjects(currentEnvironmentId);
        
        if (this == null) return;

        foreach (var obj in existingObjects)
        {
            if (this == null) return;

            string lookupName = obj.name.ToLower();
            
            if (prefabMap.TryGetValue(lookupName, out GameObject prefab))
            {
                try
                {
                    GameObject clone = Instantiate(prefab, new Vector3(obj.positionX, obj.positionY, 1), Quaternion.Euler(0, 0, obj.rotation));
                    if (clone != null)
                    {
                        clone.transform.localScale = new Vector3(obj.scaleX, obj.scaleY, 1);
                        if (objects != null)
                        {
                            clone.transform.SetParent(objects.transform);
                        }

                        var draggable = clone.GetComponent<Draggable>() ?? clone.AddComponent<Draggable>();
                        draggable.SetObjectId(obj.id);
                    }
                }
                catch (System.Exception e)
                {
                }
            }
        }
    }

    private void CreateRoom(int width, int height)
    {
        var room = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
        room.transform.localScale = new Vector3(width, height, 1);
        room.transform.position = new Vector3(0, 0, 0);
        room.transform.parent = transform;

        if (cam != null)
        {
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;
            Vector3 bottomLeftPosition = new Vector3(-halfWidth, -halfHeight, -10f);
            cam.transform.position = bottomLeftPosition;
            cam.orthographic = true;
            cam.orthographicSize = height / 2f;
        }
    }

    public void SpawnCarey()
    {
        SpawnPrefab(careyPrefab);
    }

    public void SpawnKobain()
    {
        SpawnPrefab(kobainPrefab);
    }

    public void SpawnPresley()
    {
        SpawnPrefab(presleyPrefab);
    }

    private void SpawnPrefab(GameObject prefab)
    {
        Vector3 spawnPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(cam.transform.position.z)));
        spawnPos.z = 1f;

        GameObject clone = Instantiate(prefab, spawnPos, Quaternion.identity);
        clone.transform.localScale = Vector3.one * spawnScale;
        clone.transform.SetParent(objects.transform);

        Vector3 fixedPos = clone.transform.position;
        fixedPos.z = 1f;
        clone.transform.position = fixedPos;

        if (clone.GetComponent<Collider2D>() == null)
        {
            clone.AddComponent<BoxCollider2D>();
        }

        var draggable = clone.GetComponent<Draggable>();
        if (draggable == null)
        {
            draggable = clone.AddComponent<Draggable>();
        }
    }

    void Update()
    {
        Vector3 cameraMovement = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
            cameraMovement.x = -1;
        if (Input.GetKey(KeyCode.RightArrow))
            cameraMovement.x = 1;
        if (Input.GetKey(KeyCode.UpArrow))
            cameraMovement.y = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            cameraMovement.y = -1;

        if (cam != null)
        {
            cam.transform.position += cameraMovement * cameraSpeed * Time.deltaTime;
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("EnvironmentsScene");
    }

    public async void DeleteEnvironment()
    {
        try
        {
            await objectService.DeleteAllObjectsInEnvironment(currentEnvironmentId);
            
            if (await environmentService.DeleteEnvironment(currentEnvironmentId))
            {
                SceneManager.LoadScene("EnvironmentsScene");
            }
        }
        catch (System.Exception e)
        {
        }
    }
}

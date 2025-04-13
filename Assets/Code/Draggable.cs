using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private ObjectService objectService;
    private bool isDragging = false;
    private int objectId = 0;
    private bool isInitialized = false;
    private bool isNewObject = true;  // Flag to determine if this is a new spawn or loaded from DB

    void Start()
    {
        Initialize();
        // Only start dragging if this is a new object
        if (isNewObject)
        {
            StartDragging();
        }
    }

    private void Initialize()
    {
        if (isInitialized) return;

        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectService = new ObjectService();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        
        isInitialized = true;
    }

    public void SetObjectId(int id)
    {
        objectId = id;
        isNewObject = false;  // If we're setting an ID, this is a loaded object
        Debug.Log($"[Draggable] Object ID set to: {id}");
    }

    void Update()
    {
        if (!isInitialized || cam == null) return;

        if (isDragging)
        {
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(cam.transform.position.z)));
            mouseWorldPos.z = 0f;
            transform.position = mouseWorldPos + offset;
        }
    }

    public void StartDragging()
    {
        if (!isInitialized || cam == null) return;

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(cam.transform.position.z)));
        mouseWorldPos.z = 0f;
        offset = transform.position - mouseWorldPos;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        }
        isDragging = true;
    }

    void OnMouseDown()
    {
        StartDragging();
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }

            SaveObjectPositionAsync();
        }
    }

    private async void SaveObjectPositionAsync()
    {
        if (!isInitialized) return;

        try
        {
            var selectedEnvironmentId = PlayerPrefs.GetInt("SelectedEnvironmentId");
            Debug.Log($"[Draggable] Saving object position for environment {selectedEnvironmentId}");
            
            var obj = new Object2D
            {
                id = objectId,
                name = gameObject.name.Replace("(Clone)", "").Trim(),
                positionX = (int)transform.position.x,
                positionY = (int)transform.position.y,
                rotation = transform.rotation.eulerAngles.z,
                scaleX = transform.localScale.x,
                scaleY = transform.localScale.y,
                environmentId = selectedEnvironmentId
            };

            Debug.Log($"[Draggable] Attempting to save object: {obj.name} at ({obj.positionX}, {obj.positionY})");
            if (this == null) return;

            var (savedObject, error) = await objectService.CreateObject(obj);
            
            if (this == null) return;

            if (savedObject != null)
            {
                objectId = savedObject.id;
                Debug.Log($"[Draggable] Object saved successfully with ID: {objectId}");
            }
            else if (error != null)
            {
                Debug.LogError($"[Draggable] Failed to save object position: {error}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Draggable] Error saving object position: {e.Message}");
        }
    }
}

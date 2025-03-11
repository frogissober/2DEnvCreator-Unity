using TMPro;
using UnityEngine;

public class CreateEnvironment
{
    public TMP_InputField nameInput;
    public TMP_InputField heightInput;
    public TMP_InputField widthInput;
    public TMP_Text errorText;

    private EnvironmentService environmentService;

    private void Start()
    {
        environmentService = new EnvironmentService();
    } 
    public async void OnCreate()
    {
        string name = nameInput.text;
        if (!int.TryParse(heightInput.text, out int height))
        {
            errorText.text = "Height must be a valid number.";
            return;
        }
        if (!int.TryParse(widthInput.text, out int width))
        {
            errorText.text = "Width must be a valid number.";
            return;
        }

        Environment environment = new Environment
        {
            Name = name,
            Height = height,
            Width = width
        };

        var createdEnvironment = await environmentService.CreateEnvironment(environment);

        if (createdEnvironment != null)
        {
            errorText.text = "Environment created successfully!";
            // Optionally, you can navigate to another scene or update the UI
        }
        else
        {
            errorText.text = "Failed to create environment.";
        }
    }
}

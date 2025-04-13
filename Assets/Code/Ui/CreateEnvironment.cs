using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateEnvironment : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField heightInput;
    public TMP_InputField widthInput;
    public TMP_Text errorText;
    private EnvironmentService environmentService;

    private void Start() => environmentService = new EnvironmentService();

    public async void OnCreate()
    {
        if (!ValidateInput(out var environment))
            return;

        var (created, error) = await environmentService.CreateEnvironment(environment);
        if (created != null)
        {
            SceneManager.LoadScene("EnvironmentsScene");
            return;
        }
        errorText.text = error;
    }

    private bool ValidateInput(out Environment environment)
    {
        environment = null;
        
        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            errorText.text = "Name is required";
            return false;
        }

        if (!int.TryParse(heightInput.text, out int height))
        {
            errorText.text = "Height must be a valid number";
            return false;
        }

        if (height < 10 || height > 100)
        {
            errorText.text = "Height must be between 10 and 100";
            return false;
        }

        if (!int.TryParse(widthInput.text, out int width))
        {
            errorText.text = "Width must be a valid number";
            return false;
        }

        if (width < 20 || width > 200)
        {
            errorText.text = "Width must be between 20 and 200";
            return false;
        }

        environment = new Environment { name = nameInput.text, height = height, width = width };
        return true;
    }
}

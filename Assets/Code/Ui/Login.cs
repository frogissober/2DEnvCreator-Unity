using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    private AuthService authService;
    private EnvironmentService environmentService;

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;

    private void Start()
    {
        authService = new AuthService();
        environmentService = new EnvironmentService();
    }

    public async void onLogin()
    {
        User user = new(emailInput.text, passwordInput.text);

        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            errorText.text = "Email and password cannot be empty.";
            return;
        }

        var loginResponse = await authService.LoginUser(user.Email, user.Password);

        if (loginResponse != null)
        {
            // Fetch the user's environments
            var environments = await environmentService.GetEnvironments();

            // Store the environments in the static class


            // Load the environments scene
            SceneManager.LoadScene("EnvironmentsScene");
        }
        else
        {
            errorText.text = "Login failed. Please check your credentials.";
        }
    }
}

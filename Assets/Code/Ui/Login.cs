using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    private AuthService authService;

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;

    private void Start()
    {
        authService = new AuthService();
    }

    public async void onLogin()
    {
        User user = new(emailInput.text, passwordInput.text);

        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            errorText.text = "Email and password cannot be empty.";
            return;
        }

        IWebRequestReponse loginResponse = await authService.LoginUser(user.Email, user.Password);

        switch (loginResponse)
        {
            case WebRequestData<string> dataResponse:
                errorText.text = "Login successful!";
                SceneManager.LoadScene("EnvironmentsScene");
                break;
            case WebRequestError errorResponse:
                errorText.text = errorResponse.ErrorMessage;
                break;
            default:
                throw new System.NotImplementedException("No implementation for webRequestResponse of class: " + loginResponse.GetType());

        }
    }
}

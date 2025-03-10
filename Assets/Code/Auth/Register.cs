using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    private AuthService authService;

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;

    private void Start()
    {
        authService = new AuthService();
    }

    public async void OnRegister()
    {
        User user = new(emailInput.text, passwordInput.text);

        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            errorText.text = "Email and Password are required";
            return;
        }

        if (!user.Email.Contains("@") || !user.Email.Contains("."))
        {
            errorText.text = "Email is not valid";
            return;
        }

        IWebRequestReponse response = await authService.RegisterUser(user.Email, user.Password);

        if (response is WebRequestData<string> dataResponse)
        {
            errorText.text = dataResponse.Data;
        }
        else if (response is WebRequestError errorResponse)
        {
            errorText.text = errorResponse.ErrorMessage;
        }
        else
        {
            throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }

}

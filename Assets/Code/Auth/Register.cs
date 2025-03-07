using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public void OnRegister()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email or Password is empty");
            return;
        }

        if (!email.Contains("@") || !email.Contains("."))
        {
            Debug.LogError("Email is not valid");
            return;
        }

        Debug.Log("Registering user with email: " + email + " and password: " + password);

    }

}

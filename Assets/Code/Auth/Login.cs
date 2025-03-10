using TMPro;
using UnityEngine;

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
}

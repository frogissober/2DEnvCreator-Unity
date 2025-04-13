using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void loadOpeningScene()
    {
        SceneManager.LoadScene("OpeningScene");
    }
    public void loadRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }

    public void loadLoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void loadEnvironmentScene()
    {
        SceneManager.LoadScene("EnvironmentsScene");
    }

    public void loadCreateEnvironmentScene()
    {
        SceneManager.LoadScene("CreateEnvironmentScene");
    }
}

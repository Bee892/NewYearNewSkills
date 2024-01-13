using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
   public void PlayButton ()
    {
        SceneManager.LoadScene("Main");
    }
    public void QuitButton ()
    {
        Application.Quit();
    }
    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }
    public void ReturnButton()
    {
        SceneManager.LoadScene("Menu");
    }
}

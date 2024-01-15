using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public GameObject[] gameobjects;
    public int index;
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
    public void nextButton()
    {
        if (index < 6)
        {
            gameobjects[index].SetActive(false);
            index++;
            gameobjects[index].SetActive(true);
        }
        else if (index == 6)
        {
            gameobjects[index].SetActive(false);
            index = 0;
            gameobjects[index].SetActive(true);
        }
    }
}

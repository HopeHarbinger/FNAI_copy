using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFunctions : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject MainMenu;

    public void Play() {
        
        MainMenu.SetActive(false);
        LoadingScreen.SetActive(true);

        Invoke("Restart", 15f);
    }
    public void Restart() {
        SceneManager.LoadScene("IkeaLayout");
    }

    public void Quit() {
        Debug.Log("Quitting the game");
        Application.Quit();
    }

    //private void Update() {
    //    if (Input )
    //}
}

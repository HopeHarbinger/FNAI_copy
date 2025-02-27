using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeToMenu : MonoBehaviour
{   
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            Invoke("Escapist", 2f);
        } else return;
    }
    public void Escapist() {
        if (Input.GetKey(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Continue() {
        Time.timeScale = 1;
        SceneManager.LoadScene("IkeaLayout");
    }
}

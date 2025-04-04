using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnFunc : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    private bool _isPaused = false;

    public void Pause() 
    {
        
        _isPaused = !_isPaused;
        _pauseMenu.gameObject.SetActive(_isPaused);

        if (_isPaused == true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        _pauseMenu.gameObject.SetActive(false);    
    }

    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

}

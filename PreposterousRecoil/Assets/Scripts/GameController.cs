using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject gameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHearts.OnPlayerDied += DisplayGameOverScreen;
        gameOverScreen.SetActive(false);
    }

    private void DisplayGameOverScreen() 
    {
    
    gameOverScreen.SetActive(true);
        Time.timeScale = 0;

    }

    public void Reset()
    {
        gameOverScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

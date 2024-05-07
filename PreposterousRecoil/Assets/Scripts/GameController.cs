using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject gameOverScreen;
    public PlayerHearts hearts;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHearts.OnPlayerDied += DisplayGameOverScreen;
        gameOverScreen.SetActive(false);
    }

    private void DisplayGameOverScreen() 
    {

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Debug.LogError("GameOverScreen is missing!");
        }

    }

    public void Reset()
    {
        gameOverScreen.SetActive(false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        hearts.Respawn();
        Time.timeScale = 1;
    }
}

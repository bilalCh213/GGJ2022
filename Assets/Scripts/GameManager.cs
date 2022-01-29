using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject baseA;
    public GameObject baseB;

    void Update()
    {
        if(baseB == null)
        {
            Start.firstText = "You win!";
            Start.secondText = "Click to play again";
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if(baseA == null)
        {
            Start.firstText = "Game Over";
            Start.secondText = "Click to play again";
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

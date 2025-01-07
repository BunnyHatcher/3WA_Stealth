using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOver;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
    }
}

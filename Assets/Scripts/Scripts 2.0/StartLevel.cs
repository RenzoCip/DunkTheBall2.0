using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    GameManager2 gameManager;
    public int levelActual;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager2>();
        gameManager.currentLevel = levelActual;

        gameManager.StartLevel();

        Debug.Log("el nivel debe comenzar");
    }
}

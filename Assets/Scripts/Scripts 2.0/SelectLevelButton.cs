using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLevelButton : MonoBehaviour
{
    public Button level1;
    public Button level2;
    public Button level3;
    public Button level4;
    public Button level5;
    public Button level6;

    private CameraController cameraController;
    private GameManager2 gameManager;
    private LevelManager levelMenagerSelectLevel;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager2>(); // Busca el GameManager en la escena

        GameObject mainCamera = GameObject.Find("Main Camera");
        cameraController = mainCamera.GetComponent<CameraController>();

        level1.onClick.AddListener(SelectLevel1);
        level2.onClick.AddListener(SelectLevel2);
        level3.onClick.AddListener(SelectLevel3);
        level4.onClick.AddListener(SelectLevel4); 
        level5.onClick.AddListener(SelectLevel5);
        level6.onClick.AddListener(SelectLevel6);
    }



    
    public void SelectLevel1()
    {
        gameManager.currentLevel = 0;
        cameraController.CameraToPlayLevel();
        SceneManager.LoadScene(2);

        

    }

    public void SelectLevel2()
    {
        gameManager.currentLevel = 1;
        cameraController.CameraToPlayLevel();
        SceneManager.LoadScene(3);

    } 
    public void SelectLevel3()
    {
        gameManager.currentLevel = 2;
        cameraController.CameraToPlayLevel();
        SceneManager.LoadScene(4);
  
    }

    public void SelectLevel4( )
    {
        gameManager.currentLevel = 3;
        cameraController.CameraToPlayLevel();
        SceneManager.LoadScene(5);
        
    }

    public void SelectLevel5( )
    {
        gameManager.currentLevel = 4;
        cameraController.CameraToPlayLevel();
        SceneManager.LoadScene(6);
        
    }

    public void SelectLevel6( )
    {
        gameManager.currentLevel = 6;
        cameraController.CameraToPlayLevel();
        SceneManager.LoadScene(7);
        
    }
}   

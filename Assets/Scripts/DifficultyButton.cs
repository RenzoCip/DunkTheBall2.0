using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class DifficultyButton : MonoBehaviour
{
    private Button button;
    private GameManager gameManager;

    public TextMeshProUGUI explicacionText;

    private MoveHorizontal moveHorizontal;

    public string dificultyLevel;
       
    public int attemptsLeftDifficulty;
    public int speedDifficulty;
    public int pointsToWinDifficulty;
    // Start is called before the first frame update
    void Start()
    {
        moveHorizontal = GameObject.Find("Canasta").GetComponent<MoveHorizontal>(); 
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        //explicacionText = GetComponent<TextMeshProUGUI>();
        SetExplanationText();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void SetDifficulty()
    {
        gameManager.attemptsLeft = attemptsLeftDifficulty;
        moveHorizontal.speed = speedDifficulty;
        gameManager.pointsToWin = pointsToWinDifficulty;
    }
   public  void SetExplanationText()
    {
        if (explicacionText != null)
        {
            explicacionText.text = dificultyLevel + 
                                    "\nAttemps: " + attemptsLeftDifficulty +
                                   "\nPoints to Win: " + pointsToWinDifficulty +
                                   "\nSpeed set: " + speedDifficulty;
        }

    }
 
}

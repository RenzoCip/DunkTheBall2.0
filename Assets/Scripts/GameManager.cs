
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Ball;
    public GameObject introGame;
    public DifficultyButton difficultyButton;

    public Camera mainCamera;

    public float distanceFromCamera = 10f;
    public float timeoutDuration = 10f;
    private float lastInstantiationTime = 0f;

    public int maxObjects = 10;
    public int attemptsLeft;
    public int points = 0;
    public int pointsToWin = 5;
    public int attemptsDifficulty;
    private AudioSource ballAudio;

    public AudioClip ballThrow;

    private List<GameObject> instantiateBalls = new List<GameObject>();

    public Counter counter;

    public TextMeshProUGUI ganasteText;
    public TextMeshProUGUI gameOverText;

    public Button restartButton;
    public Button menuButton;
  

    public bool gameIsActive;
    public bool canDetectClick;
    
    // Start is called before the first frame update
    void Start()
    {
        attemptsLeft = 10;
        //attemptsDifficulty = attemptsLeft;
        gameIsActive = true;
 
        ballAudio = GetComponent<AudioSource>();
        counter = FindObjectOfType<Counter>();
    }

    // Update is called once per frame
    void Update()
    {
        InstantiateBall();
        //DestroyOldObjectsIfNeeded();
        //CheckGameStatus();
    }
    void InstantiateBall()

    {   // Detecta el clic del mouse
        if (canDetectClick && Input.GetMouseButtonDown(0) && gameIsActive)
        {
            // Obtiene la posición del clic en la pantalla
            Vector3 mousePosition = Input.mousePosition;

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, distanceFromCamera));
            GameObject newBall = Instantiate(Ball, worldPosition, Quaternion.identity);
            instantiateBalls.Add(newBall);
            
            lastInstantiationTime = Time.time;

            ballAudio.PlayOneShot(ballThrow, 1f);

            attemptsLeft--;
            Debug.Log("te quedan" + attemptsLeft + "intentos");

            if (instantiateBalls.Count > maxObjects)
            {
                GameObject firstObject = instantiateBalls[0];
                instantiateBalls.RemoveAt(0);
                Destroy(firstObject);
            }
   
        }

    }

    void CheckGameStatus()
    {
        if (counter.Count >= pointsToWin)
        {
            ganasteText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            gameIsActive = false;
            Debug.Log("Ganaste");

        }
        else if ( attemptsLeft <= 0) 
        {
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            gameIsActive= false; 
            Debug.Log("perdiste");
        }
    }
    void DestroyOldObjectsIfNeeded()
    {
        if (Time.time - lastInstantiationTime > timeoutDuration)
        {
            foreach (GameObject obj in instantiateBalls)
            {
                Destroy(obj);

            }
            instantiateBalls.Clear();
        }
    }

    public void StartGame()
    {
        gameIsActive = true;
        canDetectClick = false;
        counter.Count = 0;
        introGame.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        counter.CounterText.gameObject.SetActive(true);
        introGame.gameObject.SetActive(false);
        StartCoroutine(EnableClickDetection());

    }
    private IEnumerator EnableClickDetection()
    {
        yield return new WaitForSeconds(0.2f);
        canDetectClick = true;
    }
    public void MenuGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartButton()
    {

        foreach (GameObject ball in instantiateBalls)
        {
            Destroy(ball); 
        }
        instantiateBalls.Clear();

        restartButton.gameObject.SetActive(false);
        ganasteText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        counter.Count = 0;

        counter.CounterText.text = "Points: " + counter.Count;

        attemptsLeft = attemptsDifficulty;
        
        StartGame();

      

    }
}

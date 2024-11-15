using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{

    public TextMeshProUGUI potenciaText;
    public TextMeshProUGUI score;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI levelCompleteTitleHUD;

    public Image clockTime;
    public Image potentialBar;
    public Image[] potentialPoints;
    public Image levelCompleteImage;
    public Image backgroundImage;

    public GameObject buttonsGameObject;
    public GameObject star1Full;
    public GameObject star2Full;
    public GameObject star3Full;
    public GameObject star1Empty;
    public GameObject star2Empty;
    public GameObject star3Empty;

    public GameObject levelCompleteHUD;
    public GameObject ballPrefab;
    public GameObject potentialBarGameObject;
    private GameObject newBall = null;

    private List<GameObject> instantiateBalls = new List<GameObject>();

    public Camera mainCamera;

    public GameObject buttonContinueHUD;
    public Button buttonRestartHUD;
    public Button buttonHomeHUD;
    public Button buttonPause;
    public Button buttonRestart;

    private RectTransform rectTransformButtonRestartHUD;
    private RectTransform rectTransformButtonHomeHUD;

    private LevelManager levelManager;

    private int prueba;
    public int playerPoints;
    public int pointsNeededTo01Star;
    public int pointsNeededTo02Star;
    public int pointsNeededTo03Star;
    public int pointsToWin;
    public int distanceFromCamera;
    public int maxObjects = 10;
    public int stars;

    public float topTimer;
    public float topTimerClock;
    public float timer;
    public float maxHoldTime = 2.0f; // Tiempo máximo para mantener presionado el botón
    public float maxForce = 10f; // Fuerza máxima aplicada a la bola
    private float buttonHoldStartTime = 0f;
    private float lastInstantiationTime = 0f;
    private float timeOutDuration = 10f;

    private bool isHolding = false;
    public bool gameIsActive;
    public bool showLevelComplete;


    public Quaternion rotationBall = Quaternion.Euler(0, 90, 0);

    public Counter counter;

    private PlayerProgress playerProgress;
    public int totalLevels = 6;
    public int currentLevel;
    public int currentsPoints;


    // Start is called before the first frame update
    void Start()
    {
        DynamicGI.UpdateEnvironment();
        topTimerClock = topTimer;
        pointsToWin = pointsNeededTo03Star;
        counter = FindObjectOfType<Counter>();
        potentialBarGameObject.SetActive(false);
        levelCompleteHUD.SetActive(false);
        showLevelComplete = true;
    }

    public void StartLevel()
    {
        gameIsActive = false;
        buttonPause.gameObject.SetActive(false); //descativa boton para el contador incial
        buttonRestart.gameObject.SetActive(false); //descativa boton para el contador incial

        StartCoroutine(StartCountdown());  // Iniciar el contador de 3, 2, 1

        playerProgress = SaveSystem.LoadProgress(totalLevels); //carga el prpogreso al inciar el nivel
        Debug.Log("Se Cargan Datos de guardado al iniciar el nivel ");

        rectTransformButtonRestartHUD = buttonRestartHUD.GetComponent<RectTransform>();
        rectTransformButtonHomeHUD = buttonHomeHUD.GetComponent<RectTransform>();




    }

    IEnumerator StartCountdown()
    {
        // Mostrar el contador
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();  // Mostrar el número
            yield return new WaitForSeconds(1f);  // Esperar un segundo
        }

        // Ocultar el contador después de que llegue a 0
        countdownText.gameObject.SetActive(false);

        // Iniciar el juego
        StartFirstGame();
        buttonPause.gameObject.SetActive(true);
        buttonRestart.gameObject.SetActive(true);
    }

    public void StartFirstGame()
    {
        gameIsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsActive)
        {
            playerPoints = counter.Count;
            currentsPoints = playerPoints;
        }
        HandleBallControl();
        UpdatePotenciaText();
        CheckGameStatus();
        ShowTime();
        DestroyOldObject();
    }

    public void HandleBallControl()
    // cuando el click empieza a ser presionado incia el tiempo que permanece presionado y deja como isHolding en true 
    // una vez se suelta el click se calcula la cantidad de tiempo que se mantiene presionado el click,
    // toma la poscion del del mouse respecto a la camara principal e instancia la bola en la posicion seleccionada
    // luego accede al script de ballshot para asingnar la cantidad de fuerza en el directionForce y disparar la bola con la fuerza seleccionada

    {
        if (!gameIsActive || IsPointerOverUI())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && gameIsActive)
        {
            // Inicia el tiempo cuando el botón es presionado revisa cuando empieza a ser presionado el click y activa la barra de potencia 
            buttonHoldStartTime = Time.time;
            potentialBarGameObject.SetActive(true);
            isHolding = true;

            // Instancia la bola y ajusta la fuerza
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, distanceFromCamera));
            newBall = Instantiate(ballPrefab, worldPosition, rotationBall);
            instantiateBalls.Add(newBall);

        }
        else if (Input.GetMouseButton(0) && isHolding && newBall != null)
        {
            // Si el botón sigue presionado, mueve la bola con el ratón
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, distanceFromCamera));
            newBall.transform.position = worldPosition;
        }

        else if (Input.GetMouseButtonUp(0) && isHolding && newBall != null)
        {
            if (!IsPointerOverUI())
            {
                // Calcula el tiempo que se mantuvo presionado el botón
                float holdDuration = Time.time - buttonHoldStartTime;
                float normalizedHoldTime = Mathf.Clamp01(holdDuration / maxHoldTime);

                Vector3 directionForce = new Vector3(6f, normalizedHoldTime * maxForce, 0f);

                // Configura la fuerza de la bola
                BallShoot ballShoot = newBall.GetComponent<BallShoot>();
                lastInstantiationTime = Time.time;

                if (instantiateBalls.Count > maxObjects)
                {
                    GameObject firstObject = instantiateBalls[0];
                    instantiateBalls.RemoveAt(0);
                    Destroy(firstObject);
                }

                if (ballShoot != null)
                {
                    //revisa que el click haya sido soltado, desactiva la barra de potencia y asigna la fuerza para la bola
                    ballShoot.directionForce = directionForce;
                    ballShoot.ApplyForce();
                    potentialBarGameObject.SetActive(false);
                    isHolding = false;
                    newBall = null;
                }
            }
            else
            {
                // Si el puntero está sobre UI al soltar, no suelta la bola, mantiene el control
                Debug.Log("El puntero estaba sobre UI, no suelto la bola");
            }
        }
    }

    //determina si un punto de potencia debe ser mostrado basado en la potencia actual y el índice del punto de potencia.
    bool DisplayPotentialPoints(float _potentia, int pointNumber)
    {
        //calcula el umbral de potencia requerido para mostrar un punto de potencia específico.
        return _potentia >= ((pointNumber + 1) * (maxForce / potentialPoints.Length // da la cantidad de potencia necesaria para que cada punto de potencia sea visible. Por ejemplo, si tienes 5 puntos de potencia y la fuerza máxima es 10, cada punto representa 2 unidades de fuerza.
                                                                                    ));
    }
    public void UpdatePotenciaText()
    {
        if (isHolding)
        {
            // Calcula el tiempo que se ha mantenido presionado el botón
            float holdDuration = Time.time - buttonHoldStartTime;
            float normalizedHoldTime = Mathf.Clamp01(holdDuration / maxHoldTime);
            float currentForce = normalizedHoldTime * maxForce;

            //Ajusta la cantidad de la barra de potencia(una Image en Unity) para reflejar visualmente la potencia actual. Si currentForce es 5 y maxForce es 10, la barra de potencia se llenará al 50 %
            //fillAmount es la propiedad de una Image en el inspector para hacerla visible parcialmente.
            potentialBar.fillAmount = currentForce / maxForce;
            //El bucle for recorre todos los puntos de potencia (potentialPoints) y usa la función DisplayPotentialPoints para determinar
            //si cada punto debe estar habilitado o deshabilitado.
            //Si DisplayPotentialPoints devuelve true, el punto de potencia se muestra; de lo contrario, se oculta.
            for (int i = 0; i < potentialPoints.Length; i++)
            {
                potentialPoints[i].enabled = DisplayPotentialPoints(currentForce, i);
            }

        }
    }

    void CheckGameStatus()
    {
        if (topTimer <= 0 && gameIsActive)
        {
            gameIsActive = false;
            EvaluatePlayerPerformance();
            topTimer = 0;
            
            EndLevel(currentLevel, currentsPoints, stars);
            Debug.Log("Nivel Completado, estado Guardado");
            Debug.Log("el nivel actual es el" + currentLevel);
        }
    }
    // evalúa cuántas estrellas ganó el jugador y muestra el HUD correcto
    public void EvaluatePlayerPerformance()
    {
        stars = CalculateStars(playerPoints);
        //Debug.Log(playerPoints);
        if (stars == 0)
        {
            Color colorLose;
            ColorUtility.TryParseHtmlString("#E34839", out colorLose);
            //Debug.Log("perdiste");
            levelCompleteTitleHUD.text = "Try Again.";
            levelCompleteImage.color = colorLose;
            backgroundImage.color = colorLose;
            score.text = "Score: " + counter.Count;
            rectTransformButtonRestartHUD.anchoredPosition = new Vector2(-70, -162.2f);
            rectTransformButtonHomeHUD.anchoredPosition = new Vector2(55, -162.2f);
            buttonPause.gameObject.SetActive(false);
            buttonRestart.gameObject.SetActive(false);
            buttonContinueHUD.SetActive(false);
            levelCompleteHUD.SetActive(true);
            star1Full.SetActive(false);
            star1Empty.SetActive(true);
            star2Full.SetActive(false);
            star2Empty.SetActive(true);
            star3Full.SetActive(false);
            star3Empty.SetActive(true);
        }
        else
        {
            rectTransformButtonRestartHUD.anchoredPosition = new Vector2(-70, -162.2f);
            rectTransformButtonHomeHUD.anchoredPosition = new Vector2(55, -162.2f);
            buttonPause.gameObject.SetActive(false);
            buttonRestart.gameObject.SetActive(false);
            buttonContinueHUD.SetActive(false);
            levelCompleteHUD.SetActive(true);
            ShowLevelCompleteHUD(stars);
        }
    }

    // calcula cuantas estrellas ganó el jugador
    public int CalculateStars(int playerPoints)
    {
        if (playerPoints >= pointsNeededTo03Star)
        {
            return 3;
        }
        else if (playerPoints >= pointsNeededTo02Star)
        {
            return 2;
        }
        else if (playerPoints >= pointsNeededTo01Star)
        {
            return 1;
        }
        // Si no alcanzó ningún umbral, el jugador pierde
        return 0;
    }

    // muestra el HUD correspondiente y el score basado en las estrellas obtenidas
    public void ShowLevelCompleteHUD(int stars)
    {
        score.text = "Score: " + counter.Count;
        levelCompleteHUD.SetActive(true);

        switch (stars)
        {
            case 1:
                Debug.Log("gana una estrella");
                star1Full.SetActive(true);
                star1Empty.SetActive(false);
                star2Full.SetActive(false);
                star2Empty.SetActive(true);
                star3Full.SetActive(false);
                star3Empty.SetActive(true);

                break;
            case 2:
                Debug.Log("gana dos estrellas");
                star1Full.SetActive(true);
                star1Empty.SetActive(false);
                star2Full.SetActive(true);
                star2Empty.SetActive(false);
                star3Full.SetActive(false);
                star3Empty.SetActive(true);
                break;
            case 3:
                Debug.Log("gana tres estrellas");
                star1Full.SetActive(true);
                star1Empty.SetActive(false);
                star2Full.SetActive(true);
                star2Empty.SetActive(false);
                star3Full.SetActive(true);
                star3Empty.SetActive(false);
                break;
        }

    }

    //un contador para el tiempo maximo para completar el nivel
    public float CounterTimer()
    {
        if (gameIsActive)
        {
            topTimer = topTimer - 1 * Time.deltaTime;

        }
        return topTimer;
        //return Mathf.Round(topTimer);
    }

    //muestra el tiempo en pantalla
    public void ShowTime()
    {
        clockTime.fillAmount = timer / topTimerClock;
        timer = CounterTimer();
        //timerText.text = "Time: " + timer;
        Color colorClock = Color.Lerp(Color.red, Color.green, timer / topTimerClock);
        clockTime.color = colorClock;

    }

    public void DestroyOldObject()
    {
        if (Time.time - lastInstantiationTime > timeOutDuration && isHolding == false)
        {
            foreach (GameObject obj in instantiateBalls)
            {
                Destroy(obj);

            }
            instantiateBalls.Clear();
        }
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void CompleteLevel(int levelNumber, int points, int stars)
    {
        // Actualizar el progreso del nivel
        playerProgress.UpdateLevelProgress(levelNumber, points, stars);

        SaveSystem.SaveProgress(playerProgress);
    }

    public void EndLevel(int levelIndex, int points, int stars)
    {
        Debug.Log("deberia guardar");
        Debug.Log("Puntos actuales durante el juego: " + currentsPoints);

        if (playerProgress == null)
    {
            Debug.LogError("playerProgress es null. No se ha cargado correctamente.");
            return; // Detener la ejecución si playerProgress es null
        }

        if (playerProgress.levels == null)
        {
            Debug.LogError("playerProgress.levels es null. Los niveles no se han cargado correctamente.");
            return;
        }
        Debug.Log("levelIndex: " + levelIndex + " - Tamaño de playerProgress.levels: " + playerProgress.levels.Count);
        if (levelIndex < 0 || levelIndex >= playerProgress.levels.Count)
        {
            Debug.LogError("Índice de nivel fuera de rango: " + levelIndex);
            return;
        }

        Debug.Log("Puntos actuales: " + points + ", Puntos guardados: " + playerProgress.levels[levelIndex].maxPoints);
        Debug.Log("Verificando progreso del nivel: " + levelIndex);


        if (points > playerProgress.levels[levelIndex].maxPoints || playerProgress.levels[levelIndex].maxPoints == 0)
        {
            Debug.Log("Cantidad de niveles en playerProgress.levels: " + playerProgress.levels.Count);
            playerProgress.UpdateLevelProgress(levelIndex, points, stars);
            SaveSystem.SaveProgress(playerProgress);
            Debug.Log("Nuevos Puntos Guardados: " + points);
        }
        else
        {
            Debug.Log("Los puntos guardados son mayores que los que hizo el jugador");
        }


    
}

    public void CerrarJuego()
    {
        Application.Quit();
    }
}

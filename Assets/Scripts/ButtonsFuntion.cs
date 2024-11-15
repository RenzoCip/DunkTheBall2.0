using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsFuntion : MonoBehaviour
{
    private GameManager2 gameManager2;
    private CameraController cameraController;

    private Vector3 originalRetryButtonPosition;
    private Vector3 originalContinueButtonPosition;
    private Vector3 originalHomeButtonPosition;
    private Vector3 originalNextButtonPosition;
    private Vector3 originalButtonPosition;
    private Vector2 originalBackgroundSize;
    private Vector2 originalLevelCompleteImagePosition;
    private Vector2 originalScorePosition;

    private Color originalBackgroundColor;
    private Color originalLevelCompleteImageColor;

    public Button retryButton;
    public Button continueButton;
    public Button nextButton;
    public Button homeButton;

    private PlayerProgress playerProgress;
    private int totalLevels = 6;




    private void Start()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        cameraController = mainCamera.GetComponent<CameraController>();
        gameManager2 = FindObjectOfType<GameManager2>();
        SaveOriginalStates();
    }
    
    public void StartGameButton()
    {
        // Cargar el progreso del jugador al inicio
        playerProgress = SaveSystem.LoadProgress(totalLevels);
        Debug.Log("Se Carga Progreso en Pantalla de incio");
        cameraController.CameraToSelectLevel();
        SceneManager.LoadScene(1);

    }

    private void SaveOriginalStates()
    {
        originalHomeButtonPosition = homeButton.GetComponent<RectTransform>().anchoredPosition;
        originalNextButtonPosition = nextButton.GetComponent<RectTransform>().anchoredPosition;
        originalContinueButtonPosition = continueButton.GetComponent<RectTransform>().anchoredPosition;
        originalRetryButtonPosition = retryButton.GetComponent<RectTransform>().anchoredPosition;
        // Guardar la posición original de los botones
        originalButtonPosition = gameManager2.buttonsGameObject.transform.position;

        // Guardar el tamaño original del fondo
        originalBackgroundSize = gameManager2.backgroundImage.rectTransform.sizeDelta;

        // Guardar la posición original de la imagen de nivel completo
        originalLevelCompleteImagePosition = gameManager2.levelCompleteImage.rectTransform.anchoredPosition;

        // Guardar la posición original del puntaje
        originalScorePosition = gameManager2.score.rectTransform.anchoredPosition;

        // Guardar el color original del fondo y la imagen de nivel completo
        originalBackgroundColor = gameManager2.backgroundImage.color;
        originalLevelCompleteImageColor = gameManager2.levelCompleteImage.color;
    }

    public void LoadLevelsMenu()
    {
        cameraController.CameraToSelectLevel();
        SceneManager.LoadScene(1);
    }

    public void RestartLevel()
    {
        gameManager2.topTimer = gameManager2.topTimerClock; // Restablece el temporizador a su valor inicial
        gameManager2.timer = gameManager2.topTimer; // Asegúrate de que `timer` esté sincronizado con `topTimer`
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recargar la escena actual
    }

    public void PauseMenu()
    {
        Color colorPause;
        ColorUtility.TryParseHtmlString("#00B8FF", out colorPause);

        gameManager2.levelCompleteImage.color = colorPause;
        gameManager2.backgroundImage.color = colorPause;

        RectTransform rtBackground = gameManager2.backgroundImage.rectTransform;
        rtBackground.sizeDelta = new Vector2(rtBackground.sizeDelta.x, 297f);

        RectTransform rtLevelCompleteImage = gameManager2.levelCompleteImage.rectTransform;
        rtLevelCompleteImage.anchoredPosition = new Vector2(rtLevelCompleteImage.anchoredPosition.x, 87);

        gameManager2.levelCompleteTitleHUD.text = "Pause";
        gameManager2.score.text = "Score: " + gameManager2.counter.Count;

        RectTransform rtScore = gameManager2.score.rectTransform;
        rtScore.anchoredPosition = new Vector2(rtScore.anchoredPosition.x, -7);

        homeButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-5, -95, 0);
        retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-89, -95, 0);
        continueButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(92, -95, 0);

        gameManager2.levelCompleteHUD.SetActive(true);
        gameManager2.star1Full.SetActive(false);
        gameManager2.star1Empty.SetActive(false);
        gameManager2.star2Full.SetActive(false);
        gameManager2.star2Empty.SetActive(false);
        gameManager2.star3Full.SetActive(false);
        gameManager2.star3Empty.SetActive(false);
        gameManager2.buttonRestart.gameObject.SetActive(false);
        gameManager2.buttonPause.gameObject.SetActive(false);
        gameManager2.gameIsActive = false;
    }

    public void ContinueButton()
    {
        // Restaurar los estados originales
        RectTransform rtBackground = gameManager2.backgroundImage.rectTransform;
        rtBackground.sizeDelta = originalBackgroundSize;

        RectTransform rtLevelCompleteImage = gameManager2.levelCompleteImage.rectTransform;
        rtLevelCompleteImage.anchoredPosition = originalLevelCompleteImagePosition;

        RectTransform rtScore = gameManager2.score.rectTransform;
        rtScore.anchoredPosition = originalScorePosition;

        gameManager2.backgroundImage.color = originalBackgroundColor;
        gameManager2.levelCompleteImage.color = originalLevelCompleteImageColor;


        gameManager2.buttonsGameObject.transform.position = originalButtonPosition;

        gameManager2.levelCompleteHUD.SetActive(false);

        gameManager2.buttonPause.gameObject.SetActive(true);
        gameManager2.buttonRestart.gameObject.SetActive(true);

        gameManager2.gameIsActive = true;

        homeButton.GetComponent<RectTransform>().anchoredPosition = originalHomeButtonPosition;
        retryButton.GetComponent<RectTransform>().anchoredPosition = originalRetryButtonPosition;
        continueButton.GetComponent<RectTransform>().anchoredPosition = originalContinueButtonPosition;


    }
}
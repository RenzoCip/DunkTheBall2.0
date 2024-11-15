using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Image[] levels; // Array para almacenar las imágenes de los niveles
    public List<LevelStars> stars; // Lista de objetos LevelStars, cada uno para un nivel

    public Color unlockedColor = Color.white; // El color que indica que el nivel está desbloqueado

    public GameObject selectLevel;

    void Start()
    {
       
        Invoke("ActiveSelectLevel", 0.7f);


    
}
    public void UpdateLevelSelectionUI()
    {
        PlayerProgress playerProgress = SaveSystem.LoadProgress(levels.Length);

        // Iterar sobre todos los niveles y actualizar la UI
        for (int i = 0; i < playerProgress.levels.Count; i++)
        {
            // Actualizar las estrellas del nivel
            Debug.Log("Actualizando estrellas para el nivel " + (i + 1) + ": " + playerProgress.levels[i].stars);
            CompleteStars(i, playerProgress.levels[i].stars);

            // Desbloquear el siguiente nivel si se ha obtenido al menos una estrella
            if (playerProgress.levels[i].stars > 0 && i + 1 < levels.Length)
            {
                Debug.Log("Desbloqueando el nivel " + (i + 2));  // Mostrar el nivel que se está desbloqueando
                UnlockAndEnableLevel(i + 1);
            }
        }
    }
    public void DisableLevel(int levelIndex)
    {
        Button button = levels[levelIndex].GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false; // Deshabilita el botón
            button.onClick.AddListener(() => ShowUnlockMessage());
        }
    }

    public void ShowUnlockMessage()
    {
        // Aquí puedes mostrar un mensaje en la UI
        Debug.Log("Consigue una estrella en el nivel anterior para desbloquear este nivel.");
    }


    void ActiveSelectLevel()
    {
        selectLevel.SetActive(true); // Activa el selectLevel después del retraso

        PlayerProgress playerProgress = SaveSystem.LoadProgress(levels.Length);

        if (!playerProgress.levels[0].isUnlocked)
        {
            playerProgress.levels[0].isUnlocked = true;
            SaveSystem.SaveProgress(playerProgress);
        }

        // Iterar sobre todos los niveles y actualizar la UI de niveles y estrellas
        for (int i = 0; i < levels.Length; i++)
        {
            if (playerProgress.levels[i].isUnlocked)
            {
                UnlockAndEnableLevel(i); // Desbloquear el nivel
            }
            else
            {
                DisableLevel(i); // Deshabilitar el nivel
            }

            // Mostrar las estrellas conseguidas en el nivel
            CompleteStars(i, playerProgress.levels[i].stars);
        }
    }

    public void UnlockAndEnableLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Length)
        {
            // Cambiar el color de la imagen del nivel para indicar que está desbloqueado
            levels[levelIndex].color = unlockedColor;

            // Habilitar el botón del nivel para que sea clickeable
            Button button = levels[levelIndex].GetComponent<Button>(); // Obtén el componente Button de la imagen
            if (button != null)
            {
                button.interactable = true; // Habilita el botón
            }
            else
            {
                Debug.LogError("La imagen del nivel no tiene un componente Button.");
            }
        }
        else
        {
            Debug.LogError("Índice de nivel fuera de rango.");
        }
    }

    public void CompleteStars(int levelIndex, int starCount)
    {
        if (levelIndex >= 0 && levelIndex < stars.Count)
        {
            Debug.Log("Habilitando " + starCount + " estrellas para el nivel " + (levelIndex + 1));

            // Restablecer las estrellas antes de habilitarlas
            for (int i = 0; i < stars[levelIndex].stars.Length; i++)
            {
                if (stars[levelIndex].stars[i] != null)
                {
                    stars[levelIndex].stars[i].enabled = false;  // Deshabilitar todas las estrellas al inicio
                }
            }

            for (int i = 0; i < starCount; i++)
            {
                if (stars[levelIndex].stars[i] != null)
                {
                    stars[levelIndex].stars[i].enabled = true;
                    Debug.Log("Estrella " + (i + 1) + " habilitada para el nivel " + (levelIndex + 1));
                }
                else
                {
                    Debug.LogError("Falta una estrella en el nivel " + (levelIndex + 1));
                }
            }
        }
        else
        {
            Debug.LogError("Índice de nivel fuera de rango en CompleteStars: " + levelIndex);
        }
    }
 }



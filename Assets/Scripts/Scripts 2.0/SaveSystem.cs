using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static string savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
    public static void SaveProgress(PlayerProgress progress)
    {
        string json = JsonUtility.ToJson(progress); 
        File.WriteAllText(savePath, json);
        Debug.Log("Progreso Guardado en "+ savePath);
    }
    public static PlayerProgress LoadProgress(int totalLevels)
    {
        if (File.Exists(savePath))
        {
            // Si el archivo existe, lo leemos y lo retornamos
            string json =File.ReadAllText(savePath);
            PlayerProgress progress = JsonUtility.FromJson<PlayerProgress>(json);
            Debug.Log("Progreso Cargado en Player Progress LoadProgress");
            return progress;
        }
        else
        {
            // Si el archivo no existe, creamos un nuevo progreso
            PlayerProgress newProgress = new PlayerProgress(totalLevels);
            SaveProgress(newProgress); // Guardamos el nuevo progreso
            Debug.Log("No se encontró archivo guardado. Creando nuevo progreso.");
            return newProgress;
        }
    } 
}
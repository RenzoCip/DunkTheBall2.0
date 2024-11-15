using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShowSaveData : MonoBehaviour
{

    [ContextMenu("Show Save Data")]
    public void ShowSavedData()
    {
        string path = SaveSystem.savePath;
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            Debug.Log("Contenido del archivo: " + data);
        }
        else
        {
            Debug.Log("El archivo no existe: " + path);
        }
    }
}

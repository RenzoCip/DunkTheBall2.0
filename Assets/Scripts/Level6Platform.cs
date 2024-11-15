using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Level6Platform : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        ResetPlatformPoss();
    }

    public void ResetPlatformPoss()
    {
        if (transform.position.z >= 17)
            {
                Debug.Log("llego a 1");
            transform.position = new Vector3(868.96f, 5.584804f, -4);
                
            }
    }

}

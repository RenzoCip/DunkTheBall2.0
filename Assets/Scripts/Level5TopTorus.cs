using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5TopTorus : MonoBehaviour
{
    public bool torusTopIn;
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pasó TopTorus");
        torusTopIn = true;
        StartCoroutine(ResetTorusTopIn());
    }
    // Corrutina que espera 2 segundos antes de desactivar torusTopIn
    private IEnumerator ResetTorusTopIn()
    {
        yield return new WaitForSeconds(2);
        torusTopIn = false;
    }
}

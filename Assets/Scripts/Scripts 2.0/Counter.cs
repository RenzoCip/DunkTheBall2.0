using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public TextMeshProUGUI CounterText;

    public int Count = 0;

    private void Start()
    {
        Count = 0;
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject aroExternoFisico = GameObject.Find("Aro Externo No Fisico");
        Level5TopTorus torusTopScript = FindAnyObjectByType<Level5TopTorus>();

        if ( aroExternoFisico != null && torusTopScript !=null) 
        {
            if (torusTopScript.torusTopIn)
            {
                Count += 1;
                CounterText.text = " Points: " + Count;
                torusTopScript.torusTopIn = false;
            }

        }
        else
        {
            Count += 1;
            CounterText.text = "Points: " + Count;
        }

    }
    public void Reset()
    {
        Count = 0;
    }
}



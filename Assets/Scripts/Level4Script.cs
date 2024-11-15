using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Script : MonoBehaviour
{

    public GameObject canasta;
    public GameManager2 gameManager;

    private float tiempoAleatorio;
    private float contadorInvisible;
    public float duracionDesvanecimiento = 3f;
    public float pruebaTiempo;

    private bool canastaActiva = true;
    private bool estaDesvaneciendo = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerarNuevoIntervalo();
    }

    // Update is called once per frame
    void Update()
    {
        contadorInvisible = gameManager.topTimer;
        Debug.Log("el tiempo restante es " + contadorInvisible);

        if (contadorInvisible > 0 && !estaDesvaneciendo)
        {
            // Reducir el tiempo aleatorio con el tiempo
            tiempoAleatorio -= Time.deltaTime;

            // Cuando el tiempo aleatorio llega a 0, alternar el estado de la canasta
            if (tiempoAleatorio <= 0)
            {
                canastaActiva = !canastaActiva;
                estaDesvaneciendo = true;

                StartCoroutine(DesvanecerHijos(canastaActiva)); // Activa o desactiva según el nuevo estado
               
            }
        }

    }
    void CambiarTransparenciaMateriales(Material[] materiales, float alpha)
    {
        foreach (Material material in materiales)
        {
            if (material != null)
            {
                Color color = material.color;
                color.a = alpha; // Modifica el canal alpha para cambiar la transparencia
                material.color = color; // Aplica el nuevo color con la transparencia
            }
        }
    }
    Material[] ObtenerMaterialesHijos()
    {
        // Crea una lista dinámica para almacenar los materiales
        List<Material> materialesList = new List<Material>();

        // Recorre todos los objetos hijos de "canasta"
        foreach (Transform child in canasta.transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                // Añade todos los materiales del hijo a la lista
                foreach (Material mat in childRenderer.materials)
                {
                    if (!materialesList.Contains(mat))
                    {
                        materialesList.Add(mat);
                    }
                }
            }
        }
        return materialesList.ToArray();
    }



    void GenerarNuevoIntervalo()
    {
        tiempoAleatorio = Random.Range(1f, 4f); // Genera un número aleatorio entre 1 y 3 segundos
    }
    System.Collections.IEnumerator DesvanecerHijos(bool activar)
    {
        float tiempo = 0f;
        Material[] materialesHijos = ObtenerMaterialesHijos();
        // Cambiar la transparencia de los materiales con el tiempo
        while (tiempo < duracionDesvanecimiento)
        {
            tiempo += Time.deltaTime;
            float alpha = activar ? Mathf.Lerp(0f, 1f, tiempo / duracionDesvanecimiento) : Mathf.Lerp(1f, 0f, tiempo / duracionDesvanecimiento);
            CambiarTransparenciaMateriales(materialesHijos, alpha);
            yield return null; // Espera al siguiente frame
        }
        if (!activar)
        {
            yield return new WaitForSeconds(pruebaTiempo); // Espera el tiempo generado aleatoriamente
            canastaActiva = true; // Vuelve a hacer que la canasta sea visible

            // Inicia el desvanecimiento inverso para volver a hacer visibles los objetos hijos
            StartCoroutine(DesvanecerHijos(true));
        }

        estaDesvaneciendo = false; // El desvanecimiento ha terminado
        GenerarNuevoIntervalo(); // Generar nuevo intervalo para la próxima acción
    }
}

   
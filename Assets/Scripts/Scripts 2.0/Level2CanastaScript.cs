using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Level2CanastaScript : MonoBehaviour
{
    public int speed = 3;
    public int rotationSpeed;
    public int rotationRange;

    public float limit = 5;

    private GameManager2 gameManager;

    private bool rotationForward = true;
    private bool movingRight = true;

    public Button retryButton;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager2>();
        movingRight = true;
        rotationForward = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameIsActive)
        {
            MoveHorizontal();
            Rotate();
        }

        
    }

    public void Rotate()
    {
        if (rotationForward )
        {
            RotateOnAxisForward();
        }
        else
        {
            RotateOnAxisBackward();
        }

    }

    public void MoveHorizontal()
    {
        if (movingRight )
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }
    public void RotateOnAxisForward()
    {

        // Rota hacia adelante
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Comprobamos si ha llegado al límite de 30 grados
        if (transform.eulerAngles.z >= rotationRange && transform.eulerAngles.z < 180)  // Evitamos la inversión de ángulos
        {
            rotationForward = false;
        }
    }
    public void RotateOnAxisBackward()

    {
        // Rota hacia atrás
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);

        // Comprobamos si ha llegado al límite de -30 grados
        if (transform.eulerAngles.z <= 360 - rotationRange && transform.eulerAngles.z > 180)  // Evitamos la inversión de ángulos
        {
            rotationForward = true;
        }
    }
    public void MoveRight()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (transform.position.z <= 2.7f)
        {
         
            movingRight = false;
        }

    }
    public void MoveLeft()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * -speed);
        if (transform.position.z >= 9)
        {
            movingRight = true;
        }
    }
}

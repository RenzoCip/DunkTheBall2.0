using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveHorizontal : MonoBehaviour
{
    public float speed = 3;
    public float limit = 5;

    private GameManager2 gameManager;
    private bool movingRight = true;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager2>();
        movingRight = true;

    }

    // Update is called once per frame
    void Update()
    {
       if (movingRight && gameManager.gameIsActive)
        {
            MoveRight();
        }
        if (!movingRight && gameManager.gameIsActive)
        {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
     transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (transform.position.z <=2.7f)
        {
           
            movingRight = false;
        }

    }
    private void MoveLeft()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * -speed);
        if (transform.position.z >= 9)
        {
            movingRight = true;
        }
    }
}

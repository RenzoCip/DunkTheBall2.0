using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTorusExtern : MonoBehaviour
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
        if (transform.position.z <= 4.5f)
        {
            Debug.Log("llego a 1 el torus");
            movingRight = false;
        }

    }
    private void MoveLeft()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * -speed);
        if (transform.position.z >= 11)
        {
            movingRight = true;
        }
    }
}

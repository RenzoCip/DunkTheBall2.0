using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class BallShoot : MonoBehaviour
{
    public bool ballOnGround= false;

    public AudioClip ballBounce;
    public AudioClip ballAroBounce;
    public AudioClip ballThrow;
    private AudioSource ballAudioSource;

    private Rigidbody ballRb;
    public Vector3 directionForce = new Vector3 (0f, 0f, 0f);
    public Vector3 directionTorque = new Vector3();

    public int randomDirectionTorqueX;
    public int randomDirectionTorqueY;
    public int randomDirectionTorqueZ;
    void Start()
    {
        ballAudioSource = GetComponent<AudioSource>();

        randomDirectionTorqueX = Random.Range(-10, 10);
        randomDirectionTorqueY = Random.Range(-10, 10);
        randomDirectionTorqueZ = Random.Range(-10, 10);
        directionTorque = new Vector3(randomDirectionTorqueX, randomDirectionTorqueY, randomDirectionTorqueZ);

        ballRb = GetComponent<Rigidbody>();

    }


    public void ApplyForce()
    {
        ballRb.velocity = Vector3.zero; // Asegura que la bola no tenga velocidad previa
        ballRb.AddForce(directionForce, ForceMode.Impulse);
        ballRb.AddTorque(directionTorque, ForceMode.Impulse);
        ballAudioSource.PlayOneShot(ballThrow);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Aro") && !ballOnGround)
        {
            ballOnGround = true;
            ballAudioSource.PlayOneShot(ballAroBounce, 0.6f);
        }
        if (collision.gameObject.CompareTag("Ground")&&  !ballOnGround)
        {
            ballOnGround = true;
            ballAudioSource.PlayOneShot(ballBounce, 0.3f);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ballOnGround = false;
        }
        if ((collision.gameObject.CompareTag("Aro") && !ballOnGround))
        {
            ballOnGround = false;
            ;
        }
    }
}



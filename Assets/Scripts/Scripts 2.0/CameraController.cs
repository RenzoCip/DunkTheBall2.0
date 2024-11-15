using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 cameraPosSL = new Vector3(858.57f, 1.3f, -0.670002f);
    private Quaternion cameraRotSL = Quaternion.Euler(-10, 61, 0);
    private Vector3 cameraPosPL = new Vector3(857.4877f, 5.479996f, 6.397193f);
    private Quaternion cameraRotPL = Quaternion.Euler(10, -270, 0);

    public float transitionDuration;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CameraToSelectLevel()
    {
        StartCoroutine(MoveAndRotateCamera(cameraPosSL, cameraRotSL));
    }
    public void CameraToPlayLevel()
    {
        StartCoroutine(MoveAndRotateCamera(cameraPosPL, cameraRotPL));
    }

IEnumerator MoveAndRotateCamera(Vector3 newPosition,Quaternion newRotation)
    {
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        float elapsedTime = 0;
        while (elapsedTime < transitionDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, newPosition, elapsedTime / transitionDuration);
            transform.rotation = Quaternion.Lerp(originalRotation, newRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Asegurarse de que la cámara termine exactamente en la nueva posición y rotación
        transform.position = newPosition;
        transform.rotation = newRotation;
    }


}
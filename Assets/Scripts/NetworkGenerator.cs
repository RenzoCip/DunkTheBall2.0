using UnityEngine;

public class NetworkGenerator : MonoBehaviour
{
    public GameObject segmentPrefab; // Prefab del segmento de la red (cylinder)
    public int segmentCount = 60;    // Número de segmentos en la red
    public float outerRadius = 1f;   // Radio exterior de la red (borde del aro)
    public float innerRadius = 0.5f; // Radio interior de la red (donde comienza a caer)

    void Start()
    {
        GenerateBasketballNet();
    }

    void GenerateBasketballNet()
    {
        float angleStep = 360f / segmentCount;
        for (int i = 0; i < segmentCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float xOuter = Mathf.Cos(angle) * outerRadius;
            float zOuter = Mathf.Sin(angle) * outerRadius;

            // Create outer segment
            Vector3 outerPosition = new Vector3(xOuter, 0, zOuter);
            InstantiateSegment(outerPosition, angle);

            // Create inner segments
            float xInner = Mathf.Cos(angle) * innerRadius;
            float zInner = Mathf.Sin(angle) * innerRadius;

            Vector3 innerPosition = new Vector3(xInner, 0, zInner);
            InstantiateSegment(innerPosition, angle + Mathf.PI / segmentCount); // Offset to align with outer segment
        }
    }

    void InstantiateSegment(Vector3 position, float angle)
    {
        // Instantiate the segment at the calculated position
        GameObject segment = Instantiate(segmentPrefab, position, Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0));
        segment.transform.SetParent(transform); // Optionally parent to this GameObject
    }
}
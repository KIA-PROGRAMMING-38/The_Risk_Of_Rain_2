using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSpawnLineRedererController : MonoBehaviour
{

    public float radius = 5f;
    
    public float radiusSizeSensitiviy;
    public int numSegments = 100; 

    public LineRenderer lineRenderer;
    public Transform _laserSpawnPosition;
    private void Awake()
    {
        lineRenderer.transform.position = transform.position;
    }
  
   
    private void Update()
    {
        lineRenderer.positionCount = numSegments + 1; 

        float angleStep = 360f / numSegments; 

        for (int i = 0; i <= numSegments; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius * radiusSizeSensitiviy; 
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius * radiusSizeSensitiviy; 

            Vector3 pos = new Vector3(x, y, 0f);
            lineRenderer.SetPosition(i, pos);
        }

        lineRenderer.loop = true; 
    }
}
  

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSkillSpawner : MonoBehaviour
{
   
    public GameObject _player;
    public GameObject _laserSpawnPosition;
    private LineRenderer lineRenderer;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

     

    }

    public float rangeRadius;
 

    void Update()
    {
        int layerMask = LayerMask.GetMask(LayerID.PLAYER);
        Collider[] colliders = Physics.OverlapSphere(transform.position, rangeRadius, layerMask);

        if (colliders.Length > 0)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, _laserSpawnPosition.transform.position);
            lineRenderer.SetPosition(1, _player.transform.position);
            Debug.Log("Golem's detected Player!");
            Debug.Log($"{colliders.Length}");
        }
        else
        {
            Debug.Log("there's no player within the range");
            lineRenderer.enabled = false;
        }
    }

    private void OnTriggerStay(Collider laserDetector)
    {
        if(isDetectingPlayer(laserDetector))
        {
           
        }
    }

    private void OnTriggerExit(Collider laserDetector)
    {
       
    }

    private bool isDetectingPlayer(Collider collision)
    {
        return (collision.CompareTag(TagID.PLAYER));
    }

}

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

   
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider laserDetector)
    {
        if(isDetectingPlayer(laserDetector))
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, _laserSpawnPosition.transform.position);
            lineRenderer.SetPosition(1, _player.transform.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void OnTriggerExit(Collider laserDetector)
    {
        lineRenderer.enabled = false;
    }

    private bool isDetectingPlayer(Collider collision)
    {
        return (collision.CompareTag(TagID.PLAYER));
    }

}

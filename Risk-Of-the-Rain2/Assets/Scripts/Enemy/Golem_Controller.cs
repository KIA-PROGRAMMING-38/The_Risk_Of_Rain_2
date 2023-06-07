using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem_Controller : MonoBehaviour
{
    public Renderer _renderer;

    public Material MaterialIdle;
    public Material MaterialOnDamaged;

    public NavMeshAgent agent;
    public Transform _playerTransform;
   

    void Update()
    {
        TracePlayer();
        TurnBackToNormal();
      
    }
    void SetPosition() => agent.Warp(_playerTransform.position);

    void TracePlayer()
    {
        agent.SetDestination(_playerTransform.position);
    }
    public float materialChangingSpeed;
    public float materialReversingSpeed;
    public float minBrightness;
    public float maxBrightness;
    public float currentBrightness;

    private bool onDamaged = false;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(TagID.COMMANDO_BASIC_ATTACK))
        {
            changeMaterial();
        }
      
    }

    private async UniTaskVoid changeMaterial()
    {
        onDamaged = true;
        if(currentBrightness < maxBrightness)
        {
            currentBrightness += 1.5f;
            MaterialOnDamaged.SetFloat("_Brightness", currentBrightness);
        }
        


        currentBrightness = Mathf.Lerp(currentBrightness, maxBrightness, Time.deltaTime * materialChangingSpeed);
        MaterialOnDamaged.SetFloat("_Brightness", currentBrightness);
       
        await UniTask.Delay(1000);
        onDamaged = false;

    }
    private async UniTask TurnBackToNormal()
    {
        if (onDamaged == false)
        {
            currentBrightness = Mathf.Lerp(currentBrightness, minBrightness, Time.deltaTime * materialReversingSpeed);
            MaterialOnDamaged.SetFloat("_Brightness", currentBrightness);
            
        }
       
    }
}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Controller : MonoBehaviour
{
    public Renderer _renderer;

    public Material MaterialIdle;
    public Material MaterialOnDamaged;

    void Start()
    {

    }


    void Update()
    {
        TurnBackToNormal();
    }
    public float materialChangingSpeed;
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
            currentBrightness += 2;
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
            currentBrightness = Mathf.Lerp(currentBrightness, minBrightness, Time.deltaTime * materialChangingSpeed);
            MaterialOnDamaged.SetFloat("_Brightness", currentBrightness);
            Debug.Log("SetBrightness Low");
        }
       
    }
}

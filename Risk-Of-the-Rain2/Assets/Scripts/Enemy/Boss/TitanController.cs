using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class TitanController : MonoBehaviour
{
    public Renderer _renderer;



    public Material golemMaterial;
    Material newMaterial; // use to have each golem objects get its own material to be controlled independently.

    public Transform _playerTransform;

    private Animator animator;
    private int hp;
    public bool isSpawned;
    public bool isDead;
    public bool isOnDamaged;
    public int HP
    {
        get { return hp; }
        set
        {
            if (value < 0)
            {
               
                if (isDead == false)
                {
                    isDead = true;
                }
            }
            else
            {
                hp = value;
            }
        }
    }


    private void Awake()
    {

        gameObject.SetActive(false);


        animator = GetComponent<Animator>();
    }

    private void Start()
    {
       
    }

    private void OnEnable()
    {
        currentShowingPart = minShowingPart;
        // newMaterial = new Material(golemMaterial);



        if (_renderer == null)
        {
            Debug.LogError("No Renderer found on this object or its children.");
            return;
        }


        hp = 20;

    }
    void Update()
    {
        TurnOnSpawningShader();

        if (GameManager.IsGameStarted == true)
        {
            TurnBackToNormal();
            if (isDead == false)
            {

                TurnOnSpawningShader();
            }
            else
            {
                OnDeath();
            }
        }


    }



    public float materialChangingSpeed;
    public float materialReversingSpeed;
    public float minBrightness;
    public float maxBrightness;
    public float currentBrightness;

    public bool onDamaged = false;



    public Vector3 hitPosition;
    public Vector3 UIOffest;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(TagID.COMMANDO_BASIC_ATTACK))
        {

            hitPosition = other.gameObject.transform.position + UIOffest;
            TurnOnHpAnimation();
            changeMaterial();
            HP -= 1;
        }

    }

    private async UniTaskVoid TurnOnHpAnimation()
    {
        if (onDamaged == false)
        {
            onDamaged = true;
            animator.SetBool(GolemAnimID.ON_DAMAGED, true);
            await UniTask.Delay(100);
            animator.SetBool(GolemAnimID.ON_DAMAGED, false);
            onDamaged = false;
        }
    }





    public float currentShowingPart;
    public float maxShowingPart;
    public float minShowingPart = -3;
    public float spawningSpeed;

    private async UniTaskVoid TurnOnSpawningShader()
    {
        if (isDead == false && currentShowingPart < maxShowingPart)
        {
            currentShowingPart += spawningSpeed * Time.deltaTime;
            _renderer.material.SetFloat(GolemShaderParamID.SHOWING_PART, currentShowingPart);
            isSpawned = false;
        }
        if(currentShowingPart > maxShowingPart -0.1f) isSpawned = true;


    }

    public float initialDamagedBrightness;
    private CancellationTokenSource _souce = new();
    private async UniTaskVoid changeMaterial()
    {
        onDamaged = true;
        if (currentBrightness < maxBrightness)
        {
            currentBrightness += initialDamagedBrightness;
            _renderer.material.SetFloat("_Brightness", currentBrightness);
        }


        currentBrightness = Mathf.Lerp(currentBrightness, maxBrightness, Time.deltaTime * materialChangingSpeed);
        _renderer.material.SetFloat("_Brightness", currentBrightness);

        await UniTask.Delay(90);
        onDamaged = false;

    }

    private async UniTask TurnBackToNormal()
    {
        if (onDamaged == false)
        {
            currentBrightness = Mathf.Lerp(currentBrightness, minBrightness, Time.deltaTime * materialReversingSpeed);
            _renderer.material.SetFloat("_Brightness", currentBrightness);
        }

    }

    public float vanishSpeed;
    private async UniTask OnDeath()
    {
        if (isDead == true && currentShowingPart > minShowingPart)
        {
            currentShowingPart -= vanishSpeed * Time.deltaTime;
            Debug.Log($"currentshowingPart: {currentShowingPart}");

            _renderer.material.SetFloat(GolemShaderParamID.SHOWING_PART, currentShowingPart);
        }

        await UniTask.Delay(1000);
        Deactivate();
    }


    void Deactivate() => gameObject.SetActive(false);



    private void OnDisable()
    {

    }

   
}

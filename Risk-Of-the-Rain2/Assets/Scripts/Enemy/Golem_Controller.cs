using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem_Controller : MonoBehaviour
{

 

    public Renderer _renderer;


    public Material MaterialOnSpawn;
    public Material MaterialOnDamaged;

    public NavMeshAgent agent;
    public Transform _playerTransform;

    private Animator animator;
    private int hp;
    private bool isDead;
    private bool isOnDamaged;
    public int HP
    {
        get { return hp; }
        set
        {
            if (value < 0)
            {
                TurnOnDead();
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
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        hp = 100;
      
    }
    void Update()
    {
        TurnOnSpawningShader();
        Debug.Log($"Golem Hp: {hp}");

        if (GameManager.IsGameStarted == true)
        {
            TurnBackToNormal();
            if (isDead == false)
            {
                TracePlayer();
            }
            else
            {
                OnDeath();
            }


        }


    }

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
            isOnDamaged = true;
            TurnOnHpAnimation();
            changeMaterial();
            HP -= 1;
        }

    }

    private async UniTaskVoid TurnOnHpAnimation()
    {

        animator.SetBool(GolemAnimID.ON_DAMAGED, true);
        await UniTask.Delay(500);
        animator.SetBool(GolemAnimID.ON_DAMAGED, false);
        isOnDamaged = false;

    }

    private async UniTaskVoid TurnOnDead()
    {
        animator.SetBool(GolemAnimID.DEAD, true);


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
            MaterialOnSpawn.SetFloat(GolemShaderParamID.SHOWING_PART, currentShowingPart);
        }

    }

    public float initialDamagedBrightness;
    private async UniTaskVoid changeMaterial()
    {
        onDamaged = true;
        if (currentBrightness < maxBrightness)
        {
            currentBrightness += initialDamagedBrightness;
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

    public float vanishSpeed;
    private async UniTask OnDeath()
    {
        if (isDead == true && currentShowingPart > minShowingPart)
        {
            currentShowingPart -= vanishSpeed * Time.deltaTime;
            Debug.Log($"currentshowingPart: {currentShowingPart}");

            MaterialOnDamaged.SetFloat(GolemShaderParamID.SHOWING_PART, currentShowingPart);
        }

        await UniTask.Delay(1000);
        Deactivate();



    }
    void Deactivate() => gameObject.SetActive(false);
}

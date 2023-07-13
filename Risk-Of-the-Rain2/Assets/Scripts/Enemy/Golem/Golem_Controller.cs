using Cysharp.Threading.Tasks;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Golem_Controller : MonoBehaviour
{
  
    private Animator animator;


    [Header("Material & Shader")]
    public Renderer _renderer;
    private Rigidbody rigidbody;
    public Material golemMaterial;

    public float materialChangingSpeed;
    public float materialReversingSpeed;
    public float minBrightness;
    public float maxBrightness;
    public float currentBrightness;


    public float currentShowingPart;
    public float maxShowingPart;
    public float minShowingPart = -3;
    public float spawningSpeed;
    public float initialDamagedBrightness;
    Material newMaterial; // use to have each golem objects get its own material to be controlled independently.
   

    [Space(10f)]
    [Header("Nav Agent Info")]
    public NavMeshAgent agent;
    public Transform _player;


    [Space(10f)]
    [Header("variables for animations")]
    public float stopDuration;
    private readonly float MILSEC_TO_SECOND = 1000;

  [Space(10f)]

    [Header("HP info")]
    public int hp;
    public int maxHp;
    private bool isDead;
    private bool isOnDamaged;
    public bool isFullySpawned;
    private readonly int MIL_TO_SEC = 1000;
    public float vanishSpeed;
    public float deactivateDelay;
    [Header("Slots")]
    [SerializeField]
    private GameObject _LaserController;

    [Header("UI")]
    public Vector3 hitPosition;
    public Vector3 UIOffest;

  
    public int HP
    {
        get { return hp; }
        set
        {
            if (value < 0)
            {
               

                if (isDead == false)
                {
                    OnDeath();
                    isDead = true;
                }
            }
            else
            {
                hp = value;
            }
        }
    }


    float originalAgentSpeed;
    private void Awake()
    {
        originalAgentSpeed = agent.speed;
        animator = GetComponent<Animator>();
    }



    private void OnEnable()
    {
        HP = maxHp;
        currentShowingPart = minShowingPart;
        newMaterial = new Material(golemMaterial);


        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer == null)
        {
            Debug.LogError("No Renderer found on this object or its children.");
            return;
        }
        _renderer.material = newMaterial;

      

    }
    void Update()
    {
        TurnOnSpawningShader();

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


        MoveAnimationOn();

    }
    public float agentSearchRadius = 10f;
    private async UniTaskVoid TracePlayer()
    {


        NavMeshHit hit;
        bool positionFound = NavMesh.SamplePosition(_player.position, out hit, agentSearchRadius, NavMesh.AllAreas);

        if (positionFound)
        {
            agent.SetDestination(hit.position);
        }



        if (agent.remainingDistance <= agent.stoppingDistance || agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid || !positionFound)
        {
            animator.SetBool(GolemAnimID.MOVING, false);
            agent.isStopped = true;


        }

        else
        {
            animator.SetBool(GolemAnimID.MOVING, true);
            agent.isStopped = false;
        }


    }



    public bool onDamaged = false;


 
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(TagID.COMMANDO_BASIC_ATTACK))
        {
            hitPosition = other.gameObject.transform.position + UIOffest;
            PlayDamagedAnimation();
            TurnOnHpAnimation();
            changeMaterial();
            HP -= 1;
        }



    }


    private async UniTask PlayDamagedAnimation()
    {
        animator.SetBool(GolemAnimID.ON_DAMAGED, true);
        agent.speed = 0;

        await UniTask.Delay((int)(stopDuration * MILSEC_TO_SECOND));

        if (isOnDamaged == false)
        {
            animator.SetBool(GolemAnimID.ON_DAMAGED, false);
            agent.speed = originalAgentSpeed;
        }
      


    }
    private async UniTaskVoid TurnOnHpAnimation()
    {
        if (isOnDamaged == false)
        {
            isOnDamaged = true;
          

            await UniTask.Delay(100);

            
            isOnDamaged = false;
        }


    }
    private void TurnOffCollider()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled= false;
        
    }
    private async UniTaskVoid PlayDeadAnim()
    {
        animator.SetBool(GolemAnimID.DEAD, isDead);
    }



    private async UniTaskVoid TurnOnSpawningShader()
    {
        if (isDead == false && currentShowingPart < maxShowingPart)
        {
            currentShowingPart += spawningSpeed * Time.deltaTime;
            _renderer.material.SetFloat(GolemShaderParamID.SHOWING_PART, currentShowingPart);
        }
        else
        {
            await UniTask.Delay(200);
            isFullySpawned = true;
        }

    }

  
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

        await UniTask.Delay(5);
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
   
    private async UniTask OnDeath()
    {
       
        if (isDead == true && currentShowingPart > minShowingPart)
        {
            currentShowingPart -= vanishSpeed * Time.deltaTime;
            _renderer.material.SetFloat(GolemShaderParamID.SHOWING_PART, currentShowingPart);
        }
       
        PlayDeadAnim();
        TurnOffCollider();
        _LaserController.SetActive(false);
        await UniTask.Delay((int)(MIL_TO_SEC * deactivateDelay));
        Deactivate();
       
       
    }

    private void MoveAnimationOn()
    {

        bool isMoviong = agent.remainingDistance <= agent.stoppingDistance;
        animator.SetBool(GolemAnimID.MOVING, isMoviong);
    



    }
    void Deactivate() => gameObject.SetActive(false);



    private void OnDisable()
    {
        CancelInvoke();
        ObjectPooler.ReturnToPool(gameObject);
    }
}

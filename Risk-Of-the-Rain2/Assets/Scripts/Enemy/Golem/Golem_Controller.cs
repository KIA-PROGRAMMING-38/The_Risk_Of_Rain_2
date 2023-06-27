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

    public Renderer _renderer;
    private Rigidbody rigidbody;

    public Material golemMaterial;
    Material newMaterial; // use to have each golem objects get its own material to be controlled independently.
    public NavMeshAgent agent;
    public Transform _player;

    private Animator animator;
    public int hp;
    public int maxHp;
    private bool isDead;
    private bool isOnDamaged;
    public bool isFullySpawned;

    [SerializeField]
    private GameObject _LaserController;
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
        if (isOnDamaged == false)
        {
            isOnDamaged = true;
            animator.SetBool(GolemAnimID.ON_DAMAGED, true);
            await UniTask.Delay(100);
            animator.SetBool(GolemAnimID.ON_DAMAGED, false);
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
        }
        else
        {
            await UniTask.Delay(200);
            isFullySpawned = true;
        }

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

    public float vanishSpeed;
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
        await UniTask.Delay(1000);
        Debug.Log("on Death");
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

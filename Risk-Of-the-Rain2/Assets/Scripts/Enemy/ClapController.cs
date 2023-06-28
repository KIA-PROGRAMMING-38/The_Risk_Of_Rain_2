using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class ClapController : MonoBehaviour
{
    [Header("Clap Skill Info")]
    [Space(5f)]

    [SerializeField]
    public static readonly int clapDamage = 40;
    [SerializeField]
    private float clapCooltime;
    private float elapsedTime;

    private bool isClapable;

    public float extraAngularSpeed; //rotate(agent) faster once detects player.
    private Animator _animator;
    private NavMeshAgent _agent;

    [Space(15f)]
    [Header("Collider")]
    [Space(5f)]

    [SerializeField]
    private GameObject _damageableCollider;

    [Space(15f)]
    [Header("Reference & Effect Details")]
    [Space(5f)]

    [SerializeField]
    private ParticleSystem _clapParticle;
    [SerializeField]
    private GameObject _lefthandEffect;
    [SerializeField]
    private GameObject _righthandEffect;

    public float _clapEffectWaitingTime;
    public float _damageableDuration;

   
    private void Awake()
    {
      
   

        _clapParticle.Stop();
        _lefthandEffect.SetActive(false);
        _righthandEffect.SetActive(false);
        _damageableCollider.SetActive(false);
    }
    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
        _agent = GetComponentInParent<NavMeshAgent>();
     
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(clapCooltime <  elapsedTime)
        {
            isClapable = true;
        }
        else
        {
            isClapable = false;
        }

    }

    private float ClapCoolTime = 5.0f;
    private bool isClapPlaying;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagID.PLAYER))
        {
            if (!isClapPlaying && isClapable)
            {
                StartClapAnimation();
            }
        }
    }

    private async UniTaskVoid StartClapAnimation()
    {
        elapsedTime = 0;

        isClapPlaying = true;
       

        _lefthandEffect.SetActive(true);
        _righthandEffect.SetActive(true);

        float originalAngularSpeed = _agent.angularSpeed;
        _agent.angularSpeed += extraAngularSpeed;

        _animator.SetTrigger(GolemAnimID.CLAP);
        _agent.isStopped = true;

        await UniTask.Delay((int)(_clapEffectWaitingTime * 1000));
      
        _agent.isStopped = false;
        _agent.angularSpeed = originalAngularSpeed;
        PlayClapParticle();

        _damageableCollider.SetActive(true);
        await UniTask.Delay((int)(_damageableDuration * 1000));
        _damageableCollider.SetActive(false);

        _lefthandEffect.SetActive(false);
        _righthandEffect.SetActive(false);

        isClapPlaying = false;
       
    }

    private void PlayClapParticle()
    {
        _clapParticle.Play();
    }

   
  
}

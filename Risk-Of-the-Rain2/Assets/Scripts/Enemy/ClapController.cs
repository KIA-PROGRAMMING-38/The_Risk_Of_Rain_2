using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class ClapController : MonoBehaviour
{

    private Animator _animator;
    private NavMeshAgent _agent;

    public float extraAngularSpeed;

    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
        _agent = GetComponentInParent<NavMeshAgent>();
    }

    private float ClapCoolTime = 5.0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagID.PLAYER))
        {
            StartClapAnimation();
        }
    }

    [SerializeField]
    private GameObject _clapParticle;
    [SerializeField]
    private GameObject _lefthandEffect;
    [SerializeField]
    private GameObject _righthandEffect;

    public float _clapEffectWaitingTime;
    public float _clapEffectDuration;
    

    private async UniTaskVoid StartClapAnimation()
    {
        _lefthandEffect.SetActive(true);
        _righthandEffect.SetActive(true);

        float originalAngularSpeed = _agent.angularSpeed;
        _agent.angularSpeed += extraAngularSpeed;

        _animator.SetTrigger(GolemAnimID.CLAP);
        _agent.isStopped = true;
        await UniTask.Delay(1000);
        _agent.isStopped = false;
        _agent.angularSpeed = originalAngularSpeed;
    }

    private async UniTaskVoid PlayClapParticle()
    {
        _clapParticle.SetActive(true);
        await UniTask.Delay((int)(_clapEffectDuration * 1000));

        _clapParticle.SetActive(false);
        _lefthandEffect.SetActive(false);
        _righthandEffect.SetActive(false);
    }
}

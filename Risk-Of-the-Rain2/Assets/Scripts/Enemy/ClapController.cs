using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
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


    private async UniTaskVoid StartClapAnimation()
    {
        float originalAngularSpeed = _agent.angularSpeed;
        _agent.angularSpeed += extraAngularSpeed;

        _animator.SetTrigger(GolemAnimID.CLAP);
        _agent.isStopped = true;
        await UniTask.Delay(1000);
        _agent.isStopped = false;
        _agent.angularSpeed = originalAngularSpeed;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;

public class RagDollController : MonoBehaviour
{
    private Rigidbody[] _ragdollRigidbodies;

    [SerializeField]
    private Transform _player;
   
   


    [Space(15f)]
    [Header("On Death")]
    [Space(5f)]
    public float dieKnockBackForce;


  
    void Awake()
    {
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
        gameObject.SetActive(false);
    }
    private void Start()
    {
       
    }

    
    private void OnEnable()
    {
        
        transform.position = _player.position;
        EnableRagdoll();
      

    }
    // Update is called once per frame
    void Update()
    {
   
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.Sleep();
        }
    }

    private void EnableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.WakeUp();
            rigidbody.AddForce((Vector3.back + Vector3.up) * dieKnockBackForce, ForceMode.Impulse);
        }



    }
}

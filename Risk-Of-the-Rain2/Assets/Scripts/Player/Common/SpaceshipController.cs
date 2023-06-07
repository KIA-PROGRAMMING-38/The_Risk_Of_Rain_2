using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UniRx;

public class SpaceshipController : MonoBehaviour
{
    public float speed;


    private Rigidbody rigidbody;

    [SerializeField]
    Transform startPosition;

    [SerializeField]
    GameObject arrivalExplosionPS;

    [SerializeField]
    GameObject RocketPluimingPS;

    [SerializeField]
    GameObject playerIntroPS;

    [SerializeField]
    GameObject _player;

   

    private Collider collider; 
    void Awake()
    {

       
        arrivalExplosionPS.SetActive(false);
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = (startPosition.position - transform.position).normalized * speed;
    }

    void Update()
    {

    }

    private void OnTriggerStay(Collider spaceship)
    {
        if (IsSpaceshipCollided(spaceship))
        {
            arrivalExplosionPS.SetActive(true);// explosion Particle
            RocketPluimingPS.SetActive(false);
            rigidbody.Sleep();

            collider.enabled = false;

        }

    }

    private bool IsSpaceshipCollided(Collider collision)
    {
        return (collision.CompareTag(TagID.TERRAIN));
    }
}

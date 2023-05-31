using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

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

    [SerializeField]
    Transform _playerVirtualCameraPosition;

    void Awake()
    {
        _player.SetActive(false);
        arrivalExplosionPS.SetActive(false);
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = (startPosition.position - transform.position).normalized * speed;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider spaceship)
    {
        if (IsSpaceshipCollided(spaceship))
        {
            arrivalExplosionPS.SetActive(true);// explosion Particle
            RocketPluimingPS.SetActive(false);
            rigidbody.Sleep();
            _player.SetActive(true);


            Cinemachine_Controller.virtualCamera.Follow = _playerVirtualCameraPosition;
            Cinemachine_Controller.virtualCamera.LookAt = _playerVirtualCameraPosition;
        }

    }

    private bool IsSpaceshipCollided(Collider collision)
    {
        return (collision.CompareTag(TagID.TERRAIN));
    }
}

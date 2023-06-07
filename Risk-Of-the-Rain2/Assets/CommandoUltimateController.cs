using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandoUltimateController : MonoBehaviour
{
    [SerializeField]
    Transform _ultimateSpawnPosition;

    //The player's position needs to be synchronized with the direction to ensure the movement of basic and special skills.
    [SerializeField]
    Transform _playerPosition;

    Rigidbody rigidbody;

    [SerializeField]
    ParticleSystem launchPS;
    [SerializeField]
    ParticleSystem collisionPS;

    public float _speed;
   
 
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    Vector3 launchDirection;
    private void OnEnable()
    {
        
        SetLaunchPosition();
        SetLaunchAnimation();
        launchDirection = RotateCommandoProjectile();
        LaunchProjectile(launchDirection);
    }

    private void Update()
    {

    }
  

    private void OnTriggerEnter(Collider other)
    {
        if (IsBulletCollided(other))
        {
            Play1ollisionParticleOnce();
        }
    }

    private async UniTaskVoid Play1ollisionParticleOnce()
    {
       
        rigidbody.Sleep();
        await UniTask.Delay(200);
        await UniTask.Delay(100);
        Deactivate();


    }


    private bool IsBulletCollided(Collider collision)
    {
        return (collision.CompareTag(TagID.TERRAIN) || collision.CompareTag(TagID.ENEMY));
    }

    private void OnDisable()
    {
        CancelInvoke();
        ObjectPooler.ReturnToPool(gameObject);
    }



    private void SetLaunchAnimation()
    {
     
    }

    private void SetLaunchPosition()
    {
        transform.position = _ultimateSpawnPosition.position;
    }


    /// <summary>
    /// This method rotates the camera using a Raycast. 
    /// The direction of the Raycast (where it is shot) is also calculated within this function.
    /// It is important to note that the direction should be in sync with the position of the crosshair for correct orientation.
    /// </summary>
    /// <returns></returns>
    [SerializeField]
    Transform _virtualCameraPosition;
    public Vector3 offsetX;
    private Vector3 RotateCommandoProjectile()
    {
        if (Physics.Raycast((Camera.main.transform.position + offsetX),
           10000 * _virtualCameraPosition.forward, out RaycastHit hitInfo))
        {
            Vector3 direction = hitInfo.point - _ultimateSpawnPosition.position;
            // Vector3 rotationQuantity= new Vector3(90f + direction.y, _virtualCameraPosition.position.x, 0);
            return direction.normalized;
        }

        else
        {
            Debug.Log("ERROR: Ray's hit nothing ");
            Vector3 direction = _virtualCameraPosition.forward;
            return _virtualCameraPosition.forward;
        }
    }

    private void LaunchProjectile(Vector3 direction)
    {
        rigidbody.velocity = direction * _speed;
        Invoke(nameof(Deactivate), 5f);
    }

    void Deactivate() => gameObject.SetActive(false);
}

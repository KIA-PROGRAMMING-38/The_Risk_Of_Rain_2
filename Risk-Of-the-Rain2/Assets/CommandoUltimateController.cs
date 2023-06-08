using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandoUltimateController : MonoBehaviour
{
    [SerializeField]
    Transform _ultimateSpawnPosition;

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
        launchDirection = SetDirectionToTarget();
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


   private async UniTaskVoid CheckPosition()
    {
        UniTask.Delay(50);
        Debug.Log($"current position: {transform.position}");
        UniTask.Delay(150);
    }


    [SerializeField]
    GameObject particleSystemOnCollision;
    private async UniTaskVoid Play1ollisionParticleOnce()
    {
        rigidbody.Sleep();
        particleSystemOnCollision.SetActive(true);
        await UniTask.Delay(200);
        rigidbody.Sleep();
        particleSystemOnCollision.SetActive(false);
        Deactivate();



    }


    private bool IsBulletCollided(Collider collision)
    {
        return (collision.CompareTag(TagID.TERRAIN) || collision.CompareTag(TagID.ENEMY));
    }

    private void OnDisable()
    {
        CancelInvoke();
        transform.position = _ultimateSpawnPosition.position;
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
    [SerializeField]
    Transform _virtualCameraChildren;

    public float maxDistance;


    private Vector3 SetDirectionToTarget()
    {
        return _virtualCameraPosition.forward;
    }
    //private Vector3 RotateCommandoProjectile()
    //{

    //    int enemyMask = LayerMask.GetMask(LayerID.ENEMY);
    //    int terrainMask = LayerMask.GetMask(LayerID.ENEMY);
    //    if (Physics.Raycast((Camera.main.transform.position),
    //       10000 * _virtualCameraChildren.forward, out RaycastHit hitInfo , maxDistance, terrainMask))
    //    {
    //        Vector3 direction = hitInfo.point - _ultimateSpawnPosition.position;
    //        // Vector3 rotationQuantity= new Vector3(90f + direction.y, _virtualCameraPosition.position.x, 0);
    //        return direction.normalized;
    //    }

    //    else
    //    {
    //        Debug.Log("ERROR: Ray's hit nothing ");
    //        //Vector3 direction = _virtualCameraPosition.forward;
    //        return _virtualCameraPosition.forward;
    //    }
    //}

    private void LaunchProjectile(Vector3 direction)
    {
        rigidbody.velocity = direction * _speed;
        Invoke(nameof(Deactivate), 3f);
    }

    void Deactivate() => gameObject.SetActive(false);
}

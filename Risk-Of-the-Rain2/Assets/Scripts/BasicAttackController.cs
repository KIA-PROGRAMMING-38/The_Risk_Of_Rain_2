using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;



public class BasicAttackController : MonoBehaviour
{
    [SerializeField]
    Transform _spawnPositionLeft;
    [SerializeField]
    Transform _spawnPositionRight;

    //The player's position needs to be synchronized with the direction to ensure the movement of basic and special skills.
    [SerializeField]
    Transform _playerPosition;

    Rigidbody rigidbody;

    public float _bulletSpeed;
    Vector3 setRotation;
    Transform[] spawnTransforms;
    private void Awake()
    {
        spawnTransforms = new Transform[2] { _spawnPositionLeft, _spawnPositionRight };
        rigidbody = GetComponent<Rigidbody>();
    }


    Vector3 launchDirection;
    private void OnEnable()
    {
        particleSystemOnCollision.SetActive(false);
        SetLaunchPosition();
        SetLaunchAnimation();
        launchDirection = RotateCommandoProjectile();
        LaunchProjectile(launchDirection);
    }

    [SerializeField]
    GameObject particleSystemOnCollision;

    private void OnTriggerEnter(Collider other)
    {
        if (IsBulletCollided(other))
        {
            Play1ollisionParticleOnce();
        }
    }

    private async UniTaskVoid Play1ollisionParticleOnce()
    {
        particleSystemOnCollision.SetActive(true);
        rigidbody.Sleep();
        await UniTask.Delay(200);
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
        ObjectPooler.ReturnToPool(gameObject);
    }



    [SerializeField]
    ParticleSystem particleSystemOnEnableLeft;
    [SerializeField]
    ParticleSystem particleSystemOnEnableRight;
    private void SetLaunchAnimation()
    {
        if (Commando_Skill_Spawner.launchOrder % 2 == 0)
        {
            particleSystemOnEnableLeft.Play();
        }

        else
        {
            particleSystemOnEnableRight.Play();
        }
    }

    private void SetLaunchPosition()
    {
        transform.position = spawnTransforms[Commando_Skill_Spawner.launchOrder % 2].position;
    }


    /// <summary>
    /// This method rotates the camera using a Raycast. 
    /// The direction of the Raycast (where it is shot) is also calculated within this function.
    /// It is important to note that the direction should be in sync with the position of the crosshair for correct orientation.
    /// </summary>
    /// <returns></returns>
    [SerializeField]
    Transform _virtualCameraPosition;
    private Vector3 RotateCommandoProjectile()
    {
        if (Physics.Raycast(Camera.main.transform.position,
           10000 * _virtualCameraPosition.forward, out RaycastHit hitInfo))
        {
            Vector3 direction = hitInfo.point - spawnTransforms[Commando_Skill_Spawner.launchOrder % 2].position;
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
        rigidbody.velocity = direction * _bulletSpeed;
        Invoke(nameof(Deactivate), 3f);
    }

    void Deactivate() => gameObject.SetActive(false);
}

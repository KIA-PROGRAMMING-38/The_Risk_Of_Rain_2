using Unity.VisualScripting;
using UnityEngine;



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

    private void Update()
    {

    }

    [SerializeField]
    ParticleSystem particleSystemOnEnableLeft;
    [SerializeField]
    ParticleSystem particleSystemOnEnableRight;

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

    private void OnTriggerStay(Collider other)
    {
        if (IsBulletCollided(other))
        {
            particleSystemOnCollision.SetActive(true);
            rigidbody.Sleep(); // Stop the basic skill immediately.
        }
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
    /// Get virtual camera's fowards vector to have the commando skills launch towards crosshair position.
    /// Y offset is also used since the crosshair is moved to upward a bit.
    /// </summary>
    /// 

    [SerializeField]
    Transform _virtualCameraPosition;




    public float launchPositionYOffset;
    private Vector3 RotateCommandoProjectile()
    {
        _virtualCameraPosition.position += Vector3.up * launchPositionYOffset;

        if (Physics.Raycast(Camera.main.transform.position,
           10000 * _virtualCameraPosition.forward, out RaycastHit hitInfo))
        {
            Debug.Log("Ray's hit something");
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
        Invoke(nameof(Deactivate), 10f);
    }

    void Deactivate() => gameObject.SetActive(false);
}

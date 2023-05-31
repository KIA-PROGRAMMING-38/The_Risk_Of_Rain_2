using Unity.VisualScripting;
using UnityEngine;



public class BasicAttackController : MonoBehaviour
{
    [SerializeField]
    Transform _spawnPositionLeft;
    [SerializeField]
    Transform _spawnPositionRight;


    //Get virtual camera position to rotate vertical screen.
    [SerializeField]
    Transform _virtualCameraPosition;


    //The player's position needs to be synchronized with the direction to ensure the movement of basic and special skills.
    [SerializeField]
    Transform _playerPosition;


    Rigidbody rigidbody;
    public float _bulletSpeed;
    Vector3 currentPlayerPosition;
    Transform[] spawnTransforms;
    private void Awake()
    {
        spawnTransforms = new Transform[2] { _spawnPositionLeft, _spawnPositionRight };
        rigidbody = GetComponent<Rigidbody>();
       
    }

   
    [SerializeField]
    ParticleSystem particleSystemOnEnableLeft;
    [SerializeField]
    ParticleSystem particleSystemOnEnableRight;

    private void OnEnable()
    {
        particleSystemOnCollision.SetActive(false);

        RotateCommandoProjectile();
        SetLaunchPosition();
        LaunchProjectile();
        SetLaunchAnimation();


    }



    private void SetLaunchPosition()
    {

        transform.position = spawnTransforms[Commando_Skill_Spawner.launchOrder % 2].position;

    }

    private void RotateCommandoProjectile()
    {
        
        currentPlayerPosition = new Vector3(90f + _virtualCameraPosition.rotation.eulerAngles.x, _playerPosition.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(currentPlayerPosition);
    }

    private void LaunchProjectile()
    {
        rigidbody.velocity = transform.up * _bulletSpeed;
        Invoke(nameof(Deactivate), 10f);
    }

    void Deactivate() => gameObject.SetActive(false);
   

    [SerializeField]
     GameObject particleSystemOnCollision;

    private void OnTriggerStay(Collider other)
    {
       

        if (IsBulletCollided(other))
        {
            Debug.Log("has collided!");
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
}

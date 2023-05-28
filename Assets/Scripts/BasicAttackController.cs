using UnityEngine;
using UnityEngine.UIElements;


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
    private void Start()
    {
    }

    private void OnEnable()
    {

        SetLaunchPosition();
        RotateCommandoProjectile();
        LaunchProjectile();
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
    private void Update()
    {
    }

    private void OnDisable()
    {
        CancelInvoke();
        ObjectPooler.ReturnToPool(gameObject);


    }
}

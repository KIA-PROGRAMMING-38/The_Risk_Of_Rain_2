using UnityEngine;


public class BasicAttackController : MonoBehaviour
{
    [SerializeField]
    Transform _spawnPosition;

    [SerializeField]
    Transform _playerPosition;
    Rigidbody rigidbody;
    public float _bulletSpeed;
    Vector3 currentPlayerPosition;

    private void OnEnable()
    {
        transform.position = _spawnPosition.position;
        currentPlayerPosition = new Vector3(90f, _playerPosition.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(currentPlayerPosition);
        rigidbody = GetComponent<Rigidbody>();
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

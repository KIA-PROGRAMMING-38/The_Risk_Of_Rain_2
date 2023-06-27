using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SpaceshipDoorController : MonoBehaviour
{

    private Rigidbody rb;

    public float doorMovingSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }

    private bool isAnimated;
    void Update()
    {
        if (GameManager.IsGameStarted == true)
        {
            if (!isAnimated)
            {
                transform.position += Vector3.forward * doorMovingSpeed;
                MoveDoor();
            }

        }
    }

    public float animationTime;
    public float doorExlplosionPower;
    async private UniTaskVoid MoveDoor()
    {

        await UniTask.Delay((int)animationTime);
        isAnimated = true;
        rb.isKinematic = false;
        rb.AddForce((Vector3.forward + Vector3.up) * doorExlplosionPower);

    }
}

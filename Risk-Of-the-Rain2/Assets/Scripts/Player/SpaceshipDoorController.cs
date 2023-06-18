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
    async private UniTaskVoid MoveDoor()
    {

        await UniTask.Delay((int)animationTime);
        isAnimated = true;

    }
}

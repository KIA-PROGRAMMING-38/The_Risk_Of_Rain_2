using System;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Photon.Pun.Demo.Asteroids;

public class CommandoController : MonoBehaviour
{
    public float speed = 10f;
    public float sensitivity = 2f;

    private float mouseX;
    private float mouseY;
    private float moveX;
    private float moveZ;

    private Animator animator;
    private Rigidbody rigidbody;

    private Subject<Unit> eKeyPressSubject = new Subject<Unit>();
    public IObservable<Unit> EKeyPressObservable => eKeyPressSubject;

   

    [SerializeField]
    private Transform _spaceshipTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        startSmokePS.Stop();
    }

    private void Start()
    {
        EKeyPressObservable
            .Subscribe(_ => StartGame())
            .AddTo(this); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            eKeyPressSubject.OnNext(Unit.Default);
        }

        if (GameManager.IsGameStarted == true)
        {
            // Get inputs
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
            animator.SetFloat(AnimID.EULER_X, moveX);
            animator.SetFloat(AnimID.EULER_Y, moveZ);

            mouseX = Input.GetAxis("Mouse X") * sensitivity;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            RotatePlayer();
            JumpPlayer();

            // Check for "E" key press
          
        }
        else if(GameManager.IsGameStarted == false)
        {
            transform.position = _spaceshipTransform.position;
        }
      
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    [SerializeField]
    Transform _playerVirtualCameraPosition;
    private void StartGame()
    {
       
        Debug.Log("Start Game");
        
      if(GameManager.IsGameStarted == false) 
        PlayStartAnimation();
        



    }

    [SerializeField]
    ParticleSystem startPS;

    [SerializeField]
    ParticleSystem startSmokePS;

    public float startMoveSpeed;
    private async UniTaskVoid PlayStartAnimation()
    {
        startSmokePS.Play();
        await UniTask.Delay(2000);
        startPS.transform.position = transform.position;
        startPS.Play();
        rigidbody.AddForce(Vector3.forward * startMoveSpeed,ForceMode.Impulse);
        Cinemachine_Controller.virtualCamera.Follow = _playerVirtualCameraPosition;
        Cinemachine_Controller.virtualCamera.LookAt = _playerVirtualCameraPosition;
        GameManager.IsGameStarted = true;
        startSmokePS.Stop();
    }

    private void RotatePlayer()
    {
        // Rotate the player around the Y-axis
        transform.Rotate(Vector3.up, mouseX);
    }

    private void MovePlayer()
    {
        //get transform not to get world coordinates but local ones.
        Vector3 moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
        Vector3 newVelocity = speed * moveDirection;

        newVelocity.y = rigidbody.velocity.y;

        rigidbody.velocity = newVelocity;

        animator.SetFloat(AnimID.MOVE_X, moveX);
        animator.SetFloat(AnimID.MOVE_Y, moveZ);
    }

    bool isJumping = false;
    public float jumpForce;
    private void JumpPlayer()
    {
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            animator.SetBool(AnimID.IS_JUMPING, true);
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagID.TERRAIN))
        {
            animator.SetBool(AnimID.IS_JUMPING, false);
            isJumping = false;
        }
    }


}



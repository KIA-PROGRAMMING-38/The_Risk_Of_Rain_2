using System;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class CommandoController : MonoBehaviour
{

    static public int commandoMaxHp = 60;
    public static int Hp { get; private set; }
    public void SetPlayerHp(int hp)
    {
        Hp = hp; 
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
        }
    }

    public float speed = 10f;
    public float sensitivity = 2f;

    private float mouseX;
    private float mouseY;
    private float moveX;
    private float moveZ;

    private Animator animator;
    private Rigidbody rigidbody;

    private Subject<Unit> eKeyPressSubject = new Subject<Unit>();
    private Subject<Unit> ShiftKeyPressSubject = new Subject<Unit>();

    public IObservable<Unit> EKeyPressObservable => eKeyPressSubject;
    public IObservable<Unit> ShiftKeyPressObservable => ShiftKeyPressSubject;
    [SerializeField] private MainCameraController _mainCameraController;

    [SerializeField]
    public static float DashCoolTime = 5f;
    public static float DashElapsedTime = DashCoolTime;


    [SerializeField]
    private Transform _spaceshipTransform;


    private readonly float RESET = 0f;
    public static bool isDashing = false;
    public float dashForce;
    public float dashPlayTime;
    public float lerpingSpeed;
    private float rollLerp;
   
    float originalSpeed;

    [SerializeField]
    public ParticleSystem _dashParticle;
    public Transform _dashParticlePosition;
    private void Awake()
    {
        
        originalSpeed = speed * lerpingSpeed;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        startSmokePS.Stop();
    }

    private void Start()
    {
        SetPlayerHp(commandoMaxHp);

        EKeyPressObservable
            .Subscribe(_ => StartGame())
            .AddTo(this);

        ShiftKeyPressSubject
          .Subscribe(_ => RollAndDash())
          .AddTo(this);
    }

    private void Update()
    {
       
            DashElapsedTime += Time.deltaTime;
       
     


        if (Input.GetKeyDown(KeyCode.E))
        {
            eKeyPressSubject.OnNext(Unit.Default);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && DashCoolTime < DashElapsedTime)
        {
            ShiftKeyPressSubject.OnNext(Unit.Default);
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
        
        else if (GameManager.IsGameStarted == false)
        {
            rigidbody.Sleep();
            transform.position = _spaceshipTransform.position;
        }

    }

    private void FixedUpdate()
    {
        if (GameManager.IsGameStarted ==true)
        {
            MovePlayer();
            rollLerp += Time.deltaTime;
        }
     



    }


    [SerializeField]
    Transform _playerVirtualCameraPosition;
    private void StartGame()
    {

        Debug.Log("Start Game");

        if (GameManager.IsGameStarted == false && GameManager.IsPlayerArrived == true)
        {
           
            PlayStartAnimation();
            PlayCrossHair();
        }
         

    }

   

    [SerializeField]
    GameObject _crossHair;
    private void PlayCrossHair()
    {
        _crossHair.SetActive(true);
    }


    [SerializeField]
    ParticleSystem startPS;

    [SerializeField]
    ParticleSystem startSmokePS;
    
   


    public float startMoveSpeed;
    private async UniTaskVoid PlayStartAnimation()
    {
        startSmokePS.Play();
             GameManager.IsGameStarted = true;
        await UniTask.Delay(2000);
        startPS.transform.position = transform.position;
        startPS.Play();
      
        Cinemachine_Controller.virtualCamera.Follow = _playerVirtualCameraPosition;
        Cinemachine_Controller.virtualCamera.LookAt = _playerVirtualCameraPosition;
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

        if (isDashing == true)
        {
            speed = Lerp2D.EaseOutExpo(originalSpeed, dashForce, rollLerp);
        }
        else
        {
            speed = originalSpeed;
        }
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

 
    async private UniTaskVoid RollAndDash()
    {
        
        if (!isDashing)
        {
            DashElapsedTime = 0f;
            _dashParticle.transform.position = _dashParticlePosition.position;
            _dashParticle.Play();

            //to hold the position...
            isDashing = true;
            rollLerp = 0f;
           
            // turn off on the animation statemachine behavior.
            animator.SetTrigger(AnimID.ROLL);
            speed = dashForce;

            await UniTask.Delay((int)(dashPlayTime * 1000));
            isDashing = false;


            speed = originalSpeed;
            _dashParticle.Stop();
        }

    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagID.TERRAIN))
        {
            animator.SetBool(AnimID.IS_JUMPING, false);
            isJumping = false;
        }
    }

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagID.ENEMY))
        {
            Debug.Log("got damaged!");
            TakeDamage(-1);
          
            _mainCameraController.ChangeVolumeToDamageEffect();
        }
       
    }


}



using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandoSkillSpawner : MonoBehaviour
{
    public float _basicAttackCoolTime;
    [SerializeField]
    public static float UltimateCoolTime = 10f;
    public static float UltimateElapsedTime = UltimateCoolTime;
    float _elapsedTime;
    internal static int launchOrder;




    private Animator animator;
    enum pistolLaunchDirection
    {
        Left,
        Right
    }

    pistolLaunchDirection CommandoLaunchDirecton;
    private void Awake()
    {
        bulletLightLeft.SetActive(false); 
        bulletLightRight.SetActive(false);

        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {

    }
    [SerializeField]
     Transform _playerPosition;

    [SerializeField]
    Transform _virtualCameraPosition;



    private void Update()
    {
        if (GameManager.IsGameStarted == true)
        {
            Debug.DrawRay(_playerPosition.position, 100000 * _virtualCameraPosition.forward, Color.yellow);

            _elapsedTime += Time.deltaTime;
            UltimateElapsedTime += Time.deltaTime;

            if (Input.GetKey(KeyCode.Mouse0) && _elapsedTime > _basicAttackCoolTime)
            {
                ShootBullet();
                SetAnimation();
            }
            if (Input.GetKey(KeyCode.Mouse1) && UltimateElapsedTime > UltimateCoolTime)
            {
                ShootUltimate();
            }
        }
       

    }



    private void ShootBullet()
    {

        GameObject CommandoBasicAttack = ObjectPooler.SpawnFromPool(TagID.COMMANDO_BASIC_ATTACK, transform.localPosition);
        PlayBulletLight();
        launchOrder++;
        _elapsedTime = 0.0f;

    }

    private void ShootUltimate()
    {
        GameObject CommandoUltimate = ObjectPooler.SpawnFromPool(TagID.COMMANDO_ULTIMATE_ATTACK, transform.localPosition);
        UltimateElapsedTime = 0f;
    }

    [SerializeField]
    GameObject bulletLightLeft;
    [SerializeField]
    GameObject bulletLightRight;

    async private UniTaskVoid PlayBulletLight()
    {
        switch (launchOrder % 2)
        {
            case (int)pistolLaunchDirection.Right:
                bulletLightLeft.SetActive(true);
                await UniTask.Delay(120);
                bulletLightLeft.SetActive(false);

                break;
            case (int)pistolLaunchDirection.Left:
                bulletLightRight.SetActive(true);
                await UniTask.Delay(120);
                bulletLightRight.SetActive(false);
                break;
        }
      
      

    }
    private void SetAnimation()
    {
        switch (launchOrder % 2)
        {
            case (int)pistolLaunchDirection.Right:
                animator.SetFloat(AnimID.PISTOL_DIRECTION, (float)pistolLaunchDirection.Left);

                break;
            case (int)pistolLaunchDirection.Left:
                animator.SetFloat(AnimID.PISTOL_DIRECTION, (float)pistolLaunchDirection.Right);
                break;
        }

        animator.SetTrigger(AnimID.PISTOL_LAUNCHED);

    }


    private void ShowCrosshair()
    {

    }
}

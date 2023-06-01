using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commando_Skill_Spawner : MonoBehaviour
{
    public float _spawnCoolTime;
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
        Debug.DrawRay(_playerPosition.position, 100000 * _virtualCameraPosition.forward, Color.yellow);

        _elapsedTime += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) && _elapsedTime > _spawnCoolTime)
        {
            ShootBullet();
            SetAnimation();
        }

    }



    private void ShootBullet()
    {

        GameObject CommandoBasicAttack = ObjectPooler.SpawnFromPool(TagID.COMMANDO_BASIC_ATTACK, transform.localPosition);

        launchOrder++;
        _elapsedTime = 0.0f;


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
}

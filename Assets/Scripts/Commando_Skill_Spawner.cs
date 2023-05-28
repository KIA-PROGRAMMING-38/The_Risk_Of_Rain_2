using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commando_Skill_Spawner : MonoBehaviour
{
    public float _spawnCoolTime;
    float _elapsedTime;
    internal static int launchOrder;

    [SerializeField]
    GameObject _spawnPosition;

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
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) && _elapsedTime > _spawnCoolTime)
        {
           
            ShootBullet();
            SetAnimation();
           

        }

    }

    private void ShootBullet()
    {
        if (_spawnPosition == null)
        {
            Debug.LogWarning("Spawn position isn't set.");
            return;
        }

        GameObject CommandoBasicAttack = ObjectPooler.SpawnFromPool(TagID.COMMANDO_BASIC_ATTACK, _spawnPosition.transform.localPosition);
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

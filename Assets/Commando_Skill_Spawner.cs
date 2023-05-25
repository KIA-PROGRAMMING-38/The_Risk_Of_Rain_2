using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commando_Skill_Spawner : MonoBehaviour
{
    public float _spawnCoolTime;
    float _elapsedTime;

    [SerializeField]
    GameObject _spawnPosition;

    private void OnEnable()
    {
       
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) && _elapsedTime > _spawnCoolTime)
        {
            ShootBullet();
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
        Debug.Log($"SpawnPosition: {_spawnPosition.transform.position}");
        _elapsedTime = 0.0f;

    }
}

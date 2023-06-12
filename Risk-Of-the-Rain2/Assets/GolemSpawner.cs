using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GolemSpawner : MonoBehaviour
{
    private Transform[] golemSpawnPosition;

    [SerializeField]
    Transform firstSpawnPosition;
    [SerializeField]
    Transform secondSpawnPosition;
    [SerializeField]
    Transform thirdSpawnPosition;


    public float spawnInterval;
    private float elapsedTime;

    private Subject<Unit> spawnGolem = new Subject<Unit>();
    public IObservable<Unit> EKeyPressObservable => spawnGolem;

    private void Awake()
    {
        SetPosition();
    }

    private void Start()
    {
        EKeyPressObservable
          .Subscribe(_ => SpawnGolem())
          .AddTo(this);
    }
    bool isSpawnable;
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        isSpawnable = CheckIfSpawnIsPoss(elapsedTime, spawnInterval);
        if (isSpawnable == true) spawnGolem.OnNext(Unit.Default);
    }


    int spawnOrder;
    private void SpawnGolem()
    {

        if (elapsedTime > spawnInterval)
        {
            GameObject golem = ObjectPooler.SpawnFromPool(TagID.COMMANDO_BASIC_ATTACK, golemSpawnPosition[spawnOrder % 3].localPosition);
        }
        spawnOrder++;

    }

    private bool CheckIfSpawnIsPoss(float elapsedTime, float spawnInterval)
    {
        bool spawnable = elapsedTime > spawnInterval;
        return spawnable;
    }

    private void SetPosition()
    {
        golemSpawnPosition = new Transform[3];

        golemSpawnPosition[0] = firstSpawnPosition;
        golemSpawnPosition[1] = secondSpawnPosition;
        golemSpawnPosition[2] = thirdSpawnPosition;
    }
}

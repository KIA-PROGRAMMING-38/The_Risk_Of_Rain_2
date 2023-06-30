using System;
using UniRx;
using UnityEngine;

public class GolemSpawner : MonoBehaviour
{


    public float spawnInterval;
    private float elapsedTime;

    private Subject<Unit> DecreaseInterval = new Subject<Unit>();
    public IObservable<Unit> bossObservable => DecreaseInterval;


    private Transform[] golemSpawnPosition;

    [SerializeField]
    Transform firstSpawnPosition;
    [SerializeField]
    Transform secondSpawnPosition;
    [SerializeField]
    Transform thirdSpawnPosition;

    [SerializeField]
    private TitanController _titanController;


    
    private void SetPosition()
    {
        golemSpawnPosition = new Transform[3];

        golemSpawnPosition[0] = firstSpawnPosition;
        golemSpawnPosition[1] = secondSpawnPosition;
        golemSpawnPosition[2] = thirdSpawnPosition;
    }

    private void Awake()
    {
        SetPosition();
    }

    private void Start()
    {
        bossObservable
          .Subscribe(_ => DecreaseSpawnInterval())
          .AddTo(this);
    }
    bool isSpawnable;
    private void Update()
    {
        if(_titanController.isDead == false)
        {
            elapsedTime += Time.deltaTime;
            isSpawnable = CheckSpawningPoss(elapsedTime, spawnInterval);
            if (isSpawnable == true) SpawnGolem();
        }
        if (_titanController.isSpawned == true)
        {
            DecreaseInterval.OnNext(Unit.Default);
        }

    }



    int spawnOrder;
    private void SpawnGolem()
    {
      

        GameObject Golem = ObjectPooler.SpawnFromPool(TagID.GOLEM,
            golemSpawnPosition[spawnOrder % 3].position);
        

        spawnOrder++;
       
        Debug.Log($"{Golem}: goleInfo");
        elapsedTime = 0f;
        
    }

   
    private bool CheckSpawningPoss(float elapsedTime, float spawnInterval)
    {

        if (elapsedTime > spawnInterval)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void DecreaseSpawnInterval()
    {
        spawnInterval = 10f;
    }

}

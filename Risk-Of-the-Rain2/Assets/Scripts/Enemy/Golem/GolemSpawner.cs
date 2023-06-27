using System;
using UniRx;
using UnityEngine;

public class GolemSpawner : MonoBehaviour
{


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
        isSpawnable = CheckSpawningPoss(elapsedTime, spawnInterval);
        if (isSpawnable == true) SpawnGolem();
    }


    [SerializeField]
    Golem_Controller _golemController;


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
    private Transform[] golemSpawnPosition;

    [SerializeField]
    Transform firstSpawnPosition;
    [SerializeField]
    Transform secondSpawnPosition;
    [SerializeField]
    Transform thirdSpawnPosition;
    private void SetPosition()
    {
        golemSpawnPosition = new Transform[3];

        golemSpawnPosition[0] = firstSpawnPosition;
        golemSpawnPosition[1] = secondSpawnPosition;
        golemSpawnPosition[2] = thirdSpawnPosition;
    }

}

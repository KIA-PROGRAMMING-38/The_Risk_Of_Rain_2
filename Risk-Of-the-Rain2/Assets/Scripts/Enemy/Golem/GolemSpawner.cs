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
        isSpawnable = CheckIfSpawnIsPoss(elapsedTime, spawnInterval);
        if (isSpawnable == true) spawnGolem.OnNext(Unit.Default);
    }


    [SerializeField]
    GameObject _golem;


    [SerializeField]
    Golem_Controller _golemController;


    int spawnOrder;
    private void SpawnGolem()
    {
        
        spawnOrder = UnityEngine.Random.RandomRange(0, 3);
        GameObject golem = Instantiate(_golem, golemSpawnPosition[spawnOrder].position, Quaternion.identity);
        golem.SetActive(false);
        golem.SetActive(true);


        golem.GetComponent<Golem_Controller>().hp = _golemController.maxHp;


        Debug.Log($"{golem}: goleInfo");
        elapsedTime = 0f;
    }

   
    private bool CheckIfSpawnIsPoss(float elapsedTime, float spawnInterval)
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

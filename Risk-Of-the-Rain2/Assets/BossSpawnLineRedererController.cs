using System;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Rendering.PostProcessing;
using System.Runtime.Versioning;

public class BossSpawnLineRedererController : MonoBehaviour
{
    private Subject<Unit> eKeyBossSpawn = new Subject<Unit>();
    public IObservable<Unit> bossObservable => eKeyBossSpawn;
    
  
    public GameObject _camera;
    private IDisposable disposable;


    public float radiusIncreasingSensitivity;
    public LineRenderer lineRenderer;
    public Transform _laserSpawnPosition;
    private void Awake()
    {
        laser.localScale = Vector3.zero;
      
    }

    private void Start()
    {
        bossObservable
            .Subscribe(_ => StartSpawningBoss())
            .AddTo(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsOnZone == true)
        {
            eKeyBossSpawn.OnNext(Unit.Default);
        }

        if (GameManager.IsBossSpawned == true)
        {
            IncreaseSize();
        }
    }

    static bool playerIsOnZone;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagID.PLAYER))
        {
            playerIsOnZone = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagID.PLAYER))
        {
            playerIsOnZone = false;
        }
    }

    private void StartSpawningBoss() => GameManager.IsBossSpawned = true;

    public int laserSpawnDelay;
    static bool conditionIsTrue;
  

    private void SendMessageToCamera()
    {
        _camera.SendMessage(MessageID.BOSS_SPAWN_EFFECT_ON);
        Debug.Log("Sent Message");
    }

    public float growingSpeed;
    [SerializeField]
    public Transform laser;
    private async UniTaskVoid IncreaseSize()
    {
        SendMessageToCamera();
        await UniTask.Delay(laserSpawnDelay);
        Vector3 vectorMesh = laser.localScale;
        float growing = growingSpeed * Time.deltaTime;
        laser.localScale = new Vector3(vectorMesh.x + growing, vectorMesh.y + growing, vectorMesh.z + growing);
    }


}


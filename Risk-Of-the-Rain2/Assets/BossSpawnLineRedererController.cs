using System;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Photon.Pun.Demo.Asteroids;

public class BossSpawnLineRedererController : MonoBehaviour
{
    private Subject<Unit> eKeyPressSubject = new Subject<Unit>();
    public IObservable<Unit> EKeyPressObservable => eKeyPressSubject;


    public float radius = 5f;

    public float radiusSizeSensitiviy;
    public int numSegments = 100;


    public float radiusIncreasingSensitivity;
    public LineRenderer lineRenderer;
    public Transform _laserSpawnPosition;
    private void Awake()
    {
        lineRenderer.transform.position = transform.position;
        radius = 0f;
    }

    private void Start()
    {
        EKeyPressObservable
           .Subscribe(_ => DrawLaser())
           .AddTo(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsOnZone == true)
        {
            GameManager.IsBossSpawned = true;
            eKeyPressSubject.OnNext(Unit.Default);
        }

        if (GameManager.IsBossSpawned == true)
        {
            DelayLaserSpawn();
            IncreaseRadius();
        }
    }

    bool playerIsOnZone;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagID.PLAYER))
        {
            playerIsOnZone = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        playerIsOnZone = false;
    }


    void DrawLaser()
    {
        Debug.Log("Boss Spawned! ! ");
        lineRenderer.positionCount = numSegments + 1;

        float angleStep = 360f / numSegments;

        for (int i = 0; i <= numSegments; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius * radiusSizeSensitiviy;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius * radiusSizeSensitiviy;

            Vector3 pos = new Vector3(x, y, 0f);
            lineRenderer.SetPosition(i, pos);
        }

        lineRenderer.loop = true;

    }

    public int laserSpawnDelay;
    public bool isDrawingLaserStart;
    public float maxRadius;
    public float startRadius = 0f;
    float lerp;
    private async UniTaskVoid IncreaseRadius()
    {

        if (isDrawingLaserStart == true)
        {
            lerp += radiusIncreasingSensitivity * Time.deltaTime;
            radius = Mathf.Lerp(startRadius, maxRadius, lerp);
            DrawLaser();
        }

        await UniTask.NextFrame();
        Debug.Log("2 : you should turn off this function.");

    }
    private async UniTaskVoid DelayLaserSpawn()
    {
        await UniTask.Delay(laserSpawnDelay);
        isDrawingLaserStart = true;
        Debug.Log("you should turn off this function.");
        return;
    }
}


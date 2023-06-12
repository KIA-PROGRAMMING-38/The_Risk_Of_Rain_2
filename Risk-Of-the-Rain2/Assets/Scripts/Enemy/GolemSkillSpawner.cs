using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class GolemSkillSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject _player;
    [SerializeField]
    public GameObject _laserSpawnPosition;
    private LineRenderer lineRenderer;
    private Renderer renderer;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        renderer = GetComponent<Renderer>();

    }

    public float rangeRadius;

    bool Detected;
    bool isLaserSpawning = false;
    bool isLaunched;
    void Update()
    {
        RotateGolemHead();

        Detected = DetectPlayer();

        if (Detected)
        {
            TurnOnLaser();
            changeLaserColor();
        }

        else TurnOffLaser();

    }


    private bool DetectPlayer()
    {
        int layerMask = LayerMask.GetMask(LayerID.PLAYER);
        Collider[] colliders = Physics.OverlapSphere(transform.position, rangeRadius, layerMask);

        if (colliders.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void TurnOnLaser()
    {
        if (isLaserSpawning == false)
        {
            lineRenderer.enabled = true;
        }

        if (isLaunched == false)
        {
            LaunchLaserToPlayer();
        }
        else if (isLaunched == true)
        {
            lineRenderer.SetPosition(0, _laserSpawnPosition.transform.position);
        }


    }

    private void TurnOffLaser()
    {
        lineRenderer.enabled = false;
    }

    
    public Transform _golemHead;
    private void RotateGolemHead()
    {
        _golemHead.LookAt(_player.transform);
    }

    public float emissionIntensity = 1f;


    public Material _targetMaterial;
    public Material _launchMaterial;

    int currentBlinkCount;
    public int blinkCount;
    private async UniTaskVoid LaunchLaserToPlayer()
    {
        lineRenderer.SetPosition(0, _laserSpawnPosition.transform.position);
        lineRenderer.SetPosition(1, _player.transform.position);
        await UniTask.Delay(150);
    }
    private async UniTaskVoid changeLaserColor()
    {

        if (isLaserSpawning == false)
        {
            isLaserSpawning = true;
            renderer.material = _targetMaterial;
            await UniTask.Delay(3000);

            while (currentBlinkCount < blinkCount)
            {
              
                lineRenderer.enabled = false;
                await UniTask.Delay(30);
                lineRenderer.enabled = true;
                await UniTask.Delay(30);
                currentBlinkCount++;
            }
            currentBlinkCount = 0;


            renderer.material = _launchMaterial;
            isLaunched = true;
            await UniTask.Delay(300);

            lineRenderer.enabled = false;
            await UniTask.Delay(2000);



            isLaserSpawning = false;
            isLaunched = false;

          
            lineRenderer.enabled = false;
        }

    }
}

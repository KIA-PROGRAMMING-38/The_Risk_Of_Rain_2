using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class SpaceshipController : MonoBehaviour
{
    public float speed;


    private Rigidbody rigidbody;

    [SerializeField]
    Transform startPosition;

    [SerializeField]
    GameObject arrivalExplosionPS;

    [SerializeField]
    GameObject RocketPluimingPS;



    private Collider collider;
    float currentTime;
    void Awake()
    {

        _1stEnteringEffect.SetActive(false);
        _2ndEnteringEffect.SetActive(false);

        arrivalExplosionPS.SetActive(false);
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        currentTime = Time.time;
    }

    public float startDelayTime;
    public float EnteringTiming;
    private float elapsedTime;

    private bool isLaunched;
    private bool isEffectPlayed;
    public float enteringEffectDuration;
    public float effectGap;
    public float fireRoundEffectDuration;
    public GameObject _1stEnteringEffect;
    public GameObject _2ndEnteringEffect;
    void Update()
    {
        elapsedTime += Time.time - currentTime;

        if (elapsedTime > startDelayTime && !isLaunched)
        {
            isLaunched = true;
            rigidbody.velocity = (startPosition.position - transform.position).normalized * speed;
            elapsedTime = 0f;
        }

        if (elapsedTime > EnteringTiming && !isEffectPlayed)
        {
            isEffectPlayed = true;
            PlayEnteringEffect();

        }
    }

    private Vector3 effectPosition;

    async private UniTaskVoid PlayEnteringEffect()
    {
        _1stEnteringEffect.SetActive(true);
        effectPosition = _1stEnteringEffect.transform.position;
        _1stEnteringEffect.transform.position = effectPosition;

        await UniTask.Delay((int)effectGap);

        _2ndEnteringEffect.SetActive(true);
        effectPosition = _2ndEnteringEffect.transform.position;
        _2ndEnteringEffect.transform.position = effectPosition;


        await UniTask.Delay((int)enteringEffectDuration);
        _1stEnteringEffect.SetActive(false);
        _2ndEnteringEffect.SetActive(false);
    }

    [SerializeField]
    GameObject TurnOffRoundFireEffect;
    async private UniTaskVoid TurnOffFireRoundEffect()
    {
        
        await UniTask.Delay((int)fireRoundEffectDuration);
        TurnOffRoundFireEffect.SetActive(false);
       
    }
    [SerializeField] GameObject _camera;
    private void OnTriggerStay(Collider spaceship)
    {
        if (IsSpaceshipCollided(spaceship))
        {
            arrivalExplosionPS.SetActive(true);// explosion Particle
            RocketPluimingPS.SetActive(false);
            rigidbody.Sleep();
            collider.enabled = false;
            GameManager.IsPlayerArrived = true;
            _camera.SendMessage(MessageID.VIBRATE_CAMERA_ON_ARRIVAL);
            TurnOffFireRoundEffect();
        }

    }

    private bool IsSpaceshipCollided(Collider collision)
    {
        return (collision.CompareTag(TagID.TERRAIN));
    }
}

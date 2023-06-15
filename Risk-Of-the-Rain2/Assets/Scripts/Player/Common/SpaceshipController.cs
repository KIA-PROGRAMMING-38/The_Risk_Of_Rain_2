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

        _enteringEffect.SetActive(false);
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
    public GameObject _enteringEffect;
    void Update()
    {
        
        elapsedTime += Time.time - currentTime;
        Debug.Log($"{elapsedTime}");
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

    async private UniTaskVoid PlayEnteringEffect()
    {
        _enteringEffect.SetActive(true);
        await UniTask.Delay((int)enteringEffectDuration);
        _enteringEffect.SetActive(false);
    }

    private void OnTriggerStay(Collider spaceship)
    {
        if (IsSpaceshipCollided(spaceship))
        {
            arrivalExplosionPS.SetActive(true);// explosion Particle
            RocketPluimingPS.SetActive(false);
            rigidbody.Sleep();

            collider.enabled = false;

        }

    }

    private bool IsSpaceshipCollided(Collider collision)
    {
        return (collision.CompareTag(TagID.TERRAIN));
    }
}

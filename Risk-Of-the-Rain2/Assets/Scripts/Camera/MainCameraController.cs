using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cysharp.Threading.Tasks;
using System.Threading;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

public class MainCameraController : MonoBehaviour
{

    Volume volume;
    UnityEngine.Rendering.Universal.Vignette vignette;
    ColorAdjustments colorAdjustments;

    Color colorFilter;
    float postExposure;
    float contrast;
    float hueShift;
    float saturation;

    private CancellationTokenSource _source;
    public float currentExposure;
    public float originalExposure;
    public float darkendExposure;
    public float lerp;
    public float exposureChangingSpeed;
    public float vignetteDisappearingSpeed = 1f;
    [SerializeField]
    public CinemachineVirtualCamera _virtualCamera;
    public float amplitudeGain;
    public float frequencyGain;
    public float vibratingTime;

    void Start()
    {

        volume = GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);

    }

    public float _startShakingIncreaseSpeed;

    void Update()
    {
        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.Override(1 - Mathf.Lerp(0, 1, Time.time * vignetteDisappearingSpeed));
        }


        if (GameManager.IsPlayerArrived == false)
        {
            m_FrequencyGainStart += Time.deltaTime * _startShakingIncreaseSpeed;
            _vibrationDurationTimeStart += Time.deltaTime * _startShakingIncreaseSpeed;
            VibrateCameraStart();

        }
    }


    private async UniTaskVoid BossSpawnEffectOn()
    {
        lerp += Time.deltaTime * exposureChangingSpeed;
        currentExposure = Mathf.Lerp(darkendExposure, originalExposure, 1 - lerp);

        colorAdjustments.postExposure.value = currentExposure;
        TurnOnRed();

        if (lerp > 2)
        {
            RaiseExposure();
        }






    }
    private async UniTaskVoid RaiseExposure()
    {

        lerp += Time.deltaTime * exposureChangingSpeed;
        currentExposure = Mathf.Lerp(originalExposure, darkendExposure, 1 - lerp);
    }

    public float ColorChangingSpeed;
    public float transitionDuration = 2.0f;
    public Color initialColor;
    public Color bossSpawnColor;
    bool isChangingToRed;
    float elapsed;
    private async UniTaskVoid TurnOnRed()
    {

        ColorAdjustments colorAdjustments;


        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {

            if (isChangingToRed == false)
            {
                elapsed = 0f;
                isChangingToRed = true;

            }


            while (elapsed < transitionDuration)
            {
                Debug.Log($"{elapsed}Changing To Red");
                elapsed += Time.deltaTime;

                // Lerp the color filter value from the initial color to red
                colorAdjustments.colorFilter.value = Color.Lerp(initialColor, bossSpawnColor, elapsed / transitionDuration);

                // wait for the next frame
                await UniTask.NextFrame();
            }



        }



    }




    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    private float m_FrequencyGainStart = 0f;
    private float _vibrationDurationTimeStart = 0f;

    public float m_AmplitudeGain;
    public float m_FrequencyGain;

    public float m_AmplitudeGainOnArrive;
    public float m_FrequencyGainOnArrive;
    public float _vibrationDurationTimeOnArrive;

    public float _vibrationDurationTime;

    private async UniTaskVoid VibrateCameraOnArrival()
    {
        virtualCameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        virtualCameraNoise.m_AmplitudeGain = m_AmplitudeGainOnArrive;
        virtualCameraNoise.m_FrequencyGain = m_FrequencyGainOnArrive;

        Debug.Log("impulse! ARRIVAL!");

        await UniTask.Delay((int)(_vibrationDurationTimeOnArrive * 1000));

        Debug.Log("turnback!");
        virtualCameraNoise.m_AmplitudeGain = 0;
        virtualCameraNoise.m_FrequencyGain = 0;

    }

    private void VibrateCameraStart()
    {
        virtualCameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        virtualCameraNoise.m_AmplitudeGain = m_FrequencyGainStart;
        virtualCameraNoise.m_FrequencyGain = _vibrationDurationTimeStart;

    }

    private async UniTaskVoid VibrateCamera()
    {
        virtualCameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        virtualCameraNoise.m_AmplitudeGain = m_AmplitudeGain;
        virtualCameraNoise.m_FrequencyGain = m_FrequencyGain;


        await UniTask.Delay((int)(_vibrationDurationTime * 1000));


        virtualCameraNoise.m_AmplitudeGain = 0;
        virtualCameraNoise.m_FrequencyGain = 0;

    }

    [SerializeField]
    Volume _damagedEffectVolume;



    [SerializeField]
    Color _damagedColor;
    public float _damageEffectDurationSeconds;
    public async UniTaskVoid ChangeVolumeToDamageEffect()
    {

        ColorAdjustments colorAdjustments;


        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {


            // Lerp the color filter value from the initial color to red
            colorAdjustments.colorFilter.value = _damagedColor;

            // wait for the next frame
            await UniTask.Delay((int)(_damageEffectDurationSeconds * 1000));
            colorAdjustments.colorFilter.value = initialColor;
        }



    }

}


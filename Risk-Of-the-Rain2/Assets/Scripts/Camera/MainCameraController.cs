using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cysharp.Threading.Tasks;
using System.Threading;
using Cinemachine;
using System;
using static Unity.VisualScripting.Member;

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



    public float currentExposure;
    public float originalExposure;
    public float darkendExposure;
    public float lerp;
    public float exposureChangingSpeed;
    public float vignetteDisappearingSpeed = 1f;
    [SerializeField]
    public CinemachineVirtualCamera _virtualCamera;
    [SerializeField]
    private Transform virtualCamera;
    public float amplitudeGain;
    public float frequencyGain;
    public float vibratingTime;


    private CancellationTokenSource _cancelTokenSource;
    private CancellationTokenSource _playTokenSource;
    private CancellationToken _canceltoken;

    void Start()
    {
        defaultPosition = virtualCamera.position;
        // 토큰 소스 초기화
        _cancelTokenSource = new CancellationTokenSource();
        _playTokenSource = new CancellationTokenSource();
        _cancelTokenSource.Cancel();

        // 토큰 초기화
        _canceltoken = _playTokenSource.Token;
     
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);
    }

    public float _startShakingIncreaseSpeed;


    private float startLerp;
    public float startExposureIncreasingSpeed;
    void Update()
    {


        if (GameManager.IsPlayerArrived == false)
        {
            startLerp += Time.deltaTime * startExposureIncreasingSpeed;
            RaiseExposureStart(-3, 0.4f, startLerp);


            m_FrequencyGainStart += Time.deltaTime * _startShakingIncreaseSpeed;
            _vibrationDurationTimeStart += Time.deltaTime * _startShakingIncreaseSpeed;
            VibrateCameraStart();
        }

        if (GameManager.IsGameStarted == true)
        {
            LowerCamera().Forget();

        }

        if (defaultPosition.y - _loweredQuantity < _lowerQuantity)
        {
            _canceltoken = _cancelTokenSource.Token;
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


    private void RaiseExposureStart(float from, float to, float lerp)
    {

        currentExposure = Mathf.Lerp(from, to, lerp);
        colorAdjustments.postExposure.value = currentExposure;
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



        await UniTask.Delay((int)(_vibrationDurationTimeOnArrive * 1000));


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

    public float vignetteIntensity;

    public float _damageEffectDurationSeconds;
    public async UniTaskVoid ChangeVolumeToDamageEffect()
    {
        ColorAdjustments colorAdjustments;


        //if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        //{
        //    Debug.Log("Color Changed!");
        //    // Lerp the color filter value from the initial color to red
        //    colorAdjustments.colorFilter.value = _damagedColor;

        //    // wait for the next frame
        //    await UniTask.Delay((int)(_damageEffectDurationSeconds * 1000));
        //    colorAdjustments.colorFilter.value = initialColor;
        //}


        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments) &&
      volume.profile.TryGet<Vignette>(out vignette))
        {
            Debug.Log("Color and Vignette Changed!");


            Color initialColor = colorAdjustments.colorFilter.value;
            float initialIntensity = vignette.intensity.value;
            Color initialVignetteColor = vignette.color.value;

            colorAdjustments.colorFilter.value = _damagedColor;
            vignette.intensity.value = vignetteIntensity;
            vignette.color.value = _damagedColor;

            await UniTask.Delay((int)(_damageEffectDurationSeconds * 1000));

            colorAdjustments.colorFilter.value = initialColor;
            vignette.intensity.value = initialIntensity;
            vignette.color.value = initialVignetteColor;
        }
    }




    private void PlayStartCameraMove()
    {

    }

    public float _cameraMovingTime;
    public float _loweringSpeed;
    public float _lowerQuantity;
    private float _loweredQuantity;
    private float elpasedTime;
    
    private Vector3 defaultPosition;


   
    

    public async UniTaskVoid LowerCamera()
    {
     
        while (true)
        {
         
            _loweredQuantity = defaultPosition.y - virtualCamera.position.y;
         
            if (_loweredQuantity < _lowerQuantity)
            {
                virtualCamera.position += Vector3.down * _loweringSpeed;
            }


           await UniTask.Yield(_canceltoken);

        }

     
    }

}


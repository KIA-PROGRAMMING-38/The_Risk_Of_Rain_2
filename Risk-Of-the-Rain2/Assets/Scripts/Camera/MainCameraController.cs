using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cysharp.Threading.Tasks;
using System.Threading;
using Cinemachine;
using System;
using static Unity.VisualScripting.Member;
using Unity.VisualScripting;
using UniRx;

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


    [Space(15f)]

    [Header("Boss Spawn Effect")]
    [Space(5f)]
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

    [Space(5f)]

    public float amplitudeGain;
    public float frequencyGain;
    public float vibratingTime;

    private bool isCamLowering;
    private CancellationTokenSource _cancelTokenSource;
    private CancellationTokenSource _playTokenSource;
    private CancellationToken _canceltoken;

    [Space(15f)]
    [Header("Boss Spawn Effect")]
    [Space(5f)]
    public float ColorChangingSpeed;
    public float transitionDuration = 2.0f;
    public Color initialColor;
    public Color bossSpawnColor;
    public Color bossSpawnCompleteColor;
    bool isChangingToRed;
    float elapsed;

    [Space(15f)]
    [Header("Start Move")]
    [Space(5f)]
    public float _cameraMovingTime;
    public float _loweringSpeed;
    public float _lowerQuantity;
    private float _loweredQuantity = 0;
    private float elpasedTime;
    private Vector3 defaultPosition;

    [Space(15f)]
    [Header("Intro Shake")]
    [Space(5f)]
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    private float m_FrequencyGainStart = 0f;
    private float _vibrationDurationTimeStart = 0f;

    public float m_AmplitudeGain;
    public float m_FrequencyGain;

    public float m_AmplitudeGainOnArrive;
    public float m_FrequencyGainOnArrive;
    public float _vibrationDurationTimeOnArrive;

    public float _vibrationDurationTime;

    [Space(15f)]
    [Header("Player Damaged Effect")]
    [Space(5f)]
    [SerializeField]
    Volume _damagedEffectVolume;
    [SerializeField]
    Color _damagedColor;

    public float vignetteIntensity;

    public float _damageEffectDurationSeconds;

    [Space(15f)]
    [Header("Inventory")]
    [Space(5f)]


    [SerializeField]
    public float blurSpeed;
    [SerializeField]
    public float originalDepthDistance;
    [SerializeField]
    public float targetDepthOfDistance;
    private float depthLerp;
    private DepthOfField depthOfField;
    [Space(15f)]
    [Header("Reference")]
    [Space(5f)]

    [SerializeField]
    TitanController _titanController;
    void Start()
    {
        defaultPosition = virtualCamera.position;

        _cancelTokenSource = new CancellationTokenSource();
        _playTokenSource = new CancellationTokenSource();
        //_cancelTokenSource.Cancel();

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
            RaiseExposureStart(-3, originalExposure, startLerp);


            m_FrequencyGainStart += Time.deltaTime * _startShakingIncreaseSpeed;
            _vibrationDurationTimeStart += Time.deltaTime * _startShakingIncreaseSpeed;
            VibrateCameraStart();
        }

        if (GameManager.IsGameStarted == true && isCamLowering == false)
        {

            LowerCamera().Forget();
            if (isCamLowering == false)
            {
                isCamLowering = true;
                //InvokeRepeating(nameof(CancelUniTask), 2.5f, 1);

            }
        }



        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tapIsOn = true;
            isInitalized = false;

           
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            PlayInventoryAnim();
           
        }
        else
        {
            ShutInventoryAnim();
          
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            tapIsOn = false;
            isInitalized = false;

           


        }

    }
    private bool isInitialized;
    private async UniTaskVoid BossSpawnEffectOn()
    {
        lerp += Time.deltaTime * exposureChangingSpeed;
        currentExposure = Mathf.Lerp(darkendExposure, originalExposure, 1 - lerp);

        colorAdjustments.postExposure.value = currentExposure;
        if (_titanController.isSpawned == false)
        {
            TurnOnRed();
        }
        else if (_titanController.isSpawned == true)
        {
            TurnBackToDefault();
        }


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
    private bool isTurnigBackToDefault;
    private async UniTaskVoid TurnBackToDefault()
    {
        ColorAdjustments colorAdjustments;
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {

            if (isTurnigBackToDefault == false)
            {
                elapsed = 0f;
                isTurnigBackToDefault = true;

            }

            while (elapsed < transitionDuration)
            {
                Debug.Log("Truning Back to normal");
                elapsed += Time.deltaTime;

                // Lerp the color filter value from the initial color to red
                colorAdjustments.colorFilter.value = Color.Lerp(bossSpawnColor, bossSpawnCompleteColor, elapsed / transitionDuration);

                // wait for the next frame
                await UniTask.NextFrame();
            }
        }
    }





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


    public async UniTaskVoid ChangeVolumeToDamageEffect()
    {
        ColorAdjustments colorAdjustments;



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

            colorAdjustments.colorFilter.value = Color.white;
            vignette.intensity.value = 0;
            vignette.color.value = Color.white;
        }
    }




    public async UniTaskVoid LowerCamera()
    {
        while (true)
        {

            if (_loweredQuantity < _lowerQuantity)
            {
                virtualCamera.position += (Vector3.down + Vector3.back).normalized * _loweringSpeed;

            }
            _loweredQuantity += _loweringSpeed;


            if (_loweredQuantity >= _lowerQuantity)
            {

                _cancelTokenSource.Cancel();
            }

            await UniTask.Yield(cancellationToken: _cancelTokenSource.Token);
        }


    }
    void CancelUniTask()
    {
        _cancelTokenSource.Cancel();
    }

    private async UniTaskVoid PlayInventoryAnim()
    {

        if (isInitalized == false)
        {
            isInitalized = true;
            depthLerp = 0f;
            elapsed = 0f;
        }


        if (tapIsOn == true)
        {
            if (volume.profile.TryGet<DepthOfField>(out depthOfField))
            {
                elapsed = Time.deltaTime;

                while (elapsed < transitionDuration && tapIsOn == true)
                {
                    depthLerp += Time.deltaTime * blurSpeed;

                    // Lerp the color filter value from the initial color to red
                    depthOfField.focusDistance.value = Mathf.Lerp(originalDepthDistance, targetDepthOfDistance, depthLerp);

                    Debug.Log($"depthLerp in PlayInventoryAnim: {depthLerp}");
                    // wait for the next frame
                    await UniTask.NextFrame();
                }

            }
        }


    }

    private bool tapIsOn;

    private bool isInitalized;
    private async UniTaskVoid ShutInventoryAnim()
    {
        if (isInitalized == false)
        {
            isInitalized = true;
            depthLerp = 0f;
            elapsed = 0f;
        }

        if (tapIsOn == false)
        {

            if (volume.profile.TryGet<DepthOfField>(out depthOfField))
            {
                elapsed = Time.deltaTime;

                while (elapsed < transitionDuration)
                {

                    depthLerp += Time.deltaTime * blurSpeed;
                    // Lerp the color filter value from the initial color to red
                    depthOfField.focusDistance.value = Mathf.Lerp(originalDepthDistance, targetDepthOfDistance, 1 - depthLerp);
                    // wait for the next frame
                    await UniTask.NextFrame();
                }

            }
        }

    }
}


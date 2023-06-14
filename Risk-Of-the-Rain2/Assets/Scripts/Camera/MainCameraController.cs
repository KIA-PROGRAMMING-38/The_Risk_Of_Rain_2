using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cysharp.Threading.Tasks;
using System.Threading;
using Cinemachine;

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


    void Update()
    {
        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.Override(1 - Mathf.Lerp(0, 1, Time.time * vignetteDisappearingSpeed));
        }
    }

  
    private async UniTaskVoid BossSpawnEffectOn()
    {
        lerp += Time.deltaTime * exposureChangingSpeed;
        currentExposure = Mathf.Lerp(darkendExposure, originalExposure, 1 - lerp);
      
        colorAdjustments.postExposure.value = currentExposure;

        TurnOnRed();
       
    }


     public float transitionDuration = 2.0f;

    private void TurnOnRed()
    {
        Debug.Log("Changing To Red");
        ColorAdjustments colorAdjustments;

        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            // Get the initial color
            Color initialColor = colorAdjustments.colorFilter.value;

            // We'll transition over the course of [transitionDuration] seconds
            for (float t = 0; t < transitionDuration; t += Time.deltaTime)
            {
                // Lerp the color filter value from the initial color to red
                colorAdjustments.colorFilter.value = Color.Lerp(initialColor, Color.red, t / transitionDuration);
                // wait for the next frame
            }

            // Ensure the color is set to red at the end of the transition
            colorAdjustments.colorFilter.value = Color.red;
        }



    }

  

  
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
   
    public float m_AmplitudeGain;
    public float m_FrequencyGain;

    public float _vibrationDurationTime;

    private async UniTaskVoid VibrateCamera()
    {
        virtualCameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        virtualCameraNoise.m_AmplitudeGain = m_AmplitudeGain;
        virtualCameraNoise.m_FrequencyGain = m_FrequencyGain;

        Debug.Log("impulse!");

        await UniTask.Delay((int)(_vibrationDurationTime * 1000));

        Debug.Log("turnback!");
        virtualCameraNoise.m_AmplitudeGain = 0;
        virtualCameraNoise.m_FrequencyGain = 0;

    }
}

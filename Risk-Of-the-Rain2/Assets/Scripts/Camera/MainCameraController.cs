using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cysharp.Threading.Tasks;
using System.Threading;

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
    public float vignetteDisappearingSpeed = 1f;

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

    private CancellationTokenSource _source;
    public float currentExposure;
    public float originalExposure;
    public float darkendExposure;
    public float lerp;
    public float exposureChangingSpeed;
    private async UniTaskVoid BossSpawnEffectOn()
    {
        lerp += Time.deltaTime * exposureChangingSpeed;
        currentExposure = Mathf.Lerp(darkendExposure, originalExposure, 1 - lerp);
        Debug.Log("boss spawn effect's working");
        colorAdjustments.postExposure.value = currentExposure;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class MainCameraController : MonoBehaviour
{

    Volume volume;
    Vignette vignette;
    public float vignetteDisappearingSpeed = 1f;

    void Start()
    {
        volume = GetComponent<Volume>();
    }

  
    void Update()
    {
        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.Override(1 - Mathf.Lerp(0, 1, Time.time * vignetteDisappearingSpeed));
        }
    }
}

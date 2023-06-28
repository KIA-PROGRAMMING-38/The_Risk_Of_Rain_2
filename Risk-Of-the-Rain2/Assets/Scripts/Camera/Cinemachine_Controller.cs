using Cinemachine;
using UnityEngine;



public class Cinemachine_Controller : MonoBehaviour
{
    [SerializeField]
    Material material;



    public float shoulderOffsetY = 0.0f;
    public float distance = 0.0f;
    public float targetYPosition = 0.0f;
    public float sensitivity;
    public float distanceSensitivity;

    public float MINIMUM_HEIGHT = 0;
    public float MAXIMUM_HEIGHT = 100;

    public float MINIMUM_DISTANCE = 0;
    public float MAXIMUM_DISTANCE = 100;

    public float MINIMUM_Y_POSITION = 0;
    public float MAXIMUM_Y_POSITION = 10;

    

    public static CinemachineVirtualCamera virtualCamera;
    private Cinemachine3rdPersonFollow thirdPersonFollow;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    CinemachineComposer composer;

 




    private void Start()
    {
       
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        thirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        if (!CommandoController.isDead)
        {
            RotateYAxis();
        }
       
        SetShaderParameter();
    }


    /// <summary>
    /// Rotate Y axis using lerp.
    /// Take note that the rate of change is not constant since Lerp2D.EaseOutExpo is involved.
    /// </summary>
  
    float lerp;
    private void RotateYAxis()
    {
        if (thirdPersonFollow != null)
        {
            shoulderOffsetY -= Input.GetAxis("Mouse Y") * sensitivity;
            shoulderOffsetY = Mathf.Clamp(shoulderOffsetY, MINIMUM_HEIGHT, MAXIMUM_HEIGHT);
            // Update the value of Y shoulder offset
            thirdPersonFollow.ShoulderOffset.y = shoulderOffsetY;

            // Use inverseLerp to apply direct lerp value.
            lerp = Mathf.InverseLerp(MINIMUM_HEIGHT, MAXIMUM_HEIGHT, shoulderOffsetY);

            // Update the distance.

            distance = Lerp2D.EaseOutExpo(MINIMUM_DISTANCE, MAXIMUM_DISTANCE, lerp);
            thirdPersonFollow.CameraDistance = distance;


            // / Update the y targetYPosition.
            targetYPosition = Lerp2D.EaseOutExpo(MINIMUM_Y_POSITION, MAXIMUM_Y_POSITION, lerp);
            composer.m_TrackedObjectOffset.y = targetYPosition;

        }

        // Apply any noise or post-processing effects if necessary
        // virtualCameraNoise.m_AmplitudeGain = 0.0f; // Example: Disable camera noise
    }


    public float _originalDitherSize = 1f;

    public float _minimumAlpha = 0.3f;
    public float _defaultAlpha = 0.3f;

    public float _ditherSize = 0.016f;
    public float _initialAlpha = 0.3f;

    public float _alphaChangeTippingPoint = 100f;
    public float _alphaTippingPointFinished = 30f;

    private float distanceLerp;


    private void SetShaderParameter()
    {
        // The alpha has to be lower only when the distance is closer than the tipping point.
        if (thirdPersonFollow.CameraDistance < _alphaChangeTippingPoint)
        {
            ApplyDitherEffect();
            SetAlpha();
        }
        else
        {
            SetDefaultAlpha();
            SetDefaultDither();
        }
    }

 
    
    private void SetAlpha()
    {
        distanceLerp = Mathf.InverseLerp(_alphaChangeTippingPoint, _alphaTippingPointFinished, distance);
        _initialAlpha = Mathf.Lerp(_minimumAlpha, _defaultAlpha, 1 - distanceLerp);
        material.SetFloat("_Opacity", _initialAlpha);
    }

    private void SetDefaultAlpha()
    {
        material.SetFloat("_Opacity", _defaultAlpha);
    }

    private void ApplyDitherEffect()
    {
        material.SetFloat("_Dither_Size", _ditherSize);
    }

    private void SetDefaultDither()
    {
        material.SetFloat("_Dither_Size", _originalDitherSize);
    }
}

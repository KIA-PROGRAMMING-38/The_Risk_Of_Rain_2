using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


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

 



    [SerializeField]
    GameObject _spaceShip;


    private void Start()
    {
       
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        thirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();

     
    }

    private void Update()
    {


       
        RotateYAxis();
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


    public float _shaderOriginalDitherSize = 1f;

    public float SHADER_MINIMUM_OPACITY = 0.3f;
    public float SHADER_ORIGINAL_OPACITY = 0.3f;

    public float _shaderDitherSize = 0.016f;
    public float _shaderOpacity = 0.3f;

    public float _opacityChangeTriggingPoint = 100f;
    public float _opacityChangeTriggingPointEnd = 30f;

    private float distanceLerp;


    /// <summary>
    /// set 
    /// </summary>
    private void SetShaderParameter()
    {
        if (thirdPersonFollow.CameraDistance < _opacityChangeTriggingPoint)
        {
            TurnOnDithering();
            SetOpacity();
        }
        else
        {
            SetOriginalOpacity();
            SetOriginalDither();
        }
    }

    private void SetOpacity()
    {
        distanceLerp = Mathf.InverseLerp(_opacityChangeTriggingPoint, _opacityChangeTriggingPointEnd, distance);
        _shaderOpacity = Mathf.Lerp(SHADER_MINIMUM_OPACITY, SHADER_ORIGINAL_OPACITY, 1 - distanceLerp);
        material.SetFloat("_Opacity", _shaderOpacity);
    }

    private void SetOriginalOpacity()
    {
        material.SetFloat("_Opacity", SHADER_ORIGINAL_OPACITY);
    }

    private void TurnOnDithering()
    {
        material.SetFloat("_Dither_Size", _shaderDitherSize);
    }

    private void SetOriginalDither()
    {
        material.SetFloat("_Dither_Size", _shaderOriginalDitherSize);
    }
}

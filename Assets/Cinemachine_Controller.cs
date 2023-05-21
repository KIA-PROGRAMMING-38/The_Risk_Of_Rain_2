using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class Cinemachine_Controller : MonoBehaviour
{
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



    private CinemachineVirtualCamera virtualCamera;
    private Cinemachine3rdPersonFollow thirdPersonFollow;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    CinemachineComposer composer;

    private void Start()
    {
        // Get the CinemachineVirtualCamera component attached to the virtual camera body
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        // virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        thirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        if (thirdPersonFollow != null)
        {
            shoulderOffsetY -= Input.GetAxis("Mouse Y") * sensitivity;
            shoulderOffsetY = Mathf.Clamp(shoulderOffsetY, MINIMUM_HEIGHT, MAXIMUM_HEIGHT);
            // Update the value of Y shoulder offset
            thirdPersonFollow.ShoulderOffset.y = shoulderOffsetY;

            // Use inverseLerp to apply direct lerp value.
            float lerp = Mathf.InverseLerp(MINIMUM_HEIGHT, MAXIMUM_HEIGHT, shoulderOffsetY);

            // Update the distance.
            Debug.Log($"distance: {distance}");
            distance = Lerp2D.EaseOutExpo(MINIMUM_DISTANCE, MAXIMUM_DISTANCE, lerp);
            thirdPersonFollow.CameraDistance = distance;


            // / Update the y targetYPosition.
            targetYPosition = Lerp2D.EaseOutExpo(MINIMUM_Y_POSITION, MAXIMUM_Y_POSITION, lerp);
            composer.m_TrackedObjectOffset.y = targetYPosition;

        }

        // Apply any noise or post-processing effects if necessary
        // virtualCameraNoise.m_AmplitudeGain = 0.0f; // Example: Disable camera noise
    }
}

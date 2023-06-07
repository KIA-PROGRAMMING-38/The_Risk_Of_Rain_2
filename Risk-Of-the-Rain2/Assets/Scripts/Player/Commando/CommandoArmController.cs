using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommandoArmController : MonoBehaviour
{
    [Serialize] public Transform _leftArm;
    [Serialize] public Transform _rightArm;
    [Serialize] public Transform _virtualCamera;
  
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        Quaternion rotation = Quaternion.Euler(_virtualCamera.eulerAngles.x, _virtualCamera.eulerAngles.y, _virtualCamera.eulerAngles.z);
        _leftArm.rotation = rotation;
        _rightArm.rotation = rotation;
    }
}

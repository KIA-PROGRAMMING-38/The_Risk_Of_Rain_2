using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera_Controller : MonoBehaviour
{
    public Transform target;  // 캐릭터의 Transform 컴포넌트

    public float minDistance = 1f;  // 카메라와 캐릭터 사이의 최소 거리
    public float maxDistance = 10f;  // 카메라와 캐릭터 사이의 최대 거리

    public float minHeight = 1f;  // 최소 카메라 높이
    public float maxHeight = 5f;  // 최대 카메라 높이

    public float sensitivity = 2f;  // 마우스 감도
    private Vector3 offset;  // 초기 거리와 높이에 대한 오프셋
    public float smoothSpeed;


    float mouseX;
    float mouseY;
    void Start()
    {
        offset = new Vector3(0f, maxHeight, -maxDistance);  // 오프셋 초기화
    }

    void Update()
    {
       // 마우스 움직임에 따라 플레이어와 카메라 사이의 거리 및 높이 조정
        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        //거리에 따른 시점 변화
        float distance = offset.magnitude;
        float normalizedY = Mathf.InverseLerp(0f, Screen.height, Input.mousePosition.y);
        float newDistance = Mathf.Lerp(minDistance, maxDistance, 1 - normalizedY);

        Debug.Log(normalizedY);

      //  거리에 따른 높이 변화
        float t = Mathf.InverseLerp(minDistance, maxDistance, newDistance);
        float height = Mathf.Lerp(maxHeight, minHeight, t);




        offset = offset.normalized * newDistance;
        offset.y = height;

       // 목표 위치 계산
       Vector3 targetPosition = target.position + offset;


        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        //카메라 위치 업데이트
        transform.position = smoothedPosition;

      //  카메라가 항상 캐릭터를 향하도록 설정

    }

    private void LateUpdate()
    {
        RotateCamera(mouseX, mouseY);
    }

    private void RotateCamera(float mouseX, float mouseY)
    {
     //   이전 회전 상태 저장
        Quaternion previousRotation = transform.rotation;

       // 카메라의 X, Y 축 회전(마우스 움직임과 반대 방향으로 회전)
        Quaternion cameraRotation = Quaternion.Euler(-mouseY, mouseX, 0f);

       // 카메라의 쿼터니언 회전을 현재 회전 상태에 곱하여 새로운 회전 적용
        transform.rotation = previousRotation * cameraRotation;
    }
}

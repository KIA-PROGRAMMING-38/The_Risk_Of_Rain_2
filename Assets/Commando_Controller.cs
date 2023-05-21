using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Commando_Controller : MonoBehaviour
{
    public float speed = 10f;
    public float sensitivity = 2f;

    private float mouseX;
    private float mouseY;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        Vector3 velocity = movement * speed * Time.deltaTime;
        transform.Translate(velocity);

        RotatePlayer();
    }

    private void RotatePlayer()
    {
        // 플레이어의 Y축 회전
        transform.Rotate(Vector3.up, mouseX);
    }
}

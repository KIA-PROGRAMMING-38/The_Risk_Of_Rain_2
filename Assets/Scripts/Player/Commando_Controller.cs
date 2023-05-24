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

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {

        MovePlayer();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        // 플레이어의 Y축 회전
        transform.Rotate(Vector3.up, mouseX);
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;


        if (moveX == 0 && moveZ == 0)
        {
          
            animator.SetBool(AnimID.RUN, false);
        }

        else
        {
            
            animator.SetBool(AnimID.RUN, true);
        }

        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        Vector3 velocity = movement * speed * Time.deltaTime;
        transform.Translate(velocity);


    }

}

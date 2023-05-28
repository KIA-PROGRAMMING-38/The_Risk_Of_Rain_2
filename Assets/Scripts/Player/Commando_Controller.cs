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
    float moveX;
    float moveZ;

    private Animator animator;
    private Rigidbody rigidbody;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //Get inputs.
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        animator.SetFloat(AnimID.EULER_X, moveX);
        animator.SetFloat(AnimID.EULER_Y, moveZ);


        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;


        MovePlayer();
        RotatePlayer();
        JumpPlayer();
    }

    private void RotatePlayer()
    {
        // 플레이어의 Y축 회전
        transform.Rotate(Vector3.up, mouseX);

        
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        Vector3 velocity = movement * speed * Time.deltaTime;
        transform.Translate(velocity);

        animator.SetFloat(AnimID.MOVE_X, moveX);
        animator.SetFloat(AnimID.MOVE_Y, moveZ);
    }

    bool isJumping = false;
    public float jumpForce;
    private void JumpPlayer()
    {
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            animator.SetBool(AnimID.IS_JUMPING, true);
            rigidbody.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagID.TERRAIN))
        {
            animator.SetBool(AnimID.IS_JUMPING, false);
            isJumping = false;
        }
    }


}

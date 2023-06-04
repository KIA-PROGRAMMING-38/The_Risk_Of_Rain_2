using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private RectTransform reticle; // The RecTransform of reticle UI element.

    public float restingSize;
    public float maxSize;
    public float speed;
    private float currentSize;

   
    private void Start()
    {

        reticle = GetComponent<RectTransform>();

    }

    private void Update()
    {

        // Check if player is currently moving and Lerp currentSize to the appropriate value.
        if (Input.GetKey(KeyCode.Mouse0) && isMoving)
        {
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
        }

        // Set the reticle's size to the currentSize value.
        reticle.sizeDelta = new Vector2(currentSize, currentSize);

    }


    // Bool to check if player is currently moving.
    bool isMoving
    {

        get
        {
           
            if (
                Input.GetAxis("Horizontal") != 0 ||
                Input.GetAxis("Vertical") != 0 ||
                Input.GetAxis("Mouse X") != 0 ||
                Input.GetAxis("Mouse Y") != 0
                    )
                return true;
            else
                return false;

        }

    }


    // Bool to check if player is currently moving.

}

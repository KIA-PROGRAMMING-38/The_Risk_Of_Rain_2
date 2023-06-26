using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI _timeTextWholeNumber;

    [SerializeField]
    public TextMeshProUGUI _timeTextDecimal;

    public static bool IsBossSpawned;
    public static bool IsGameStarted;
    public static bool IsPlayStarted;
    public static bool IsPlayerArrived;
    public static bool StartAnimation;
    public static float playTime;

    private double timeInSeconds;
    private float gameStarttime;

    private void Start()
    {
       
    }
    private void Update()
    {
        if (IsMouseOverGameWindow())
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reloads the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
       

        if (IsGameStarted == true)
        {
            timeInSeconds = Time.time - gameStarttime;
            int minutes = (int)(timeInSeconds / 60);
            int seconds = (int)timeInSeconds % 60;
            double fractionalSeconds = timeInSeconds - seconds;
            _timeTextWholeNumber.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            _timeTextDecimal.text = string.Format(".{0:00}", Math.Abs(fractionalSeconds * 100));
        }
        else
        {
            gameStarttime = Time.time;
        }
    }

   
    bool IsMouseOverGameWindow()
    {
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x < 0 || mousePos.y < 0 || mousePos.x > Screen.width || mousePos.y > Screen.height)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}



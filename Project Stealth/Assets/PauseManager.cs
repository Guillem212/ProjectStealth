using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject endMenuUI;

    public static bool gameIsPaused;
    public static bool gameIsFinished;

    // Start is called before the first frame update
    void Start()
    {
        Resume();
        gameIsFinished = false;
        PlayerMovement.allowedToMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && !gameIsFinished)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EndScreen()
    {
        endMenuUI.SetActive(true);
        Time.timeScale = 0f;        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void callEndCoroutine()
    {
        StartCoroutine("launchEndScreen");
    }

    IEnumerator launchEndScreen()
    {
        gameIsFinished = true;
        yield return new WaitForSeconds(1.5f);
        PlayerMovement.allowedToMove = false;
        yield return new WaitForSeconds(4f);
        EndScreen();
    }
}

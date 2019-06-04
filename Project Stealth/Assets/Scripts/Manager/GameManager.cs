using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /*
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Toggle AO = GameObject.Find("ambient oclussion").GetComponent<Toggle>();
            AO.isOn = FindObjectOfType<SettingsManager>().ambientOclussionSetting;
            Toggle MB = GameObject.Find("motion blur").GetComponent<Toggle>();
            MB.isOn = FindObjectOfType<SettingsManager>().motionBlurSetting;
            Toggle B = GameObject.Find("bloom").GetComponent<Toggle>();
            B.isOn = FindObjectOfType<SettingsManager>().bloomSetting;
            //GameObject.Find("motion blur").GetComponent<Toggle>().isOn = FindObjectOfType<SettingsManager>().motionBlurSetting;
            //GameObject.Find("bloom").GetComponent<Toggle>().isOn = FindObjectOfType<SettingsManager>().bloomSetting;
            AO.onValueChanged.AddListener(delegate {
                ToggleValueChanged(AO);
            });
            MB.onValueChanged.AddListener(delegate {
                ToggleValueChanged(MB);
            });
            B.onValueChanged.AddListener(delegate {
                ToggleValueChanged(B);
            });
        }
    }*/

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void LoadScene (int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static int GetCurrentScene() { return SceneManager.GetActiveScene().buildIndex; }
}

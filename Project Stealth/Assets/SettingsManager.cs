using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public bool bloomSetting;
    public bool motionBlurSetting;
    public bool ambientOclussionSetting;

    public static SettingsManager instance;

    //private GameObject instance;
    // Start is called before the first frame update
    void Awake()
    {
        bloomSetting = true;
        motionBlurSetting = true;
        ambientOclussionSetting = true;

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
            /*if(instance != null)
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);*/
        }

    public void setBloom(bool value) { bloomSetting = value; }
    public void setMB(bool value) { motionBlurSetting = value; }
    public void setAO(bool value) { ambientOclussionSetting = value; }


}

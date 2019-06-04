using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public bool bloomSetting = true;
    public bool motionBlurSetting = true;
    public bool ambientOclussionSetting = true;
    
    public void setBloom(bool value) { bloomSetting = value; }
    public void setMB(bool value) { motionBlurSetting = value; }
    public void setAO(bool value) { ambientOclussionSetting = value; }


}

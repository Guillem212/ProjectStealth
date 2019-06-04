using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class PPEffects : MonoBehaviour
{

    PostProcessVolume volume;
    ChromaticAberration chromaticAberrationLayer = null;
    LensDistortion lensDistortionLayer = null;
    MotionBlur motionBlurLayer = null;
    DepthOfField depthOfFieldLayer = null;
    //PostProcessProfile profile;

    //Bloom bloomLayer = null;
    //ColorGrading colorGradingLayer = null;
    AmbientOcclusion ambientOcclusionLayer = null;
    Vignette vignetteLayer = null;
    ColorGrading colorGradingLayer = null;
    Bloom bloomLayer = null;

    SettingsManager setting;    
    
    bool vignetteState = false;
    bool hookingState = false;

    bool usingCamera = false;

    // Start is called before the first frame update
    void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        setting = GameObject.Find("SoundAndSettingsManager").GetComponent<SettingsManager>();

        volume.profile.TryGetSettings<AmbientOcclusion>(out ambientOcclusionLayer);
        volume.profile.TryGetSettings<Vignette>(out vignetteLayer);
        volume.profile.TryGetSettings<ChromaticAberration>(out chromaticAberrationLayer);
        volume.profile.TryGetSettings<LensDistortion>(out lensDistortionLayer);
        volume.profile.TryGetSettings<MotionBlur>(out motionBlurLayer);        
        volume.profile.TryGetSettings<DepthOfField>(out depthOfFieldLayer);
        volume.profile.TryGetSettings<ColorGrading>(out colorGradingLayer);
        volume.profile.TryGetSettings<Bloom>(out bloomLayer);

        motionBlurLayer.enabled.value = setting.motionBlurSetting;
        ambientOcclusionLayer.enabled.value = setting.ambientOclussionSetting;
        bloomLayer.enabled.value = setting.bloomSetting;

        chromaticAberrationLayer.enabled.value = true;
        vignetteLayer.enabled.value = false;
        lensDistortionLayer.enabled.value = false;
        depthOfFieldLayer.enabled.value = false;        

    }    

    // Update is called once per frame
    void Update()
    {        
        if (vignetteState && vignetteLayer.intensity.value < 0.3f)
        {
            vignetteLayer.intensity.value += 0.05f;
        }                
        if (!vignetteState)
        {
            if (vignetteLayer.intensity.value > 0f)
                vignetteLayer.intensity.value -= 0.05f;
            else vignetteLayer.enabled.value = false;
        }

        /*if (hookingState && lensDistortionLayer.intensity.value < 34.5f)
        {
            lensDistortionLayer.intensity.value += 5f;
        }
        if (!hookingState && lensDistortionLayer.intensity.value > 0f)
        {            
            lensDistortionLayer.intensity.value -= 3f;
            if (lensDistortionLayer.intensity.value < 0) lensDistortionLayer.intensity.value = 0f;
        }*/
    }

    public void SetVignetteLayer(bool state) //viñetado para cuando el personaje se agache
    {
        if (state)
        {
            vignetteState = true;
            vignetteLayer.enabled.value = true;
        }
        else
        {
            vignetteState = false;
        }
        
    }

    public void HookingEffect(bool state) //efecto de distorsion del gancho
    {
        hookingState = state;
        depthOfFieldLayer.enabled.value = state;
        lensDistortionLayer.enabled.value = state;
        if (state)
        {
            lensDistortionLayer.intensity.value = 50f;
        }
        else
        {
            lensDistortionLayer.intensity.value = 0f;
        }
    }

    public void LenDistortion(bool state)
    {
        //colorGradingLayer.enabled.value = state;
        lensDistortionLayer.enabled.value = state;
        if (state)
        {
            usingCamera = true;
            lensDistortionLayer.intensity.value = 40f;
            colorGradingLayer.saturation.value = -100f;
        }
        else
        {
            usingCamera = false;
            lensDistortionLayer.intensity.value = 0f;
        }
    }

    public void SetOverLife(float lifeAmountNormalized)
    {        
        if (!usingCamera)
        {
            chromaticAberrationLayer.intensity.value = 1 - lifeAmountNormalized;
            colorGradingLayer.saturation.value = -100 + lifeAmountNormalized * 100;
        }                
    }    
}

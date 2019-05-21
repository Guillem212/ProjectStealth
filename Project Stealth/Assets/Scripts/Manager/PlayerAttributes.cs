using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour    
{
    private Image lifeBarImage;
    //private Image soundBarImage;
    private RawImage soundBarImage;
    private RectTransform barMaskRectTransform;
    private float barMaskWidth;

    public Life life;
    [SerializeField] Transform lifeBar;
    [SerializeField] Transform soundBar;
    [SerializeField] Transform soundBarMask;
    PPEffects postProcess;
    [SerializeField] GameObject PP;
    float sound;

    public float values = 0.5f;

    private void Awake()
    {
        lifeBarImage = lifeBar.GetComponent<Image>();
        soundBarImage = soundBar.GetComponent<RawImage>();
        barMaskRectTransform = soundBarMask.GetComponent<RectTransform>();
        barMaskWidth = barMaskRectTransform.sizeDelta.x;

        life = new Life();
        postProcess = PP.GetComponent<PPEffects>();
    }

    private void Update()
    {
        life.Update();
        lifeBarImage.fillAmount = life.GetLifeNormalized();
        Rect uvRect = soundBarImage.uvRect;
        uvRect.x -= 0.3f * Time.deltaTime;
        soundBarImage.uvRect = uvRect;

        if (Input.GetKeyDown(KeyCode.P))
        {
            life.TrySpendLife(30);
        }
        postProcess.SetOverLife(life.GetLifeNormalized());
        sound = StealthBehaviour.amountOfSound;
        //soundBarImage.fillAmount = sound / 20f;
        float fixedSound = Mathf.Clamp(sound, 0f, barMaskWidth);
        Vector2 barMaskSizeDelta = barMaskRectTransform.sizeDelta;        
        //barMaskSizeDelta.x = fixedSound * barMaskWidth;
        barMaskSizeDelta.x = Mathf.Clamp(sound / 15f * barMaskWidth, 0f, barMaskWidth);
        barMaskRectTransform.sizeDelta = barMaskSizeDelta;
        
    }


    public class Life
    {
        public const int LIFE_MAX = 100;

        public float lifeAmount;
        private float lifeRegenAmount;
        private float lifeRegenDelay = 0f;
        private float lifeRegenDelayOriginalValue = 2f;

        public Life()
        {
            lifeAmount = LIFE_MAX;
            lifeRegenAmount = 10f;
        }

        public void Update()
        {           
            if (lifeRegenDelay <= 0f)
            {
                lifeAmount += lifeRegenAmount * Time.deltaTime;
            }
            else
            {
                lifeRegenDelay -= Time.deltaTime;
            }            
            lifeAmount = Mathf.Clamp(lifeAmount, 0f, LIFE_MAX);
        }

        public void TrySpendLife(float amount)
        {
            if (lifeAmount >= amount) lifeAmount -= amount;
            lifeRegenDelay = lifeRegenDelayOriginalValue;
        }

        public float GetLifeNormalized()
        {
            return lifeAmount / LIFE_MAX;
        }
    }
}

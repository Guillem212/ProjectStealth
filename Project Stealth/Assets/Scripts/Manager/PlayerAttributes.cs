using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour    
{
    private Image lifeBarImage;
    private Image soundBarImage;
    private Life life;
    [SerializeField] Transform lifeBar;
    [SerializeField] Transform soundBar;
    PPEffects postProcess;
    [SerializeField] GameObject PP;
    float sound;

    private void Awake()
    {
        lifeBarImage = lifeBar.GetComponent<Image>();
        soundBarImage = soundBar.GetComponent<Image>();

        life = new Life();
        postProcess = PP.GetComponent<PPEffects>();
    }

    private void Update()
    {
        life.Update();
        lifeBarImage.fillAmount = life.GetLifeNormalized();

        if (Input.GetKeyDown(KeyCode.P))
        {
            life.TrySpendLife(30);
        }
        postProcess.SetOverLife(life.GetLifeNormalized());
        sound = StealthBehaviour.amountOfSound;
        soundBarImage.fillAmount = sound / 20f;
    }


    public class Life
    {
        public const int LIFE_MAX = 100;

        public float lifeAmount;
        private float lifeRegenAmount;

        public Life()
        {
            lifeAmount = LIFE_MAX;
            lifeRegenAmount = 10f;
        }

        public void Update()
        {            
            lifeAmount += lifeRegenAmount * Time.deltaTime;
            lifeAmount = Mathf.Clamp(lifeAmount, 0f, LIFE_MAX);
        }

        public void TrySpendLife(float amount)
        {
            if (lifeAmount >= amount) lifeAmount -= amount;
        }

        public float GetLifeNormalized()
        {
            return lifeAmount / LIFE_MAX;
        }
    }
}

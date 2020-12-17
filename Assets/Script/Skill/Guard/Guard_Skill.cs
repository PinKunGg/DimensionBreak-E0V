using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guard_Skill : MonoBehaviour
{
    #region private variable
    private float GuardMaxValue;
    private bool isRegen, isPlayGuardBreakSound, isPlayGuardCreateSound;
    #endregion

    #region SerializeField variable
    [SerializeField] private float RegenRate = 0.01f;
    [SerializeField] private Slider GuardBar;
    [SerializeField] private SkillManager SkillManager;
    [SerializeField] private Character_SCAO Character_SCAO;
    [SerializeField] private GameObject HitParticle, GuardObj, DamagePopCanvas, DamagePopUp;
    [SerializeField] private Image colorBG,colorFill;
    #endregion

    #region Class property
    public Slider GetGuardBar
    {
        get
        {
            return GuardBar;
        }
    }
    #endregion
    
    //Prepare all setting
    private void Start()
    {
        colorBG.CrossFadeAlpha(0f,0f,false);
        colorFill.CrossFadeAlpha(0f,0f,false);

        GuardObj.SetActive(false);
        this.GetComponent<BoxCollider2D>().enabled = false;
        
        GuardMaxValue = Character_SCAO.MaxHp / 2f;
        GuardBar.maxValue = GuardMaxValue;
        GuardBar.value = GuardMaxValue;
    }
    void Update()
    {
        if(GuardBar.value >= GuardMaxValue)
        {
            isRegen = false;
            CancelInvoke();

            colorBG.CrossFadeAlpha(0f,0.3f,false);
            colorFill.CrossFadeAlpha(0f,0.3f,false);
        }
        else if(GuardBar.value < GuardMaxValue && GuardBar.value > 0)
        {
            colorBG.CrossFadeAlpha(1f,0f,false);
            colorFill.CrossFadeAlpha(1f,0f,false);
        }
        else if(GuardBar.value <= 0)
        {
            SkillManager.GuardSkillDeActive();
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void GuardAvtive()
    {
        GuardCancleRegen();
        this.GetComponent<BoxCollider2D>().enabled = true;
        GuardObj.SetActive(true);
        isPlayGuardBreakSound = false;

        if(isPlayGuardCreateSound == false)
        {
            isPlayGuardCreateSound = true;
            SoundManager.instance.PlaySelectAudio(3);
        }
    }
    public void GuardDeActive()
    {
        GuardObj.SetActive(false);
        isPlayGuardCreateSound = false;

        if(isPlayGuardBreakSound == false && Input.GetButton("Fire2"))
        {
            isPlayGuardBreakSound = true;
            SoundManager.instance.PlaySelectAudio(2);
        }
    }
    public void GuardRegenManager()
    {
        GuardObj.SetActive(false);
        this.GetComponent<BoxCollider2D>().enabled = false;

        if(isRegen == false)
        {
            isRegen = true;
            InvokeRepeating("GuardRegen",0.1f,0.01f);
        }
    }
    void GuardRegen()
    {
        GuardRegenCal(RegenRate);
    }
    public void GuardCal(float value)
    {
        SoundManager.instance.PlaySelectAudio(1);
        CameraShake.instance.ShakeCam(1f,0.1f);
        value = Mathf.Ceil(value);

        GuardBar.value -= value;

        float randY = Random.Range(1f,1.3f);
        float randX = Random.Range(-0.3f,0.4f);

        GameObject DamagePop = Instantiate(DamagePopUp);
        Vector2 SpawnPos = new Vector2(transform.position.x + randX, transform.position.y + randY);
        DamagePop.transform.SetParent(DamagePopCanvas.transform,false);
        DamagePop.transform.position = SpawnPos;
        DamagePop.GetComponent<DamagePopUp>().SetDamagePopUp(value);
    }
    void GuardRegenCal(float value)
    {
        GuardBar.value += value;
    }
    public void GuardCancleRegen()
    {
        isRegen = false;
        CancelInvoke();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("EnemyAttack"))
        {
            Vector2 point = other.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);

            GameObject HitPar = Instantiate(HitParticle,point,Quaternion.Euler(-90f,0f,0f));
            Destroy(HitPar,1f);
        }
    }
}

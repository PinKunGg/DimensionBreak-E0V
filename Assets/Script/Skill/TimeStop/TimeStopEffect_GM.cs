using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeStopEffect_GM : MonoBehaviour
{
    #region private variable
    private Volume Vp;
    private ColorAdjustments Ca;
    private ChromaticAberration Cha;
    private Vignette Vig;
    #endregion

    #region SerializeField variable
    [SerializeField] private ParticleSystem[] ParticleSys;
    #endregion
    
    //Get all component
    void Awake()
    {
        Vp = GameObject.Find("PostProcessing").GetComponent<Volume>();
    }

    //Prepare all setting
    void Start()
    {
        Vp.profile.TryGet(out Ca);
        Vp.profile.TryGet(out Cha);
        Vp.profile.TryGet(out Vig);
        Ca.saturation.value = 0f;
        Cha.intensity.value = 0f;
        Vig.intensity.value = 0.3f;
    }

    private void Update()
    {
        if(TimeManager.GetTimeStop() == false && (Ca.saturation.value != 0f && Cha.intensity.value != 0f && Vig.intensity.value != 0.3f))
        {
            Ca.saturation.value = 0f;
            Cha.intensity.value = 0f;
            Vig.intensity.value = 0.3f;
        }    
    }

    public IEnumerator StopTimeGM()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var items in ParticleSys)
        {
            items.Pause(true);
        }
        
        
        Ca.saturation.value = -70f;
        Cha.intensity.value = 1f;
        Vig.intensity.value = 0.4f;
    }
    public IEnumerator CountinueTimeGM()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var items in ParticleSys)
        {
            items.Play(true);
        }

        Ca.saturation.value = 0f;
        Cha.intensity.value = 0f;
        Vig.intensity.value = 0.3f;
    }
}

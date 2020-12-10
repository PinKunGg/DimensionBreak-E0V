using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    #region private variable
    private static bool isGuardActive;
    #endregion

    #region SerializeField variable
    [SerializeField] private Guard_Skill Guard_Skill;
    #endregion

    #region Class Property
    public static bool GetGuardActive
    {
        get
        {
            return isGuardActive;
        }
    }
    #endregion
    
    public void GuardSkill()
    {
        if(Guard_Skill.GetGuardBar.value > 0 && isGuardActive == false)
        {
            isGuardActive = true;
            Guard_Skill.GuardAvtive();
        }
    }
    public void GuardSkillDeActive()
    {
        Guard_Skill.GuardDeActive();
        isGuardActive = false;
    }
    public void GuardRegen()
    {
        Guard_Skill.GuardRegenManager();
    }
}

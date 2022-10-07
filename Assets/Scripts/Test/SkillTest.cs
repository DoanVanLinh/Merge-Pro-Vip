using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillTest : MonoBehaviour
{

    public SkillType skillType;
    public UnitTest unitInfo;
    public float timeExcute;
    public abstract void ExcuteSkill();

}
public enum SkillType
{
    Active,
    Passive
}
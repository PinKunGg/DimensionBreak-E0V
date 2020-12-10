using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "DimensionBreak/Character/Enemy")]
public class Enemy_SCAO : ScriptableObject
{
    public string Name = "New Enemy";
    public bool FlyAble;
    public int Index = 0;
    public float MaxHp = 100f;
    public float DefValue = 100f;
    public float ProjectileDef = 100f;
    public float MeleeDef = 100f;
    public float Damage = 10f;
    public float AttackDis = 2f;
    public float DetectRange = 10f;
    public float RetreatDis = 5f;
    public float speed = 10f;
    public float JumpForce = 10f;
}
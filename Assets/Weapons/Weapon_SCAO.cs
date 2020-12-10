using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "DimensionBreak_Items/Weapons")]
public class Weapon_SCAO : ScriptableObject
{
    public string Name = "New Item";
    public string ObjectType = "Close_Range";
    public int WeaponIndex = 0;
    public float Damage = 5f;
    public float FixBullet = 3f;
    public float Knoback = 10f;
    public bool DualWeapon;
    public Sprite icon = null;
    public Sprite icon_Use = null;
    public Sprite icon_NotUse = null;
    public GameObject Obj_Prefab, Drop_Prefab;
}

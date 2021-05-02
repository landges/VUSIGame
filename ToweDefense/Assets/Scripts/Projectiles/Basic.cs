using UnityEngine;

public enum projecttileType
{
    rock,arrow,fireball
};
public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    int attackDamage;
    [SerializeField]
    projecttileType pType;

    public int AttackDamage
    {
        get
        {
            return attackDamage;
        }
        set
        {
            attackDamage=value;
        }
    }
    public projecttileType PType
    {
        get
        {
            return pType;
        }
    }
}

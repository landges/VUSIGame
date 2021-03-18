using UnityEngine;

public enum projecttileType
{
    rock,arrow,fireball
};
public class ProjectTile : MonoBehaviour
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
    }
    public projecttileType PType
    {
        get
        {
            return pType;
        }
    }
}

using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    TowerControl towerObject;
    [SerializeField]
    int towerPrice;
    public TowerControl TowerObject
    {
        get
        {
            return towerObject;
        }
    }
    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
    }

}

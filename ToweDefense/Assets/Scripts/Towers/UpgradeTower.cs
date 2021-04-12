using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTower
{
    public int Price{get; private set;}
    public int Damage{get; private set;}
    public float AttackRadius{get; private set;}
    public float RotationSpeed{get; private set;}

    public UpgradeTower(int price, int damage, float attackRadius, float rotationSpeed)
    {
        Price=price;
        Damage=damage;
        AttackRadius=attackRadius;
        RotationSpeed=rotationSpeed;
    }
    // example for new type of tower
    // public UpgradeTower(int price;int damage)
    // {
    //     this.Price=price;
    //     this.Damage=damage;
    // }
}

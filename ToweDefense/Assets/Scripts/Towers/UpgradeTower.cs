using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTower
{
    public int Price{get; private set;}
    public int Damage{get; private set;}
	public float AttackSpeed { get; private set; }
	public float AttackRadius{get; private set;}
    public float RotationSpeed{get; private set;}
	public float ExplosionRange { get; private set; }
	public float ChargeSpeed { get; private set; }
	public float CastDuration { get; private set; }
	public UpgradeTower(int price, int damage, float attackRadius, float rotationSpeed, float attackSpeed)
    {
        Price=price;
        Damage=damage;
        AttackRadius=attackRadius;
        RotationSpeed=rotationSpeed;
		AttackSpeed = attackSpeed;

	}
	public UpgradeTower(int price, int damage, float attackRadius, float rotationSpeed, float explosionRange, float attackSpeed)
	{
		Price = price;
		Damage = damage;
		AttackSpeed = attackSpeed;
		AttackRadius = attackRadius;
		RotationSpeed = rotationSpeed;
		ExplosionRange = explosionRange;
	}
	public UpgradeTower(float chargeSpeed, int price, int damage, float attackRadius, float rotationSpeed,  float castDuration)
     {
		Price=price;
        Damage=damage;
		ChargeSpeed = chargeSpeed;
		AttackRadius = attackRadius;
		RotationSpeed = rotationSpeed;
		CastDuration = castDuration;
     }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerControl
{
	public LineRenderer lineRenderer;
	const float lMultiplier = 20f;
	// Start is called before the first frame update
	new void Start()
	{
		Upgrades = new UpgradeTower[]
		{
			new UpgradeTower(price: 20, damage: 1, attackRadius:.5f, rotationSpeed:0.1f, chargeSpeed: 0.001f, castDuration: 0.005f),
			new UpgradeTower(price: 20, damage: 1, attackRadius:.5f, rotationSpeed:0.1f, chargeSpeed: 0.001f, castDuration: 0.005f),
			new UpgradeTower(price: 20, damage: 1, attackRadius:.5f, rotationSpeed:0.1f, chargeSpeed: 0.001f, castDuration: 0.005f),
		};
	}

	// Update is called once per frame
	public override void Attack()
	{
		hasTurned = false;
		isAttacking = false;
		if (GetNearestEnemy() != null)
		{
			Debug.Log("LASERRR");
			lineRenderer.positionCount=2;
			Vector3 difference = targetEnemy.transform.position - transform.position;
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, targetEnemy.transform.position + difference*lMultiplier);
		}
	}
	
	public override string GetStats()
	{
		if (NextUpgrade != null)
		{
			return string.Format("{0} \nRotation Speed: {1} + <color=#00ff00ff> +{2}</color>", base.GetStats(), rotationSpeed, NextUpgrade.RotationSpeed);
		}
		return string.Format("{0} \nRotation Speed: {1}", base.GetStats(), rotationSpeed);
	}
	public override void Upgrade()
	{
		//over specific Upgrades
		timeBetweenAttacks -= NextUpgrade.ChargeSpeed;
		castDuration += NextUpgrade.CastDuration;
		base.Upgrade();
	}
}

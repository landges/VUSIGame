using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : TowerControl
{
    new void Start()
    {
        Upgrades=new UpgradeTower[]
		{
			new UpgradeTower(price: 20, damage: 1, attackRadius: .5f, rotationSpeed: 0.1f, attackSpeed: 0.005f),
			new UpgradeTower(price: 30, damage: 1, attackRadius: .5f, rotationSpeed: 0.1f, attackSpeed: 0.005f),
			new UpgradeTower(price: 40, damage: 1, attackRadius: .5f, rotationSpeed: 0.1f, attackSpeed: 0.005f),
            new UpgradeTower(price: 40, damage: 1, attackRadius: .5f, rotationSpeed: 0.1f, attackSpeed: 0.005f),
		};
    }
    public override string GetStats()
	{
		if(NextUpgrade!=null)
		{
			return string.Format("{0} \nRotation Speed: {1} + <color=#00ff00ff> +{2}</color>", base.GetStats(),rotationSpeed,NextUpgrade.RotationSpeed);
		}
		return string.Format("{0} \nRotation Speed: {1}", base.GetStats(),rotationSpeed);
	}
    public override void Upgrade()
    {
		//over specific Upgrades
		timeBetweenAttacks -= NextUpgrade.AttackSpeed;
		base.Upgrade();
    }
}

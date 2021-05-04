using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerTower : TowerControl
{
	new void Start()
    {
        Upgrades=new UpgradeTower[]
		{
			new UpgradeTower(20,1,.5f,0.1f),
			new UpgradeTower(30,1,.5f,0.1f),
			new UpgradeTower(40,1,.5f,0.1f),
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
        base.Upgrade();
    }
}

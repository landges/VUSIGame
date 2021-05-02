using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : TowerControl
{
    void Start()
    {
        Upgrades=new UpgradeTower[]
		{
			new UpgradeTower(20,1,.5f,0.1f),
			new UpgradeTower(30,1,.5f,0.1f),
			new UpgradeTower(40,1,.5f,0.1f),
            new UpgradeTower(40,1,.5f,0.1f),
		};
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : TowerControl
{
	// Start is called before the first frame update
	public void Attack()
	{
		isAttacking = false;
		hasTurned = false;
		if (GetNearestEnemy() != null)
		{
			//anim.Play("Shoot", layer: 0);
			
			//
			//ADD AUDIO
			//Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Sniper);
		}
	}
}

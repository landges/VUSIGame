using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerControl
{
	public LineRenderer lineRenderer;
	BoxCollider2D lineCollider;
	const float lMultiplier = 20f;
	private float castCounter=0;
	RaycastHit hit;
	
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
	protected override void Init()
	{
		lineCollider = transform.Find("LaserBeam").GetComponent<BoxCollider2D>();
		base.Init();
	}
	// Update is called once per frame
	void Update()
	{
		if (isAttacking && castCounter > 0)
		{
			castCounter -= Time.deltaTime;

			// deal damage
		}
		//stop lasering
		else if (isAttacking && castCounter <= 0)
		{
			isAttacking = false;
			attackCounter = timeBetweenAttacks;
			lineRenderer.positionCount = 0;
			StartCoroutine(RotateTower());
			lineCollider.transform.position = transform.position;
			lineCollider.size = new Vector2(0f, 0f);
		}
		//if not casting
		else
		{
			attackCounter -= Time.deltaTime;
			if (targetEnemy == null || targetEnemy.IsDead)
			{
				Enemy nearestEnemy = GetNearestEnemy();
				if (nearestEnemy != null)
				{
					targetEnemy = nearestEnemy;
				}
				else
				{
					isAttacking = false;
					hasTurned = false;
				}
			}
			else
			{
				StartCoroutine(RotateTower());
				if (attackCounter <= 0 && hasTurned) //hasTurned
				{
					isAttacking = true;
					castCounter = castDuration;
					lineRenderer.positionCount = 2;
					Vector3 difference = targetEnemy.transform.position - transform.position;
					lineRenderer.SetPosition(0, transform.position + difference.normalized/3);
					lineRenderer.SetPosition(1, targetEnemy.transform.position + difference * lMultiplier);
					float lineLength = Vector2.Distance(transform.position, lineRenderer.GetPosition(1));
					float lineWidth = lineRenderer.endWidth*1.5f;
					lineCollider.transform.position = (transform.position + lineRenderer.GetPosition(1)) / 2;
					lineCollider.size = new Vector3(lineWidth, lineLength, 1f);
					var dir = targetEnemy.transform.localPosition - transform.localPosition;
					var targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg-90f;
					lineCollider.transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
				}
				else
				{
					isAttacking = false;
					hasTurned = false;
				}
				if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
				{
					targetEnemy = null;
					hasTurned = false;
				}
			}
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("hitTrigger");
		Debug.Log(collision.gameObject.GetComponents<Enemy>());
		foreach (Enemy newE in collision.gameObject.GetComponents<Enemy>())
		{
			newE.EnemyHit(Damage);
		}
	}
	private void OnCollisionStay(Collision collisionInfo)
	{
		Debug.Log("hit");
		foreach (Enemy newE in collisionInfo.gameObject.GetComponents<Enemy>())
		{
			newE.EnemyHit(Damage);
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

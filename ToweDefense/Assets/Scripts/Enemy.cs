using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	[SerializeField]
	float speed = 1;
	[SerializeField]
	float navigation;
	[SerializeField]
	public int startingHealth;
	[SerializeField]
	int revertAmount;

	GameObject exit;
	GameObject[] wayPoints;
	Transform enemy;
	Collider2D enemyCollider;
	Animator anim;
	int target = 0;
	float navigationTime = 0;
	bool hasCome = false;

	public int health;
	public float x_offset;
	public float y_offset;
	public bool IsDead { get; private set; } = false;
	// Start is called before the first frame update
	void Start()
	{
		wayPoints = GameObject.FindGameObjectsWithTag("MovingPoint");
		exit = GameObject.FindWithTag("Finish");
		enemy = GetComponent<Transform>();
		enemyCollider = GetComponent<Collider2D>();
		anim = GetComponent<Animator>();
		//Manager.Instance.RegisterEnemy(this);
	}

	// Update is called once per frame
	void Update()
	{
		if (wayPoints != null && IsDead == false)
		{
			navigationTime += Time.deltaTime;
			if (navigationTime > navigation)
			{
				if (target < wayPoints.Length)
				{
					var newpos = wayPoints[target].transform.position;
					var prevpos = enemy.position;
					newpos.x += x_offset;
					newpos.y += y_offset;
					enemy.position = Vector2.MoveTowards(enemy.position, newpos, speed * navigationTime);
					if (Vector3.Distance(enemy.position, prevpos) ==0f)
					{
						target += 1;
					}
				}
				else
				{
					enemy.position = Vector2.MoveTowards(enemy.position, exit.GetComponent<CircleCollider2D>().ClosestPoint(enemy.position), speed * navigationTime);
				}
				navigationTime = 0;
			}
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Projectile")
		{
			Projectile newP = collision.gameObject.GetComponent<Projectile>();
			EnemyHit(newP.AttackDamage);
			Destroy(collision.gameObject);
		}

		else if (collision.tag == "Finish")
		{
			Manager.Instance.Health -= 1;
			Manager.Instance.UnregisterEnemy(this);
			Manager.Instance.IsWaveOver();
		}
	}
	public void EnemyHit(int hitpoints)
	{
		if (health - hitpoints > 0)
		{
			health -= hitpoints;
			//hurt
			Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Hit);
			anim.Play("Hurt");
		}
		else
		{
			anim.SetTrigger("didDie");
			//dying
			Die();			
		}
	}
	public void Die()
	{
		IsDead = true;
		enemyCollider.enabled = false;
		Manager.Instance.TotalKilled += 1;
		Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Death);
		Manager.Instance.AddMoney(revertAmount);
		Manager.Instance.EnemyList.Remove(this);
		Destroy(enemy.gameObject,anim.GetCurrentAnimatorStateInfo(0).length);
		Manager.Instance.IsWaveOver();
	}
}

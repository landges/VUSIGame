using UnityEngine;

public enum projecttileType
{
    rock,arrow,fireball
};
public class Projectile : MonoBehaviour
{
    
    [SerializeField]
    int attackDamage;
	[SerializeField]
	float speed = 5f;
	[SerializeField]
	float explosionRange = 0f;
	[SerializeField]
    projecttileType pType;
	public int AttackDamage { get; set; }
	public projecttileType PType { get; }
	//Collider2D projectileCollider;
	Animator anim;
	bool isExploding = false;
	private Enemy target;
	public void Seek(Enemy _target)
	{
		target = _target;
	}
	private void Start()
	{
		anim = GetComponent<Animator>();
	}
	void Update()
	{

		if (target == null)
		{
			Destroy(gameObject);
			return;
		}
		var dir = target.transform.localPosition - transform.localPosition;
		float distanceThisFrame = speed * Time.deltaTime;
		var targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.localPosition = Vector2.MoveTowards(transform.localPosition,
			target.transform.localPosition, distanceThisFrame);
		transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);

	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			isExploding = true;
			if (explosionRange == 0f)
			{
				Enemy newE = collision.gameObject.GetComponent<Enemy>();
				newE.EnemyHit(AttackDamage);
				Destroy(gameObject);
			}
			else
			{
				anim.Play("Boom", layer: 0);
				Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRange);
				Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
				foreach (Collider2D collider in colliders)
				{
					if (collider.tag == "Enemy")
					{
						Enemy newE = collision.gameObject.GetComponent<Enemy>();
						newE.EnemyHit(AttackDamage);
					}
				}
			}
		}
	}

}

using UnityEngine;

public enum projecttileType
{
    rock,arrow,fireball
};
public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    int attackDamage;
	[SerializeField]
	float explosionRange = 0f;
	[SerializeField]
    projecttileType pType;
	//Collider2D projectileCollider;
	Animator anim;
	private void Start()
	{
		anim = GetComponent<Animator>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			if (explosionRange == 0f)
			{
				Enemy newE = collision.gameObject.GetComponent<Enemy>();
				newE.EnemyHit(AttackDamage);
				Destroy(gameObject);
			}
			else
			{
				anim.SetTrigger("Explode");
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
	public int AttackDamage
    {
        get
        {
            return attackDamage;
        }
        set
        {
            attackDamage=value;
        }
    }
    public projecttileType PType
    {
        get
        {
            return pType;
        }
    }
}

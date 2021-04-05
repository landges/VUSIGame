using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttacks;
	//degrees per second
	[SerializeField]
	public float rotationSpeed;
	[SerializeField]
    public float attackRadius;
    [SerializeField]
    public Projectile projectile;
    [SerializeField]
    public int sellPrice { get; set; }
    Enemy targetEnemy = null;
    float attackCounter;
	bool hasTurned = false;
    bool isAttacking = false;
    private SpriteRenderer rangeSpriteRenderer;
    // Start is called before the first frame update
    void Init(){
        rangeSpriteRenderer=this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        rangeSpriteRenderer.transform.localScale=new Vector3(this.attackRadius*2f,this.attackRadius*2f,1);
    }
    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;
        if(targetEnemy == null || targetEnemy.IsDead)
        {
            Enemy nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null)
            {
                targetEnemy = nearestEnemy;
            }
        }
        else
        {
			StartCoroutine(RotateTower());
			if (attackCounter <= 0 && hasTurned) //hasTurned
			{
				isAttacking = true;
				attackCounter = timeBetweenAttacks;
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
    public void FixedUpdate()
    {
        if (isAttacking == true)
        {
			hasTurned = false;
			Attack();
		}
    }
	private IEnumerator RotateTower()
	{
		while (!hasTurned && targetEnemy!=null)
		{
			var dir = targetEnemy.transform.localPosition - transform.localPosition;
			var targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle),
				rotationSpeed * Time.deltaTime);
			if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, targetAngle)) < 3f)
			{
				hasTurned = true;
			}
				
			yield return null;
		}
	}
	public void Attack()
    {
        isAttacking = false;
		if (GetNearestEnemy() != null)
		{
			Projectile newProjectTile = Instantiate(projectile) as Projectile;
			newProjectTile.transform.localPosition = transform.localPosition;
			if (newProjectTile.PType == projecttileType.arrow)
			{
				Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Arrow);
			}
			else if (newProjectTile.PType == projecttileType.fireball)
			{
				Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Fireball);
			}
			else if (newProjectTile.PType == projecttileType.rock)
			{
				Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Rock);
			}
			if (targetEnemy == null)
			{
				Destroy(newProjectTile.gameObject);
			}
			else
			{
				//move  projectile to enemy
				StartCoroutine(MoveProjectTile(newProjectTile));
			}
		}
    }
    IEnumerator MoveProjectTile(Projectile projectile)
    {
        while ((projectile != null && targetEnemy != null) && (GetTargetDistance(targetEnemy)>0.20f||GetProjectileDistance(projectile)>0f))
        {
			var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
			projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);

			yield return null;
        }
		if (targetEnemy == null && projectile!=null)
		{
			while (projectile != null) {
				targetEnemy = GetNearestEnemy(inRange: false);
				if (targetEnemy == null)
				{
					Destroy(projectile.gameObject);
				}
				else
				{
					var dir = targetEnemy.transform.localPosition - projectile.transform.localPosition;
					var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
					projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
					projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
					
				}
				yield return null;
			}
				
		}
		if (projectile != null)
		{
			Destroy(projectile.gameObject);
		}
		yield return null;

	}
    private float GetTargetDistance(Enemy thisEnemy)
    {
        if (thisEnemy == null)
        {
            thisEnemy = GetNearestEnemy();
            if(thisEnemy == null)
            {
                return float.PositiveInfinity;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }
	private float GetProjectileDistance(Projectile thisProjectile)
	{
		if (thisProjectile == null)
		{
			return 0f;
		}
		return Mathf.Abs(Vector2.Distance(transform.localPosition, thisProjectile.transform.localPosition));
	}
	private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach (Enemy enemy in Manager.Instance.EnemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    private Enemy GetNearestEnemy(bool inRange=true)
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
		List<Enemy> enemiesToLook = new List<Enemy>();
		if (inRange)
			enemiesToLook = GetEnemiesInRange();
		else
			enemiesToLook = Manager.Instance.EnemyList;
		foreach (Enemy enemy in enemiesToLook)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }
    public void EnableRange(){
        if(rangeSpriteRenderer == null){
            Init();
        }
        rangeSpriteRenderer.enabled=true;
    }
    public void DisableRange(){
        rangeSpriteRenderer.enabled=false;
    }
}

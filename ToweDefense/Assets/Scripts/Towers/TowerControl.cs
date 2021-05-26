using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerControl : MonoBehaviour
{
	public int Level{get;set;}=1;
	[SerializeField]
	public int Damage;
    [SerializeField]
    public float timeBetweenAttacks;
	//degrees per second
	[SerializeField]
	public float rotationSpeed;
	[SerializeField]
    public float attackRadius;
	[SerializeField]
	public float castDuration;
	[SerializeField]
    public Projectile projectile;
    [SerializeField]
    public int sellPrice { get; set; }

	public UpgradeTower[] Upgrades{get; protected set;}

	public UpgradeTower NextUpgrade
	{
		get
		{
			if(Upgrades.Length > Level-1)
			{
				return Upgrades[Level-1];
			}
			return null;
		}
	}
    protected Enemy targetEnemy = null;
    float attackCounter;
	protected bool hasTurned = false;
    protected bool isAttacking = false;
    private SpriteRenderer rangeSpriteRenderer;

	public void Start()
	{
		//anim.Play("Boom", layer: 0);
		Upgrades =new UpgradeTower[]
		{
			new UpgradeTower(20,1,.5f,0.1f, 0.005f),
		};
	}
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
	public virtual void Attack()
    {
        isAttacking = false;
		hasTurned = false;
		if (GetNearestEnemy() != null)
		{
			Projectile newProjectile = Instantiate(projectile) as Projectile;
			newProjectile.Seek(targetEnemy);
			newProjectile.AttackDamage=Damage;
			newProjectile.transform.localPosition = transform.localPosition;
			if (newProjectile.PType == projecttileType.arrow)
			{
				Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Arrow);
			}
			else if (newProjectile.PType == projecttileType.fireball)
			{
				Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Fireball);
			}
			else if (newProjectile.PType == projecttileType.rock)
			{
				Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Rock);
			}
		}
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
    protected Enemy GetNearestEnemy(bool inRange=true)
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
	public virtual string GetStats()
	{
		if(NextUpgrade!=null)
		{
			return string.Format("\nLevel: {0} \nDamage: {1} <color=#00ff00ff> +{3}</color> \nRadius: {2} <color=#00ff00ff> +{4}</color>", Level,Damage,attackRadius,NextUpgrade.Damage,NextUpgrade.AttackRadius);
		}
		return string.Format("\nLevel: {0} \nDamage: {1} \nRadius: {2}", Level,Damage,attackRadius);
	}
	public virtual void Upgrade()
	{
		Manager.Instance.TotalMoney-=NextUpgrade.Price;
		sellPrice+=NextUpgrade.Price/2;
		Damage+=NextUpgrade.Damage;
		attackRadius+=NextUpgrade.AttackRadius;
		rotationSpeed+=NextUpgrade.RotationSpeed;
		Init();
		Level+=1;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttacks;
    [SerializeField]
    float attackRadius;
    [SerializeField]
    Projectile projectile;
    Enemy targetEnemy = null;
    float attackCounter;
    bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;
        if(targetEnemy == null || targetEnemy.IsDead)
        {
            Enemy nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <=attackRadius)
            {
                targetEnemy = nearestEnemy;
            }
        }
        else
        {
            if (attackCounter <= 0)
            {
                isAttacking = true;
                attackCounter = timeBetweenAttacks;
            }
            else
            {
                isAttacking = false;
            }
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            {
                targetEnemy = null;
            }
        }
    }
    public void FixedUpdate()
    {
        if (isAttacking == true)
        {
            Attack();
        }
    }
    public void Attack()
    {
        isAttacking = false;
        Projectile newProjectTile = Instantiate(projectile) as Projectile;
        newProjectTile.transform.localPosition = transform.localPosition;
        if (newProjectTile.PType == projecttileType.arrow)
        {
            Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Arrow);
        }
        else if(newProjectTile.PType == projecttileType.fireball)
        {
            Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Fireball);
        }
        else if(newProjectTile.PType == projecttileType.rock)
        {
            Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Rock);
        }
        if (targetEnemy == null)
        {
            Destroy(newProjectTile);
        }
        else
        {
            //move  projecttile to enemy
            StartCoroutine(MoveProjectTile(newProjectTile));
        }
    }
    IEnumerator MoveProjectTile(Projectile projectile)
    {
        while (GetTargetDistance(targetEnemy)>0.20f && projectile != null && targetEnemy!=null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }
        if (projectile != null || targetEnemy != null)
        {
            Destroy(projectile);
        }
    }
    private float GetTargetDistance(Enemy thisEnemy)
    {
        if (thisEnemy == null)
        {
            thisEnemy = GetNearestEnemy();
            if(thisEnemy == null)
            {
                return 0f;
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
    private Enemy GetNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach(Enemy enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }
}

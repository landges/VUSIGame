﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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
	Transform enemy;
	Collider2D enemyCollider;
	Animator anim;
	int target = 0;
	float navigationTime = 0f;
	bool hasCome = false;

	public float health;
	public Slider healthSlider;
	public Gradient healthGradient;
	public Image fill;

	public float x_offset;
	public float y_offset;
	public bool IsDead { get; private set; } = false;

	private Stack<Node> path;
	public Point GridPosition{get;set;}
	private Vector3 destination;
	// Start is called before the first frame update
	void Start()
	{
		exit = GameObject.FindWithTag("Finish");
		enemy = GetComponent<Transform>();
		enemyCollider = GetComponent<Collider2D>();
		anim = GetComponent<Animator>();
		healthSlider.maxValue = health;
		healthSlider.value = health;
		fill.color = healthGradient.Evaluate(1f);
		//Manager.Instance.RegisterEnemy(this);
		SetPath(ManagerScene.Instance.Path);
	}

	// Update is called once per frame
	void Update()
	{
		Move();
	}
	private void Move()
	{
		navigationTime += Time.deltaTime;

		transform.position=Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
		if(transform.position==destination)
		{
			if(path!=null && path.Count>0)
			{
				GridPosition=path.Peek().GridPosition;
				destination=path.Pop().WorldPosition;
				destination.x += x_offset;
				destination.y += y_offset;
			}
		}
	
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Finish")
		{
            Manager.Instance.Health -= 1;
            Manager.Instance.UnregisterEnemy(this);
			Manager.Instance.IsWaveOver();
		}
	}
	private void OnTriggerStay2D(Collider2D collisionInfo)
	{
		foreach (BoxCollider2D col in collisionInfo.gameObject.GetComponents<BoxCollider2D>())
		{
			if (col.gameObject.tag == "LaserBeam")
			{
				LaserTower lt = col.gameObject.GetComponentInParent<LaserTower>();
				EnemyHit(lt.Damage*Time.deltaTime);
			}
		}
	}
	public void EnemyHit(float hitpoints)
	{
		if (health - hitpoints > 0)
		{
			//change color
			health -= hitpoints;
			healthSlider.value = health;
			fill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
			//hurt
			Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Hit);
			anim.Play("Hurt");
		}
		else
		{
			health = 0;
			healthSlider.value = health;
			fill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
			anim.SetTrigger("Explode");
			//dying
			Die();			
		}
	}
	public void Die()
	{
		IsDead = true;
		enemyCollider.enabled = false;
		Manager.Instance.TotalKilled += 1;
		Manager.Instance.Score += 1;
		Manager.Instance.AddMoney(revertAmount);
		Manager.Instance.EnemyList.Remove(this);
		Destroy(enemy.gameObject,anim.GetCurrentAnimatorStateInfo(0).length);
		Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.Death);
		Manager.Instance.IsWaveOver();
	}

	private void SetPath(Stack<Node> newPath)
	{
		if(newPath !=null)
		{
			this.path=newPath;
			GridPosition=path.Peek().GridPosition;
			destination=path.Pop().WorldPosition;
		}
	}
}

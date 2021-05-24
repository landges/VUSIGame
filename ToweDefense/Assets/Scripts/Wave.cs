using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public int IndexEnemy{get;set;}
    public float SpawnDelay{get;set;}
    public int EnemiesPerSpawn{get;set;}
    public int TotalEnemies{get;set;}

    public Wave(int indexEnemy,float spawnDelay,int enemiesPerSpawn,int totalEnemies)
    {
        IndexEnemy=indexEnemy;
        SpawnDelay=spawnDelay;
        EnemiesPerSpawn=enemiesPerSpawn;
        TotalEnemies=totalEnemies;
    }
}

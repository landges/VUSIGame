using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next,play,gameover,win
}

public class Manager : Loader<Manager>
{
    [SerializeField]
    int totalWaves = 10;
    [SerializeField]
    Text totalMoneyLabel;
    [SerializeField]
    Text currentWave;
    [SerializeField]
    Text totalEscapedLabel;
    [SerializeField]
    Text playBtnLabel;
    [SerializeField]
    Button playBtn;
    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    Enemy[] enemies;
    [SerializeField]
    int totalEnemies=5;
    [SerializeField]
    int enemiesPerSpawn;

    int waveNumber = 0;
    int totalMoney = 50;
    int totalEscaped = 0;
    int roundEscaped = 0;
    int totalKilled = 0;
    int whichEnemyToSpawn = 0;
    int enemiesToSpawn = 0;
    gameStatus currentState = gameStatus.play;
    AudioSource audioSource;
    public List<Enemy> EnemyList = new List<Enemy>();

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }
    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }
    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }
    const float spawnDelay = 0.5f;

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = totalMoney.ToString();
        }
    }
    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        ShowMenu();
    }
    private void Update()
    {
        HandleEscape();
    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0,enemiesToSpawn)]) as Enemy;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void DestroyEnemies()
    {
        foreach (Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }
    public void addMoney(int amount)
    {
        TotalMoney += amount;
    }
    public void subtractMoney(int amount)
    {
        TotalMoney -= amount;
    }
    public void IsWaveOver()
    {
        totalEscapedLabel.text = "Escaped " + TotalEscaped + "/ 10";
        if ((RoundEscaped+TotalKilled)==totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            SetCurrentGameState();
            ShowMenu();
        }
    }
    public void SetCurrentGameState()
    {
        if (totalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
        }
        else if(waveNumber==0 && (RoundEscaped + TotalKilled) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }
    public void PlayBtnPressed()
    {
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 1;
                totalEscaped = 0;
                totalMoney = 50;
                enemiesToSpawn = 0;
                TowerManager.Instance.DestroyAllTower();
                TowerManager.Instance.RenameTagBuildSite();
                totalMoneyLabel.text = TotalMoney.ToString();
                totalEscapedLabel.text = "Escaped " + TotalEscaped + "/ 10";
                audioSource.PlayOneShot(SoundManager.Instance.Newgame);
                break;

        }
        DestroyEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWave.text = "Wave " + (waveNumber + 1);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }
    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLabel.text = "Play Again?";
                AudioSource.PlayOneShot(SoundManager.Instance.Gameover);
                break;

            case gameStatus.next:
                playBtnLabel.text = "Next Wave?";
                break;

            case gameStatus.play:
                playBtnLabel.text = "Play game";
                break;

            case gameStatus.win:
                playBtnLabel.text = "Play game";
                break;

        }
        playBtn.gameObject.SetActive(true);
    }
    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDrag();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
}

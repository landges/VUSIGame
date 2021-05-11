using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using System.IO;

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
	Text healthLabel;
	[SerializeField]
    Text playBtnLabel;
    [SerializeField]
    Text ScoreLabel;
    [SerializeField]
    Button playBtn;
    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    Enemy[] enemies;
    [SerializeField]
    int totalEnemies;
    [SerializeField]
    int enemiesPerSpawn;

    [SerializeField]
    GameObject WavesInfoPanel;

    int waveNumber = 0;
    int totalMoney = 280;
    int enemiesToSpawn = 0;
    gameStatus currentState = gameStatus.play;
	public List<Enemy> EnemyList = new List<Enemy>();
	public List<GameObject> wayPoints;
	public int TotalHealth { get; set;} = 20;
	public int health;
	public int TotalKilled { get; set; } = 0;
	const float spawnDelay = 0.5f;
    public int Score { get; set; } = 0;
    public int MainScore { get; set; } = 0;
    public bool gameOver = false;
    string path;
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject WinMenu;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            healthLabel.text = health.ToString();
        }
    }
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
	public AudioSource AudioSrc { get; private set; }
	// Start is called before the first frame update
	void Start()
    {
        path = Application.dataPath + "/Saves/SavedData/score.xml";
        if (File.Exists(path))
            MainScore = Serializer.DeXml(path);
        //IComparer<GameObject> wpc = new IComparer<GameObject>() { };
        wayPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("MovingPoint"));
		wayPoints.Sort(SortByName);
		Health = TotalHealth;
        totalMoneyLabel.text=TotalMoney.ToString();
        healthLabel.text=TotalHealth.ToString();
        playBtn.gameObject.SetActive(false);
		AudioSrc = GetComponent<AudioSource>();
        ShowMenu();
    }
    private void Update()
    {
        HandleEscape();
    }

	private static int SortByName(GameObject o1, GameObject o2)
	{
		return o1.name.CompareTo(o2.name);
	}

	IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
					var center = spawnPoint.transform.position;
					var size  = spawnPoint.GetComponent<BoxCollider2D>().size;
					Enemy newEnemy = Instantiate(enemies[Random.Range(0,enemiesToSpawn)]) as Enemy;
                    // Enemy newEnemy = Instantiate(enemies[1]) as Enemy;
					//need balancing
					newEnemy.health = (int)(newEnemy.startingHealth * (1 + (float)waveNumber/10));
					newEnemy.x_offset = Random.Range(-size.x/2, size.x/2);
					newEnemy.y_offset = Random.Range(-size.y / 2, size.y/2);
					Vector2 pos = new Vector3(center.x + newEnemy.x_offset, center.y + newEnemy.y_offset);
					newEnemy.transform.position = pos;
					RegisterEnemy(newEnemy);
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
    public void AddMoney(int amount)
    {
        TotalMoney += amount;
    }
    public void SubtractMoney(int amount)
    {
        TotalMoney -= amount;
    }
    public void IsWaveOver()
    {
		healthLabel.text = Health.ToString();
        if ((TotalHealth-Health+TotalKilled)>=totalEnemies)
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
        if (Health==0)
        {
            this.Health = 0;
            playBtn.interactable=false;
            currentState = gameStatus.gameover;
            GameOver();
        }
        else if(waveNumber==0 && (TotalHealth-Health + TotalKilled) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves && EnemyList.Count == 0)
        {
            currentState = gameStatus.win;
            playBtn.interactable=false;
            WinGame();
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
                Health = TotalHealth;
                totalMoney = TotalMoney;
                enemiesToSpawn = 0;
				totalMoneyLabel.text = TotalMoney.ToString();
                healthLabel.text = TotalHealth.ToString();
				AudioSrc.PlayOneShot(SoundManager.Instance.Newgame);				
				break;
        }
		DestroyEnemies();
        TotalKilled = 0;
        currentWave.text = "Wave " + (waveNumber + 1);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
		
    }
    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtn.interactable=false;
                playBtnLabel.text = "Play Again?";
                AudioSrc.PlayOneShot(SoundManager.Instance.Gameover);
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
            // TowerManager.Instance.DisableDrag();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
    public void WinGame()
    {
        if(currentState == gameStatus.win)
        {
            Time.timeScale = 0f;
            WinMenu.SetActive(true);
            // Временная заглушка для подсчета итоговых очков
            Score=Score+Health+TotalMoney;
            MainScore = MainScore + Score;
            ScoreLabel.text="Score: "+ Score.ToString();
            Serializer.SaveXml(MainScore, path);
        }
    }
    public void GameOver()
    {
        if (currentState == gameStatus.gameover)
        {
            MainScore = MainScore + Score;
            Serializer.SaveXml(MainScore, path);
            Time.timeScale = 0f;
            gameOverMenu.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void Quit(int _sceneNumber)
    {
        SceneManager.LoadScene(_sceneNumber);
    }
    public void ShowWavesInfo()
    {
        // TowerManager.Instance.SelectTile();
        TowerManager.Instance.towerPanel.gameObject.SetActive(true);
        TowerManager.Instance.DisableChilds();
        WavesInfoPanel.SetActive(true);
    }
}

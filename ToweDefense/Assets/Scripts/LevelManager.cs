using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Assets.Scripts;

public class LevelManager : MonoBehaviour
{
    int levelUnLock;
    public Button[] buttons;
    public float scoreMain { get; set;} = 0;
    public float score { get; set; } = 0;
    [SerializeField]
    Text MoneyLabel;
    [SerializeField]
    GameObject SelectLvl;

    public void loadLevel(int levelIndex)
    {
        PlayerPrefs.SetInt("levelIndex", levelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        var listLvl = GameObject.Find("SelectLvl");
        Debug.Log(listLvl);
        MoneyLabel = GameObject.Find("MoneyLabel").GetComponent<Text>();
        string datapath = Application.dataPath + "/Saves/SavedData/score.xml";
        if (PlayerPrefs.HasKey("MoneyScore"))
        {
            scoreMain = PlayerPrefs.GetFloat("MoneyScore");
            if (scoreMain != 0)
            {
                MoneyLabel.text = MoneyLabel.text.Remove(MoneyLabel.text.Length - 1) + scoreMain.ToString();
            }
            
        }
        //if (File.Exists(datapath))
        //{
        //    scoreMain = Serializer.DeXml(datapath);
        //    ScoreLabel.text = ScoreLabel.text.Remove(ScoreLabel.text.Length - 1) + scoreMain.ToString();
        //}

        //IComparer<GameObject> wpc = new IComparer<GameObject>() { };

    }

    public void LoadScore()
    {
        var listLvl = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ground")); ;
        // wayPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("MovingPoint"));
        Debug.Log(listLvl.Count);
        if (PlayerPrefs.HasKey("MoneyScore"))
        {
            for (int i = 0; i < listLvl.Count; i++)
            {

                var textElem = listLvl[i].GetComponentInChildren<Text>();
                if (PlayerPrefs.HasKey("Level" + (i+1).ToString()))
                {
                    var scoreStr = PlayerPrefs.GetInt("Score").ToString();
                    if (textElem.text.Length == 1)
                    {
                        textElem.text = textElem.text + ": " + scoreStr;
                    }
                       
                }
                Debug.Log(textElem.text);
            }

            scoreMain = PlayerPrefs.GetFloat("MoneyScore");
            if (scoreMain != 0)
            {
                MoneyLabel.text = MoneyLabel.text.Remove(MoneyLabel.text.Length - 1) + scoreMain.ToString();
            }

        }
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}

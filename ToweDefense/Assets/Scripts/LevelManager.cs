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
        if (PlayerPrefs.HasKey("MoneyScore"))
        {
            scoreMain = PlayerPrefs.GetFloat("MoneyScore");
            if (scoreMain != 0)
            {
                MoneyLabel.text = MoneyLabel.text.Remove(MoneyLabel.text.Length - 1) + scoreMain.ToString();
            }   
        }

    }

    public void LoadScore()
    {
        var listLvl = new List<GameObject>(GameObject.FindGameObjectsWithTag("Level")); ;
        if (PlayerPrefs.HasKey("MoneyScore"))
        {
            for (int i = 0; i < listLvl.Count; i++)
            {

                var textElem = listLvl[i].GetComponentInChildren<Text>();
                if (PlayerPrefs.HasKey("Score_" + (i+1).ToString()))
                {
                    var scoreStr = PlayerPrefs.GetInt("Score_" + (i + 1).ToString()).ToString();
                    if (textElem.text.Length == 1)
                    {
                        textElem.text = textElem.text + ": " + scoreStr;
                    }

                }
                Debug.Log(textElem.text);
            }
        }
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}

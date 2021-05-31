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
    [SerializeField]
    Button levelBtnPrefab;
    [SerializeField]
    Transform levelList;

    
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        MoneyLabel = GameObject.Find("MoneyLabel").GetComponent<Text>();
        if (PlayerPrefs.HasKey("MoneyScore"))
        {
            scoreMain = PlayerPrefs.GetFloat("MoneyScore");
            if (scoreMain != 0)
            {
                MoneyLabel.text = MoneyLabel.text.Remove(MoneyLabel.text.Length - 1) + scoreMain.ToString();
            }   
        }
        SetLevelList();
    }
    public void loadLevel(int levelIndex)
    {
        Debug.Log(levelIndex);
        PlayerPrefs.SetInt("levelIndex", levelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
    public void SetLevelList()
    {
        Object[] worlds = Resources.LoadAll("levels", typeof(TextAsset));
        Sprite[] levelImages = Resources.LoadAll<Sprite>("ImagesLevel");
        for(int j=0;j<worlds.Length;j++)
        {
            int param=j+1;
            Button levelBtn=Instantiate(levelBtnPrefab);
            levelBtn.gameObject.transform.GetChild(1).GetComponent<Image>().sprite=levelImages[j];
            levelBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text=(j+1).ToString();
            levelBtn.gameObject.transform.SetParent(levelList);
            levelBtn.gameObject.transform.localScale=new Vector3(1.0f,1.0f,1.0f);
            levelBtn.onClick.AddListener(() => loadLevel(param));
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
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

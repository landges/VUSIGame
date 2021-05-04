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
    public int scoreMain { get; set;} = 0;
    [SerializeField]
    Text ScoreLabel;

    public void loadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    void Start()
    {
        string datapath = Application.dataPath + "/Saves/SavedData/score.xml";
        if (File.Exists(datapath))
        {
            scoreMain = Serializer.DeXml(datapath);
            ScoreLabel.text = ScoreLabel.text.Remove(ScoreLabel.text.Length - 1) + scoreMain.ToString();
        }
            
        //IComparer<GameObject> wpc = new IComparer<GameObject>() { };

    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}

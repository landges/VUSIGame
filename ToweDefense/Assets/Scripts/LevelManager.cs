using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    int levelUnLock;
    public Button[] buttons;

    public void loadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}

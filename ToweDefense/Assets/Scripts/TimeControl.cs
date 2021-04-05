using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    public Text speed;
    public void Speed()
    {
        if (speed.text == "1x")
        {
            Time.timeScale = 1.5f;
            speed.text = "1,5x";
        }
        else if (speed.text == "1,5x")
        {
            Time.timeScale = 2f;
            speed.text = "2x";
        }
        else if (speed.text == "2x")
        {
            Time.timeScale = 1f;
            speed.text = "1x";
        }
    }
}

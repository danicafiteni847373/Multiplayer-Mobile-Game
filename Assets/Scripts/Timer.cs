using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(remainingTime / 60);
            float seconds = Mathf.FloorToInt(remainingTime % 60);

            // Use string.Format to format the minutes and seconds with leading zeros
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            // If the remaining time is less than or equal to 0, change the scene
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        // You can change "YourNextSceneName" to the actual name of your next scene
        SceneManager.LoadScene(3);
    }

}

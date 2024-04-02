using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public void restartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HomePage()
    {
        SceneManager.LoadScene(0);
    }

}

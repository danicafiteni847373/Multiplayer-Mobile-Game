using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public TextMeshProUGUI player1WINS;
    public TextMeshProUGUI player2WINS;
    public TextMeshProUGUI itsaTie;

    void Start()
    {
        Debug.Log("HighscoreManager Start method called.");

       

        // Retrieve scores from PlayerPrefs
        int Score = PlayerPrefs.HasKey("Score") ? PlayerPrefs.GetInt("Score") : 0;
        int Score2 = PlayerPrefs.HasKey("Score2") ? PlayerPrefs.GetInt("Score2") : 0;

        // Display scores on the UI
        Debug.Log("Player 1 Score: " + Score);
        Debug.Log("Player 2 Score: " + Score2);


        // Display scores on the UI or perform other actions
        player1ScoreText.text = "Player 1 Score: " + Score;
        player2ScoreText.text = "Player 2 Score: " + Score2;

        // Determine the winner
        int highestScore = Mathf.Max(Score, Score2);

        if (highestScore == Score)
        {
            player1WINS.text = "Player 1 Wins!";
        }
        else if (highestScore == Score2)
        {
            player2WINS.text = "Player 2 Wins!";
        }
        else
        {
            itsaTie.text = "It's a Tie!";
        }
    }
}

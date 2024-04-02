using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviourPun
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;

    [SerializeField]
    public int score;
    [SerializeField]
    public int score2;

    private PhotonView photonView;

    public void Start()
    {
        Debug.Log("Starting");
        photonView = GetComponent<PhotonView>();
        UpdateScoreText();
        UpdateScoreText2();
    }

    //score

    [PunRPC]
    public void IncreaseScore(int amount)
    {
        if (photonView != null && photonView.IsMine)
        {
            UpdatePlayer1Score(amount);
            Debug.Log("player1 is mine");
        }
        else
        {
            Debug.Log("error");
        }
    }

    [PunRPC]
    public void IncreaseScore2(int amount2)
    {
        // No need to check photonView.IsMine here, as this method is an RPC already
        UpdateOtherPlayersScore(amount2);
        Debug.Log("player2 is mine");
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Player 1 score: " + score.ToString();
    }

    private void UpdatePlayer1Score(int amount)
    {
        score += amount;
        Debug.Log("Player 1 score increased by " + amount);
        UpdateScoreText();
        Debug.Log(score);
    }

    private void UpdateScoreText2()
    {
        scoreText2.text = "Player 2 score: " + score2.ToString();
    }

    private void UpdateOtherPlayersScore(int amount2)
    {
        score2 += amount2;
        scoreText2.text = "Character_02: " + score2;
        // Your logic to update Other players' scores
        Debug.Log("Player 2' score increased by " + amount2);
        UpdateScoreText2();
    }
}

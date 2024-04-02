using UnityEngine;
using TMPro;
using Photon.Pun;

public class Score : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI scoreText;
    private int score = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("coins") && photonView.IsMine)
        {
            // Increase the score on the local player
            photonView.RPC("IncreaseScore", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void IncreaseScore()
    {
        // Increase the score and update the text on all clients
        score++;
        scoreText.text = score.ToString();
        Debug.Log("++++++++++++++++++1");
    }
}


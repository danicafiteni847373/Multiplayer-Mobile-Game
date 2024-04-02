using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

[RequireComponent(typeof(PhotonView))]

public class Scoring : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;
    // Start is called before the first frame update
    public void PlayerScored()
    {
        if (photonView.IsMine)
        {
            Debug.Log("PlayerScored called on player " + PhotonNetwork.LocalPlayer.ActorNumber);
            // Increase the score on the local player
            photonView.RPC("IncreaseScore", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void IncreaseScore()
    {
        Debug.Log("IncreaseScore called on player " + PhotonNetwork.LocalPlayer.ActorNumber);
        // Increase the score and update the text on all clients
        score += 1;
        scoreText.text = score.ToString();
    }
}

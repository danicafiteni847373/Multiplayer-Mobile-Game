using UnityEngine;
using Photon.Pun;

public class EnemyAttack : MonoBehaviourPun
{
    private PhotonView photonView;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider has the "coins" tag
        if (other.CompareTag("enemies"))
        {
            // Find the GameManager in the scene
            GameManager gameManager = FindObjectOfType<GameManager>();

            // Check if GameManager is found
            if (gameManager != null)
            {
                // Check if the photonView is mine
                if (photonView.IsMine)
                {
                    // Use the photonView.RPC method directly on the current object
                    if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    {
                        // Use RpcTarget.All to ensure all clients receive the RPC call
                        gameManager.photonView.RPC("UpdateHealth", RpcTarget.All, 1);
                        Debug.Log("damage");

                    }
                    else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
                    {
                        // Use RpcTarget.All to ensure all clients receive the RPC call
                        gameManager.photonView.RPC("UpdateHealth2", RpcTarget.All, 1);
                        Debug.Log("damage");
                    }

                    if (gameManager.CurrentHealth <= 0)
                    {
                        Debug.Log("GameManager's currentHealth is 0. Destroying the player.");
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
        }
    }
}

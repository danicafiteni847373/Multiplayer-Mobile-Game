using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyImportantObject : MonoBehaviourPun
{
    private PhotonView photonView;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("treasures"))
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
                        gameManager.photonView.RPC("IncreaseScore", RpcTarget.All, 5);
                        Debug.Log("Before Destroy");

                        // Check if the current client is the owner before destroying
                        if (other.gameObject.GetPhotonView().IsMine)
                        {
                            PhotonNetwork.Destroy(other.gameObject); // Use PhotonNetwork.Destroy to properly handle networked object destruction
                        }

                        Debug.Log("After Destroy");
                    }
                    else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
                    {
                        // Use RpcTarget.All to ensure all clients receive the RPC call
                        gameManager.photonView.RPC("IncreaseScore2", RpcTarget.All, 5);
                        Debug.Log("Before Destroy");

                        // Check if the current client is the owner before destroying
                        if (other.gameObject.GetPhotonView().IsMine)
                        {
                            PhotonNetwork.Destroy(gameObject); // Use PhotonNetwork.Destroy to properly handle networked object destruction
                        }

                        Debug.Log("After Destroy");
                    }
                }
            }
        }

    }
}

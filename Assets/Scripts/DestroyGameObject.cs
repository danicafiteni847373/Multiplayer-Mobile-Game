using UnityEngine;
using Photon.Pun;

public class DestroyGameObject : MonoBehaviourPun
{
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("coins"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();

            if (gameManager != null && photonView.IsMine)
            {
                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

                // Use RpcTarget.All to ensure all clients receive the RPC call
                if (actorNumber == 1)
                {
                    gameManager.photonView.RPC("IncreaseScore", RpcTarget.All, 1);
                }
                else if (actorNumber == 2)
                {
                    gameManager.photonView.RPC("IncreaseScore2", RpcTarget.All, 1);
                }

                // Check if the current client is the owner before destroying
                if (other.gameObject.GetPhotonView().IsMine)
                {
                    PhotonNetwork.Destroy(other.gameObject);
                }
            }
        }
    }
}


using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private PhotonView photonView;
    public void OnPhotonSerializeView(GameObject _prefab, PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);
    }
}

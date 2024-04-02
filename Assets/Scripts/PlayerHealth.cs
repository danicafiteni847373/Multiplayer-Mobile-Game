using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviourPun
{


    [SerializeField]
    private int currentHealth = 3;
    public TextMeshProUGUI healthText;

    [SerializeField]
    private int currentHealth2 = 3;
    public TextMeshProUGUI healthText2;

    new private PhotonView photonView;

    public void Start()
    {
        //healthText = GetComponent<TextMeshProUGUI>();
        //healthText2 = GetComponent<TextMeshProUGUI>();

        Debug.Log("start method is called");
        photonView = GetComponent<PhotonView>();

        UpdateHealthText();
        UpdateHealthText2();

    }



    // HEALTH




    //PLAYER 1
    [PunRPC]
    public void UpdateHealth(int damage)
    {
        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                ApplyDamageRPC(damage);
                Debug.Log("Health: " + currentHealth);
            }
            else
            {
                Debug.Log("UpdateHealth called on non-local player!");
            }
        }
        else
        {
            Debug.Log("PhotonView is null!");
        }
    }




    private void UpdateHealthText()
    {
        print("Test0" + currentHealth);
        healthText.text = "Health: " + currentHealth.ToString();
        print("test" + currentHealth.ToString());
    }
    [PunRPC]
    private void ApplyDamageRPC(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        UpdateHealthText();

        // Check if the player is dead
        if (currentHealth <= 0)
        {
            // Only destroy the player on the owner client
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);

                // Perform actions when the player dies
                Debug.Log("Player is dead");


                // Optionally, you might respawn the player or show a game over screen
                //if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
                //{
                //    PhotonNetwork.Instantiate("Character_01",
                //        new Vector3(0f, 0f, 0f),
                //        Quaternion.identity,
                //        0);
                //}
                //
                // If player 2:
                //if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
                //{
                //    PhotonNetwork.Instantiate("Character_02",
                //        new Vector3(0f, 0f, 0f),
                //        Quaternion.identity,
                //        0);
                //}
            }
        }
    }

    



    //PLAYER 2
    [PunRPC]
    public void UpdateHealth2(int damage2)
    {
        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                ApplyDamageRPC2(damage2);
                Debug.Log("Health: " + currentHealth2);
            }
            else
            {
                Debug.Log("UpdateHealth called on non-local player!");
            }
        }
        else
        {
            Debug.Log("PhotonView is null!");
        }
    }


    private void UpdateHealthText2()
    {
        healthText2.text = "Health: " + currentHealth2.ToString();
    }

    [PunRPC]
    private void ApplyDamageRPC2(int damage2)
    {
        currentHealth2 -= damage2;
        Debug.Log(currentHealth2);
        UpdateHealthText2();

        // Check if the player is dead
        if (currentHealth2 <= 0)
        {
            // Only destroy the player on the owner client
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);

                // Perform actions when the player dies
                Debug.Log("Player is dead");

                // Optionally, you might respawn the player or show a game over screen
            }
        }
    }

    

}
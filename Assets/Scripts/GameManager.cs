using Newtonsoft.Json;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPun, IPunObservable
{
    public static GameManager instance;
    [SerializeField]
    private int currentHealth = 3;
    public TextMeshProUGUI healthText;

    [SerializeField]
    private int currentHealth2 = 3;
    public TextMeshProUGUI healthText2;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;

    [SerializeField]
    public int score;
    [SerializeField]
    public int score2;


    new private PhotonView photonView;


    private void Awake()
    {
        instance = this;
  
    }

    public void Start()
    {
        //healthText = GetComponent<TextMeshProUGUI>();
        //healthText2 = GetComponent<TextMeshProUGUI>();

        Debug.Log("start method is called");
        photonView = GetComponent<PhotonView>();

        UpdateHealthText();
        UpdateHealthText2();

        Debug.Log("Starting");
        photonView = GetComponent<PhotonView>();
        UpdateScoreText();
        UpdateScoreText2();
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    //score

    [PunRPC]
    public void IncreaseScore(int amount)
    {
        if (photonView.IsMine)
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
        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                UpdateOtherPlayersScore(amount2);
                Debug.Log("Player 2 score increased by " + amount2);
            }
            else
            {
                Debug.Log("IncreaseScore2 called on non-local player!");
            }
        }
        else
        {
            Debug.Log("PhotonView is null!");
        }
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

        // Save the score using PlayerPrefs
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
        Debug.Log("Scores saved - Score: " + score + ", Score2: " + score2);
    }

    private void UpdateScoreText2()
    {
        scoreText2.text = "Player 2 score: " + score2.ToString();
    }

    private void UpdateOtherPlayersScore(int amount2)
    {
        score2 += amount2;
        scoreText2.text = "Character_02: " + score2;
        UpdateScoreText2();

        // Save the score using PlayerPrefs
        PlayerPrefs.SetInt("Score2", score2);
        PlayerPrefs.Save();
        Debug.Log("Scores saved - Score: " + score + ", Score2: " + score2);
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
        healthText.text = "Health: " + currentHealth.ToString();
    }
   

    private void ApplyDamageRPC(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        UpdateHealthText();

        if (currentHealth <= 0)
        {
            // Only destroy the player on the owner client
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);

                // Perform actions when the player dies
                Debug.Log("Player is dead");
                SceneManager.LoadScene(2);
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
                SceneManager.LoadScene(2);
            }
            
        }
    }






    //SPAWNING

    public void SpawnButton()
    {
     
        // If player 1:
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate("Character_01",
                new Vector3(0f, 0f, -4f),
                Quaternion.identity,
                0);
        }

        // If player 2:
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            PhotonNetwork.Instantiate("Character_02",
                new Vector3(0f, 0f, -4f),
                Quaternion.identity,
                0);
        }
    }

    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Sending data to others
            stream.SendNext(currentHealth);
            stream.SendNext(currentHealth2);
            stream.SendNext(score);
            stream.SendNext(score2);
            stream.SendNext(transform.position);
        }
        else
        {
            // Receiving data from the owner
            currentHealth = (int)stream.ReceiveNext();
            Debug.Log(currentHealth);
            currentHealth2 = (int)stream.ReceiveNext();
            Debug.Log(currentHealth2);
            score = (int)stream.ReceiveNext();
            Debug.Log(score);
            score2 = (int)stream.ReceiveNext();
            Debug.Log(score2);

            // Update the health text on all clients
            UpdateHealthText();
            UpdateHealthText2();

            UpdateScoreText();
            UpdateScoreText2();
            Debug.Log("Received actor number: " + info.Sender.ActorNumber);
        }
    }

    
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    private Vector3 targetPosition;
    public float smoothSpeed = 0.125f;
    private CharacterController characterController;
    private Animator animator;

    private bool isMoving = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Local player controls movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Update target position based on input
            targetPosition += new Vector3(horizontal, 0, vertical) * Time.deltaTime * 3;

            // Smoothly move towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            if (moveDirection.magnitude >= 0.1f)
            {
                characterController.Move(moveDirection * Time.deltaTime * 5f);
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            // Update the IsMoving bool in the Animator locally
            animator.SetBool("IsMoving", isMoving);

            // Send RPC to update IsMoving bool on other clients
            photonView.RPC("UpdateIsMoving", RpcTarget.Others, isMoving);
        }
    }

    [PunRPC]
    private void UpdateIsMoving(bool moving)
    {
        // Update the IsMoving bool on remote clients
        animator.SetBool("IsMoving", moving);
    }

    // IPunObservable implementation for network synchronization
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Sending data to others
            stream.SendNext(transform.position);
            stream.SendNext(isMoving);
        }
        else
        {
            // Receiving data from the owner
            transform.position = (Vector3)stream.ReceiveNext();
            isMoving = (bool)stream.ReceiveNext();
            animator.SetBool("IsMoving", isMoving);
        }
    }
}

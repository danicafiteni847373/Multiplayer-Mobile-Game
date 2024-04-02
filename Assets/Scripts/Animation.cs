using UnityEngine;
using Photon.Pun;

public class Animation : MonoBehaviourPun, IPunObservable
{
    private CharacterController characterController;
    private Animator animator;

    private bool isMoving = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (!photonView.IsMine)
        {
            // Disable scripts and components that should only be active for the local player
            // For example, you might want to disable the camera control script for remote players
            // Disable any scripts that directly control the character's movement
            // ...
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Handle local player input and movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

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

            // Update the IsMoving bool in the Animator
            animator.SetBool("IsMoving", isMoving);

            // Send RPC to update IsMoving bool on other clients
            photonView.RPC("UpdateIsMoving", RpcTarget.Others, isMoving);
        }
    }

    [PunRPC]
    private void UpdateIsMoving(bool moving) =>
        // Update the IsMoving bool on remote clients
        animator.SetBool("IsMoving", moving);

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // You can use this method to synchronize additional data if needed
        if (stream.IsWriting)
        {
            // Send data to others
            stream.SendNext(isMoving);
        }
        else
        {
            // Receive data from others
            isMoving = (bool)stream.ReceiveNext();
            animator.SetBool("IsMoving", isMoving);
        }
    }
}


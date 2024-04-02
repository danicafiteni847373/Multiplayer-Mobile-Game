using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour, IPunInstantiateMagicCallback, IPunObservable
{
    public MovementJoystick movementJoystick;
    private PhotonView photonView;
    private Rigidbody2D rb;
    private Vector3 playerPos;
    private Quaternion playerRot; // New variable to store rotation information

    private Animator animator;

    public float playerSpeed;
    public bool IsMoving { get; private set; }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
    }

    void Start()
    {
        photonView = PhotonView.Get(this);
        animator = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            movementJoystick = GameObject.Find("MovementJoystick").GetComponent<MovementJoystick>();
            rb = GetComponent<Rigidbody2D>();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(IsMoving);
            stream.SendNext(transform.rotation); // Send rotation information
        }
        else
        {
            playerPos = (Vector3)stream.ReceiveNext();
            IsMoving = (bool)stream.ReceiveNext();
            playerRot = (Quaternion)stream.ReceiveNext(); // Receive rotation information
            UpdateAnimation();
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            rb.velocity = new Vector2(movementJoystick.joystickVec.x * playerSpeed, movementJoystick.joystickVec.y * playerSpeed);
            UpdateMovementState();
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, playerPos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, playerRot, Time.deltaTime * 10); // Smoothly interpolate rotation
            UpdateAnimation();
        }
    }

    private void UpdateMovementState()
    {
        float horizontal = movementJoystick.joystickVec.x;
        float vertical = movementJoystick.joystickVec.y;

        IsMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;
        UpdateAnimation();

        // Update player's rotation based on movement direction
        if (IsMoving)
        {
            float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", IsMoving);
        }
    }
}

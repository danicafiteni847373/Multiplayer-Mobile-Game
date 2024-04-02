using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviourPun
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private void Update()
    {
        if (!photonView.IsMine)
        {
            transform.gameObject.SetActive(false); // Disable camera for non-local player
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("Target not set for PlayerCameraFollow script.");
            return;
        }

        // Calculate the desired position for the camera
        Vector3 targetPosition = target.position + offset;

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Keep the camera looking at the target without rotation
        transform.LookAt(target.position);
    }
}

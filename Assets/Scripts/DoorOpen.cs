using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DoorOpen : UdonSharpBehaviour
{
    [Header("Door")]
    public Transform door;              // Kéo object cửa vào đây

    [Header("Rotation")]
    public Vector3 closedRotation = new Vector3(0f, 0f, 0f);
    public Vector3 openRotation = new Vector3(0f, 90f, 0f);

    [Header("Speed")]
    public float openSpeed = 3f;

    [UdonSynced] private bool isOpen = false;

    private Quaternion targetRotation;

    void Start()
    {
        if (door == null) door = transform;

        targetRotation = Quaternion.Euler(closedRotation);
        door.localRotation = targetRotation;
    }

    public override void Interact()
    {
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }

        isOpen = !isOpen;
        ApplyDoorState();

        RequestSerialization();
    }

    public override void OnDeserialization()
    {
        ApplyDoorState();
    }

    private void ApplyDoorState()
    {
        if (isOpen)
        {
            targetRotation = Quaternion.Euler(openRotation);
        }
        else
        {
            targetRotation = Quaternion.Euler(closedRotation);
        }
    }

    void Update()
    {
        if (door == null) return;

        door.localRotation = Quaternion.Lerp(
            door.localRotation,
            targetRotation,
            Time.deltaTime * openSpeed
        );
    }
}
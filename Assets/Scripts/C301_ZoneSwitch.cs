using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class C301_ZoneSwitch : UdonSharpBehaviour
{
    [Header("Den trong khu vuc")]
    public GameObject[] lightObjects;

    [Header("Light component neu co")]
    public Light[] lightComponents;

    [Header("Quat trong khu vuc")]
    public C301_FanSpin[] fans;

    [Header("Can cong tac")]
    public Transform switchLever;
    public Vector3 offRotation = new Vector3(0f, 0f, 0f);
    public Vector3 onRotation = new Vector3(25f, 0f, 0f);

    [Header("Trang thai hien tai")]
    [UdonSynced] public bool isOn = false;

    void Start()
    {
        // Luon ep trang thai ban dau la OFF
        isOn = false;
        ApplyState();

        // Apply lai sau mot chut de tranh script khac tu bat den/quat len sau Start()
        SendCustomEventDelayedSeconds("DelayedApplyState", 0.25f);
    }

    public void DelayedApplyState()
    {
        isOn = false;
        ApplyState();

        if (Networking.LocalPlayer != null && Networking.IsOwner(gameObject))
        {
            RequestSerialization();
        }
    }

    public override void Interact()
    {
        if (Networking.LocalPlayer != null)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }

        isOn = !isOn;

        ApplyState();
        RequestSerialization();

        Debug.Log("ZoneSwitch clicked. Is On = " + isOn);
    }

    public override void OnDeserialization()
    {
        ApplyState();
    }

    public void TriggerInteract()
    {
        Interact();
    }

    public void ApplyState()
    {
        if (lightObjects != null)
        {
            for (int i = 0; i < lightObjects.Length; i++)
            {
                if (lightObjects[i] != null)
                {
                    lightObjects[i].SetActive(isOn);
                }
            }
        }

        if (lightComponents != null)
        {
            for (int i = 0; i < lightComponents.Length; i++)
            {
                if (lightComponents[i] != null)
                {
                    lightComponents[i].enabled = isOn;
                }
            }
        }

        if (fans != null)
        {
            for (int i = 0; i < fans.Length; i++)
            {
                if (fans[i] != null)
                {
                    fans[i].SetFanState(isOn);
                }
            }
        }

        if (switchLever != null)
        {
            switchLever.localEulerAngles = isOn ? onRotation : offRotation;
        }
    }
}

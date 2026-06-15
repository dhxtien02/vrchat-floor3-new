using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

// Gan script nay len NUT STOP (object co Box Collider)
public class MicrowaveStopButton : UdonSharpBehaviour
{
    [Header("Keo object NUT START (co script MicrowaveControl) vao day")]
    public MicrowaveControl microwave;

    public override void Interact()
    {
        if (microwave != null) microwave.StopCooking();
    }
}

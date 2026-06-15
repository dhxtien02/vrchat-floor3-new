using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

// Gan script nay len TUNG nut (object co Box Collider). Dat "Action" khac nhau cho moi nut.
public class ACButton : UdonSharpBehaviour
{
    [Header("Keo object may lanh (air_conditioner_02) vao day")]
    public AirConditioner ac;   // kieu AirConditioner de goi ham truc tiep

    [Header("0 = Bat | 1 = Tat | 2 = Tang nhiet | 3 = Giam nhiet")]
    public int action = 0;

    public override void Interact()
    {
        if (ac == null) { Debug.Log("[ACBTN] ac NULL!"); return; }

        Debug.Log("[ACBTN] action=" + action + " | goi truc tiep tren: " + ac.gameObject.name);

        // Goi ham THANG (noi cung luc compile, khong di lac)
        if (action == 0) ac.PowerOn();
        else if (action == 1) ac.PowerOff();
        else if (action == 2) ac.TempUp();
        else if (action == 3) ac.TempDown();
    }
}
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
 
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class TV_Slide : UdonSharpBehaviour
{
    [Header("Keo tha MeshRenderer cua Quad man hinh vao day")]
    public MeshRenderer screenRenderer;
 
    [Header("Danh sach anh slide - keo tha theo dung thu tu")]
    public Texture[] slides;
 
    // state = 0 : TV dang TAT
    // state = 1..N : dang chieu slide thu N
    [UdonSynced] private int state = 0;
 
    private Material screenMat;
 
    void Start()
    {
        screenMat = screenRenderer.material;
        ApplyState();
    }
 
    public override void Interact()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
 
        state++;
        if (state > slides.Length)
        {
            state = 0;
        }
 
        ApplyState();
        RequestSerialization();
    }
 
    public override void OnDeserialization()
    {
        ApplyState();
    }
 
    private void ApplyState()
    {
        if (state == 0)
        {
            screenRenderer.enabled = false;
        }
        else
        {
            screenRenderer.enabled = true;
            screenMat.mainTexture = slides[state - 1];
        }
    }
}
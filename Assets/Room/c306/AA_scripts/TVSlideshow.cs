using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

// Dòng dưới đây bắt buộc phải có để đồng bộ (sync) trạng thái cho mọi người trong world
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class TVSlideshow : UdonSharpBehaviour
{
    [Header("Keo tha MeshRenderer cua Quad man hinh vao day")]
    public MeshRenderer screenRenderer;

    [Header("Danh sach anh slide - keo tha theo dung thu tu")]
    public Texture[] slides;

    // state = 0 : TV dang TAT
    // state = 1..N : dang chieu slide thu N
    [UdonSynced] private int state = 0;

    private Material screenMat;   // material rieng cua man hinh nay

    void Start()
    {
        // Lay ra mot ban material rieng -> sua TV nay khong anh huong TV khac
        screenMat = screenRenderer.material;
        ApplyState();
    }

    // Ham nay tu chay khi nguoi choi bam (click / bop trigger) vao vat the
    public override void Interact()
    {
        // Gianh quyen dieu khien de duoc phep sync cho moi nguoi
        Networking.SetOwner(Networking.LocalPlayer, gameObject);

        // Bam 1 lan: TAT -> slide 1 -> slide 2 -> ... -> slide cuoi -> TAT -> ...
        state++;
        if (state > slides.Length)
        {
            state = 0;
        }

        ApplyState();            // cap nhat man hinh o may minh
        RequestSerialization();  // gui trang thai moi cho nhung nguoi khac
    }

    // Ham nay tu chay o may NGUOI KHAC khi ho nhan duoc trang thai moi
    public override void OnDeserialization()
    {
        ApplyState();
    }

    // Ve hinh anh len man hinh theo state hien tai
    private void ApplyState()
    {
        if (state == 0)
        {
            // TAT: an Quad slide di (man hinh TV se troi ve mau den goc cua no)
            screenRenderer.enabled = false;
        }
        else
        {
            // BAT: hien Quad va doi sang dung anh slide
            screenRenderer.enabled = true;
            screenMat.mainTexture = slides[state - 1];
        }
    }
}

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

// ============================================================
//  DoorToggle - Đóng/mở cửa khi người chơi bấm vào cánh.
//  - Gắn script này vào TỪNG cánh cửa (object có Mesh + Collider).
//  - Người chơi bấm "Use" vào cánh  -> cánh xoay mở.
//  - Bấm lần nữa                    -> cánh đóng lại.
//  - Trạng thái được ĐỒNG BỘ QUA MẠNG: mọi người trong world
//    đều thấy cửa đóng/mở giống nhau.
// ============================================================
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class DoorToggle : UdonSharpBehaviour
{
    [Tooltip("Vật sẽ xoay (bản lề). Để TRỐNG nếu pivot của cánh đã nằm ở mép bản lề.")]
    public Transform hinge;

    [Tooltip("Góc mở quanh trục Y (độ). Đổi DẤU +/- để mở sang phía ngược lại.")]
    public float openAngle = 100f;

    [Tooltip("Tốc độ đóng/mở (độ mỗi giây). Lớn hơn = nhanh hơn.")]
    public float speed = 220f;

    [UdonSynced] private bool isOpen;          // trạng thái, đồng bộ qua mạng
    private Quaternion closedRot, openRot, targetRot;

    void Start()
    {
        // Không gán hinge -> tự xoay chính object này (pivot phải nằm ở mép bản lề)
        if (hinge == null) hinge = transform;

        closedRot = hinge.localRotation;                          // tư thế lúc đóng
        openRot = closedRot * Quaternion.Euler(0f, openAngle, 0f); // tư thế lúc mở
        targetRot = closedRot;
    }

    // Gọi khi người chơi bấm "Use" vào cánh cửa (cần có Collider trên cánh)
    public override void Interact()
    {
        // Giành quyền sở hữu để gửi trạng thái cho người khác
        if (!Networking.IsOwner(gameObject))
            Networking.SetOwner(Networking.LocalPlayer, gameObject);

        isOpen = !isOpen;
        UpdateTarget();
        RequestSerialization();   // đẩy trạng thái mới lên mạng
    }

    // Chạy trên máy người khác khi nhận được trạng thái mới
    public override void OnDeserialization()
    {
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        targetRot = isOpen ? openRot : closedRot;
    }

    void Update()
    {
        // Xoay mượt từ tư thế hiện tại về tư thế đích
        hinge.localRotation = Quaternion.RotateTowards(
            hinge.localRotation, targetRot, speed * Time.deltaTime);
    }
}
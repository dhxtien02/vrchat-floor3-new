using UdonSharp;
using UnityEngine;

public class C301_FanSpin : UdonSharpBehaviour
{
    [Header("Trang thai quat")]
    public bool isOn = false;

    [Header("Toc do quay")]
    public float rotateSpeed = 350f;

    [Header("Phan se quay")]
    [Tooltip("Keo phan canh quat vao day. Neu de trong thi se xoay object dang gan script.")]
    public Transform rotatingTarget;

    [Header("Truc quay")]
    [Tooltip("0 = X, 1 = Y, 2 = Z, 3 = Custom Axis")]
    public int axisMode = 1;

    [Tooltip("Dung khi Axis Mode = 3")]
    public Vector3 customAxis = new Vector3(0f, 1f, 0f);

    [Header("Huong quay")]
    public bool reverseDirection = false;

    void Start()
    {
        if (rotatingTarget == null)
        {
            rotatingTarget = transform;
        }
    }

    void Update()
    {
        if (!isOn || rotatingTarget == null) return;

        Vector3 axis = GetAxis();

        float direction = reverseDirection ? -1f : 1f;
        float angle = rotateSpeed * direction * Time.deltaTime;

        // Xoay tai cho theo truc local cua object
        // Cach nay giup quat khong bi bay vong quanh / quay lech tam
        rotatingTarget.Rotate(axis, angle, Space.Self);
    }

    private Vector3 GetAxis()
    {
        if (axisMode == 0)
        {
            return Vector3.right;   // X
        }
        else if (axisMode == 1)
        {
            return Vector3.up;      // Y
        }
        else if (axisMode == 2)
        {
            return Vector3.forward; // Z
        }
        else
        {
            if (customAxis == Vector3.zero)
            {
                return Vector3.up;
            }

            return customAxis.normalized;
        }
    }

    public void SetFanState(bool state)
    {
        isOn = state;
    }

    public void TurnOn()
    {
        isOn = true;
    }

    public void TurnOff()
    {
        isOn = false;
    }

    public void ToggleFan()
    {
        isOn = !isOn;
    }
}

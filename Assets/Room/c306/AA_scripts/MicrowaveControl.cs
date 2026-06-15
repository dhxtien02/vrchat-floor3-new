using UdonSharp;
using UnityEngine;
using TMPro;
using VRC.SDKBase;
using VRC.Udon;

// Gan script nay len NUT START (object co Box Collider)
public class MicrowaveControl : UdonSharpBehaviour
{
    [Header("Man hinh hien gio (keo object TextMeshPro vao day)")]
    public TextMeshPro display;

    [Header("Thoi gian nau, tinh bang giay (vd 30)")]
    public float duration = 30f;

    [Header("Am thanh (de trong neu chua co)")]
    public AudioSource runningSound;   // tieng lo chay (bat Loop)
    public AudioSource beepSound;      // tieng beep khi xong

    [Header("Den ben trong lo (tuy chon, de trong cung duoc)")]
    public Light insideLight;

    [Header("Chu hien khi lo dang TAT")]
    public string idleText = "12:45";

    private bool isRunning = false;
    private float remaining = 0f;

    void Start()
    {
        ShowIdle();
    }

    // Nguoi choi bam nut START
    public override void Interact()
    {
        if (isRunning) return;   // dang chay roi thi khong lam gi
        StartCooking();
    }

    public void StartCooking()
    {
        isRunning = true;
        remaining = duration;

        if (runningSound != null) runningSound.Play();
        if (insideLight != null) insideLight.enabled = true;

        UpdateDisplay();
    }

    // Nut STOP se goi ham nay
    public void StopCooking()
    {
        isRunning = false;
        if (runningSound != null) runningSound.Stop();
        if (insideLight != null) insideLight.enabled = false;
        ShowIdle();
    }

    void Update()
    {
        if (!isRunning) return;

        remaining -= Time.deltaTime;

        if (remaining <= 0f)
        {
            // Het gio: tat tieng, tat den, keu beep, ve lai chu cu
            remaining = 0f;
            isRunning = false;
            if (runningSound != null) runningSound.Stop();
            if (insideLight != null) insideLight.enabled = false;
            if (beepSound != null) beepSound.Play();
            ShowIdle();
            return;
        }

        UpdateDisplay();
    }

    // Ve thoi gian con lai len man hinh dang m:ss
    private void UpdateDisplay()
    {
        if (display == null) return;

        int totalSec = (int)Mathf.Ceil(remaining);
        int minutes = totalSec / 60;
        int seconds = totalSec % 60;

        string sec = seconds < 10 ? "0" + seconds : "" + seconds;
        display.text = minutes + ":" + sec;
    }

    private void ShowIdle()
    {
        if (display != null) display.text = idleText;
    }
}

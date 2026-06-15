using UdonSharp;
using UnityEngine;
using TMPro;
using VRC.SDKBase;
using VRC.Udon;

// Gan script nay len object goc cua may lanh (khong can collider)
public class AirConditioner : UdonSharpBehaviour
{
    [Header("Man hinh nhiet do (TextMeshPro)")]
    public TextMeshPro display;

    [Header("O vuong bao hieu - keo MeshRenderer cua TempIndicator vao")]
    public MeshRenderer indicator;
    public Color onColor = Color.green;
    public Color offColor = new Color(0.12f, 0.13f, 0.15f);

    [Header("Am thanh gio thoi (bat Loop tren Audio Source)")]
    public AudioSource windSound;

    [Header("Nhiet do (do C)")]
    public int temperature = 25;
    public int minTemp = 16;
    public int maxTemp = 30;

    private bool isOn = false;
    private Material indicatorMat;

    void Start()
    {
        if (indicator != null) indicatorMat = indicator.material;
        ApplyIndicator();
        UpdateDisplay();
    }

    public void PowerOn()
    {
        Debug.Log("[AC] >>> PowerOn CHAY");
        if (isOn) return;
        isOn = true;
        if (windSound != null) windSound.Play();
        ApplyIndicator();
    }

    public void PowerOff()
    {
        Debug.Log("[AC] >>> PowerOff CHAY");
        if (!isOn) return;
        isOn = false;
        if (windSound != null) windSound.Stop();
        ApplyIndicator();
    }

    public void TempUp()
    {
        Debug.Log("[AC] >>> TempUp CHAY (truoc khi tang temp=" + temperature + ")");
        temperature++;
        if (temperature > maxTemp) temperature = maxTemp;
        UpdateDisplay();
    }

    public void TempDown()
    {
        Debug.Log("[AC] >>> TempDown CHAY (truoc khi giam temp=" + temperature + ")");
        temperature--;
        if (temperature < minTemp) temperature = minTemp;
        UpdateDisplay();
    }

    private void ApplyIndicator()
    {
        if (indicatorMat == null) return;
        indicatorMat.color = isOn ? onColor : offColor;
    }

    private void UpdateDisplay()
    {
        Debug.Log("[AC] UpdateDisplay: display null? " + (display == null) + " | temp=" + temperature);
        if (display == null) return;
        display.text = temperature + "\u00B0C";
    }
}
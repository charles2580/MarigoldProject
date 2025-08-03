using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct forensicTarget
{
    public Button Button;
    public Image Image;
    public TMP_Text TargetName;
    public Sprite FingerPrint;
}


public class ForensicTargetSlot : MonoBehaviour
{
    [SerializeField] private List<forensicTarget> targetList;
    [SerializeField] private Image fingerPrintImage;
    
    private PoliceNoteInventoryUI inventoryUI;

    public void Initialize(Image TargetImage)
    {
        foreach (var target in targetList)
        {
            target.Button.onClick.AddListener(() => SetForensicTarget(TargetImage, target.Image, target.FingerPrint));
        }
    }

    public void SetForensicTarget(Image TargetImage, Image ButtonImage, Sprite FingerPrintImage)
    {
        TargetImage.sprite = ButtonImage.sprite;
        TargetImage.gameObject.SetActive(true);
        fingerPrintImage.sprite = FingerPrintImage;
        fingerPrintImage.gameObject.SetActive(true);
        inventoryUI = PoliceNoteInventoryUI.Instance;
        //inventoryUI.targetName = TargetName;
        gameObject.SetActive(false);
    }

}

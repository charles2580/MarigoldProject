using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using System.ComponentModel;

[System.Serializable]
public struct ForensicTarget
{
    public CharacterSO Suspector;
    public Sprite FingerPrintImage;
}


public class ForensicManager : MonoBehaviour
{
    //감식 아이템 선택 버튼
    [SerializeField] private Button evidenceSelectButton;
    [SerializeField] private GameObject forensicItemPopUp;
    [SerializeField] private Transform evidenceSlotContainer; // 슬롯을 동적으로 생성할 부모
    [SerializeField] private GameObject forensicSlotPrefab;   // ForensicSlotUI 프리팹
    [SerializeField] private GameObject evidenceInventory;
    [SerializeField] private GameObject evidencePlusImage;

    //감식 아이템 선택창 ui
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image itemFingerprint;

    //감식 대상 선택 버튼
    [SerializeField] private Button targetSelectButton;
    [SerializeField] private GameObject forensicTargetPopUp;
    [SerializeField] private Transform targetSlotContainer;
    [SerializeField] private GameObject targetSlotPrefab;
    [SerializeField] private List<ForensicTarget> SuspectorList;
    [SerializeField] private GameObject targetPlusImage;

    //감식 대상 선택창 ui
    [SerializeField] private Image targetIcon;
    [SerializeField] private Image targetFingerprint;

    [SerializeField] private Button submitButton; // 검사하기 버튼

    //결과 팝업 ui
    [SerializeField] private GameObject forensicPopUp;
    [SerializeField] private Transform popUpParent;
    private ForensicPopUpUI popUpUI;

    private Item selectedItem;
    private CharacterSO selectedTarget;

    private List<Item> allEvidenceItems = new List<Item>();
    private List<Item> allPossessionItems = new List<Item>();

    public void Setup(List<Item> possessions, List<Item> evidences)
    {
        allPossessionItems = possessions;
        allEvidenceItems = evidences;
        ClearSelection();

        evidenceSelectButton.onClick.RemoveAllListeners();
        evidenceSelectButton.onClick.AddListener(OpenEvidenceSelection);

        targetSelectButton.onClick.RemoveAllListeners();
        targetSelectButton.onClick.AddListener(OpenTargetSelection);

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(SubmitForensicCheck);


    }

    private void ClearSelectedDisplays()
    {
        itemIcon.gameObject.SetActive(false);
        itemFingerprint.gameObject.SetActive(false);
        targetIcon.gameObject.SetActive(false);
        targetFingerprint.gameObject.SetActive(false);
        targetPlusImage.SetActive(true);
        evidencePlusImage.SetActive(true);
    }

    public void ClearSelection()
    {
        selectedItem = null;
        selectedTarget = null;
        ClearSelectedDisplays();
    }

    private void OpenEvidenceSelection()
    {
        forensicItemPopUp.SetActive(true);
        evidenceInventory.SetActive(false);
        // 기존 슬롯 삭제
        foreach (Transform t in evidenceSlotContainer)
        {
            Destroy(t.gameObject);
        }

        // 증거 + 소지품에서 검사 가능한 아이템만 필터링
        var candidates = new List<Item>(allEvidenceItems);
        candidates.AddRange(allPossessionItems);
        candidates.RemoveAll(i => i.ItemData.FingerPrint == null);

        // 슬롯 생성 및 클릭 바인딩
        foreach (var item in candidates)
        {
            var go = Instantiate(forensicSlotPrefab, evidenceSlotContainer);
            var slot = go.GetComponent<ForensicSlotUI>();
            slot.Initialize(item, OnEvidenceSelected);
        }
    }

    private void OnEvidenceSelected(Item item)
    {
        selectedItem = item;

        // UI 업데이트
        itemIcon.sprite = item.ItemData.Icon;
        itemIcon.gameObject.SetActive(true);

        itemFingerprint.sprite = item.ItemData.FingerPrint;
        itemFingerprint.gameObject.SetActive(true);
        forensicItemPopUp.SetActive(false);
        evidenceInventory.SetActive(true);
        evidencePlusImage.SetActive(false);
    }

    private void OpenTargetSelection()
    {
        forensicTargetPopUp.SetActive(true);

        // Clear existing slots
        for (int i = targetSlotContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(targetSlotContainer.GetChild(i).gameObject);
        }
        // Instantiate suspector slots
        foreach (var ft in SuspectorList)
        {
            GameObject go = Instantiate(targetSlotPrefab, targetSlotContainer);
            var slot = go.GetComponent<ForensicSlotUI>();
            slot.Initialize(ft.Suspector, OnTargetSelected);
        }
    }

    private void OnTargetSelected(CharacterSO character)
    {
        selectedTarget = character;
        // Update UI
        targetIcon.sprite = character.CharacterImages[0];
        targetIcon.gameObject.SetActive(true);

        // Find fingerprint image from SuspectorList
        var ft = SuspectorList.Find(x => x.Suspector == character);
        targetFingerprint.sprite = ft.FingerPrintImage;
        targetFingerprint.gameObject.SetActive(true);

        forensicTargetPopUp.SetActive(false);
        targetPlusImage.SetActive(false);
    }

    private void SubmitForensicCheck()
    {
        if (selectedItem == null || selectedTarget == null)
            return;

        // Compare owner IDs
        bool match = selectedItem.ItemData.Owner.ID == selectedTarget.ID;
        GameObject popup = Instantiate(forensicPopUp, popUpParent, false);
        popUpUI = popup.GetComponent<ForensicPopUpUI>();
        popUpUI.SetForensicPopUpUI(selectedItem, targetIcon,targetFingerprint,
            selectedTarget.Name,match );
    }
}

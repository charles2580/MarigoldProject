using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

//Item 또는 Character 중 하나로 Initialize하여 사용합니다.

public class ForensicSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject fingerprintOffImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button button;
    [SerializeField] private Image highlightImage;

    private Item item;
    private CharacterSO character;

    private Action<Item> onItemSelected;
    private Action<CharacterSO> onCharacterSelected;

    // 아이템 슬롯으로 초기화합니다.
    public void Initialize(Item item, Action<Item> callback)
    {
        this.item = item;
        this.character = null;
        onItemSelected = callback;
        onCharacterSelected = null;

        // UI 세팅
        iconImage.sprite = item.ItemData.Icon;
        bool hasFingerPrint = item.ItemData.FingerPrint == null; 
        fingerprintOffImage.gameObject.SetActive(hasFingerPrint);
        highlightImage.gameObject.SetActive(false);

        // 버튼은 Pointer 이벤트만 사용하므로 비활성화
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }
    }

    // 대상 슬롯으로 초기화합니다. 캐릭터의 지문 이미지는 외부에서 전달받아 세팅합니다.
    public void Initialize(CharacterSO character, Action<CharacterSO> callback)
    {
        this.character = character;
        this.item = null;
        onCharacterSelected = callback;
        onItemSelected = null;

        // UI 세팅
        iconImage.sprite = character.CharacterImages[2];
        nameText.text = character.Name;
        //highlightImage.gameObject.SetActive(false);

        // Button 클릭 이벤트 설정
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onCharacterSelected?.Invoke(this.character));
            button.interactable = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            onItemSelected?.Invoke(item);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
        {
            highlightImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            highlightImage.gameObject.SetActive(false);
        }
    }
}

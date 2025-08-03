using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

//Item �Ǵ� Character �� �ϳ��� Initialize�Ͽ� ����մϴ�.

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

    // ������ �������� �ʱ�ȭ�մϴ�.
    public void Initialize(Item item, Action<Item> callback)
    {
        this.item = item;
        this.character = null;
        onItemSelected = callback;
        onCharacterSelected = null;

        // UI ����
        iconImage.sprite = item.ItemData.Icon;
        bool hasFingerPrint = item.ItemData.FingerPrint == null; 
        fingerprintOffImage.gameObject.SetActive(hasFingerPrint);
        highlightImage.gameObject.SetActive(false);

        // ��ư�� Pointer �̺�Ʈ�� ����ϹǷ� ��Ȱ��ȭ
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }
    }

    // ��� �������� �ʱ�ȭ�մϴ�. ĳ������ ���� �̹����� �ܺο��� ���޹޾� �����մϴ�.
    public void Initialize(CharacterSO character, Action<CharacterSO> callback)
    {
        this.character = character;
        this.item = null;
        onCharacterSelected = callback;
        onItemSelected = null;

        // UI ����
        iconImage.sprite = character.CharacterImages[2];
        nameText.text = character.Name;
        //highlightImage.gameObject.SetActive(false);

        // Button Ŭ�� �̺�Ʈ ����
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

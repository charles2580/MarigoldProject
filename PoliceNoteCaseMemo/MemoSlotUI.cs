using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ImageSize
{
    Small = 0,
    Medium = 1,
    Large = 2
}

public class MemoSlotUI : MonoBehaviour
{
    [SerializeField] private Image speakerImage;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text conversationLocationText;
    [SerializeField] private TMP_Text conversationText;
    [SerializeField] private Button deleteButton;               

    public void Initialize(string place, CharacterSO speaker, string conversation)
    {
        if (speakerImage != null)
            speakerImage.sprite = speaker.CharacterImages[(int)ImageSize.Small];
        if (speakerNameText != null)
            speakerNameText.text = speaker.Name;
        if (conversationLocationText != null)
            conversationLocationText.text = place;
        if (conversationText != null)
            conversationText.text = conversation;

        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(OnDeleteClicked);
        }
    }

    void OnDeleteClicked()
    {
        Destroy(this.gameObject);
    }
}

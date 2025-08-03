using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class SpeakerIconUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private GameObject choiceImage;
    [SerializeField] private Image speakerImage;
    
    private CharacterSO speaker;

    [SerializeField] private UIEventHandler eventHandler;
    public UnityEvent<string> OnSpeakerClicked;

    private Toggle toggle;

    public void Initialize()
    {
        eventHandler = gameObject.AddComponent<UIEventHandler>();
        toggle = gameObject.GetComponent<Toggle>();
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(isOn =>
        {
            choiceImage.SetActive(isOn);
            if(isOn)
            {
                OnSpeakerClicked?.Invoke(speakerNameText.text);
            }
        });
    }

    public void Setup(CharacterSO character)
    {
        this.speaker = character;
        if (speakerNameText != null)
        {
            speakerNameText.text = character.Name;
        }

        if(speakerImage !=null)
        {
            speakerImage.sprite = character.CharacterImages[(int)ImageSize.Small];
        }
        choiceImage.SetActive(false);
        toggle.isOn = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("OnPointerClick called");
        //choiceImage.SetActive(true);
        //OnSpeakerClicked.Invoke(speakerNameText.text);
        toggle.isOn = !toggle.isOn;
    }
}

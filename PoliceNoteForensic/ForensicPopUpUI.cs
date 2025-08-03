using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForensicPopUpUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image itemFingePrintImage;
    [SerializeField] Image characterImage;
    [SerializeField] Image characterFingerPrintImage;

    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text characterName;

    [SerializeField] Animator animator;
    //[SerializeField] GameObject SuccessText;
    //[SerializeField] GameObject FailureText;

    private bool bAnimFinished = false;

    public void SetForensicPopUpUI(Item Item, Image CharacterImage,
        Image CharacterFingerPrint, string CharacterName, bool match)
    {
        bAnimFinished = false;
        itemImage.sprite = Item.ItemData.Icon;
        itemFingePrintImage.sprite = Item.ItemData.FingerPrint;
        characterImage.sprite = CharacterImage.sprite;
        characterFingerPrintImage.sprite = CharacterFingerPrint.sprite;
        itemName.text = Item.ItemData.ItemName;
        characterName.text = CharacterName;

        Item.SetForensicResult(match);

        if(animator != null)
        {
            animator.ResetTrigger("MatchSuccess");
            animator.ResetTrigger("MatchFail");
            animator.SetTrigger(match ? "MatchSuccess" : "MatchFail");
        }

    }

    public void OnAnimFinished()
    {
        bAnimFinished = true;
    }

    public void Update()
    {
        if(bAnimFinished && Input.anyKeyDown)
        {
            GameObject.Destroy(gameObject);
        }
    }
}

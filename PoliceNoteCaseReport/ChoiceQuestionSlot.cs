using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceQuestionSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private List<Button> optionButtons;
    [SerializeField] private int questionID;

    [SerializeField] private PoliceNoteCaseReportUI reportUI;
    public void Awake()
    {
        for (int i = 0; i < optionButtons.Count; i++)
        {
            Button btn = optionButtons[i];
            int index = i;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate
            {
                Debug.Log("Button Clicked");
                var answer = new UserAnswerData(questionID, index.ToString());
                reportUI.reportData.AddAnswer(answer);
            });
        }
    }
}


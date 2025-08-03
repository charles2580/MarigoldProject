using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubjectiveQuestionSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private int questionID;

    [SerializeField] private PoliceNoteCaseReportUI reportUI;
    public void Start()
    {
        inputField.onEndEdit.RemoveAllListeners();
        inputField.onEndEdit.AddListener(OnInputEnded);
    }

    private void OnInputEnded(string input)
    {
        var answer = new UserAnswerData(questionID, input);
        reportUI.reportData.AddAnswer(answer);
    }
}

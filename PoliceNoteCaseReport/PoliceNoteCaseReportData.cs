using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceNoteCaseReportData
{
    [SerializeField] private List<UserAnswerData> answerList = new List<UserAnswerData>();

    public IReadOnlyList<UserAnswerData> AnswerList => answerList;

    public void AddAnswer(UserAnswerData answer)
    {
        if (answer == null) return;

        Debug.Log($"[AddAnswer] Q{answer.QuestionID} ¡æ ¡°{answer.UserAnswer}¡±");

        for (int i = 0; i < answerList.Count; i++)
        {
            if (answerList[i].QuestionID == answer.QuestionID)
            {
                answerList[i] = answer;
                return;
            }
        }

        answerList.Add(answer);
    }

    public UserAnswerData GetAnswer(int questionID)
    {
        foreach (var answer in answerList)
        {
            if (answer.QuestionID == questionID)

            {
                return answer;
            }
        }
        return null;
    }

    public void RemoveAnswer(int questionID)
    {
        answerList.RemoveAll(a => a.QuestionID == questionID);
    }
}

public class UserAnswerData
{
    [SerializeField] private int questionID;
    public int QuestionID => questionID;

    [SerializeField] private string userAnswer;
    public string UserAnswer => userAnswer;

    public UserAnswerData(int id, string answer)
    {
        questionID = id;
        userAnswer = answer;
    }
}
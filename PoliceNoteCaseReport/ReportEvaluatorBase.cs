//사용자 답안을 평가, 이동할 엔딩 시나리오 반환 인터페이스
using System;
using System.Linq;
using UnityEngine;

public interface IReportEvaluator
{
    string Evaluate(PoliceNoteCaseReportData reportData);
}

public abstract class ReportEvaluatorBase : IReportEvaluator
{

    public abstract string Evaluate(PoliceNoteCaseReportData reportData);

    protected UserAnswerData GetAnswerData(PoliceNoteCaseReportData data, int questionID)
    {
        if (data == null)
        {
            Debug.LogError("[ReportEvaluatorBase] reportData is null");
            return null;
        }
        return data.GetAnswer(questionID);
    }

    protected string GetAnswerText(PoliceNoteCaseReportData data, int questionID)
    {
        var answer = GetAnswerData(data, questionID);
        if (answer == null || string.IsNullOrWhiteSpace(answer.UserAnswer))
        {
            Debug.LogError("is Empty");
            return string.Empty;
        }
        return answer.UserAnswer.Trim();
    }

    protected bool CheckSubjective(PoliceNoteCaseReportData data, int questionID, string correct)
    {
        var answer = GetAnswerText(data, questionID);
        if (string.IsNullOrEmpty(answer) || string.IsNullOrEmpty(correct))
        {
            return false;
        }
        return string.Equals(answer, correct.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    protected bool CheckMultipleChoice(PoliceNoteCaseReportData data, int questionID, params string[] correctOptions)
    {
        var answer = GetAnswerText(data, questionID);
        // 빈 문자열이나 null 옵션 제외
        return correctOptions
            .Where(opt => !string.IsNullOrEmpty(opt))
            .Any(opt => string.Equals(answer, opt.Trim(), StringComparison.Ordinal));
    }

    protected bool CheckEvidenceById(PoliceNoteCaseReportData data, int questionID, params int[] correctIds)
    {
        var answer = GetAnswerData(data, questionID);
        if (answer == null || string.IsNullOrWhiteSpace(answer.UserAnswer))
            return false;

        if (!int.TryParse(answer.UserAnswer.Trim(), out var selectedId))
            return false;

        return correctIds.Contains(selectedId);
    }
}

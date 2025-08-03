using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Episode1ReportEvaluator : ReportEvaluatorBase
{

    private const string Ed01 = "Scenario1Ed01";
    private const string Ed02 = "Scenario1Ed02";
    private const string Ed03 = "Scenario1Ed03";
    private const string Ed04 = "Scenario1Ed04";
    private const string Ed05 = "Scenario1Ed05";
    private const string Ed06 = "Scenario1Ed06";
    private const string Ed07 = "Scenario1Ed07";

    public override string Evaluate(PoliceNoteCaseReportData data)
    {

        if (!CheckSubjective(data, 1, "유상우"))
            return Ed01;


        if (!CheckMultipleChoice(data, 2, "2"))
            return Ed01;


        if (!CheckMultipleChoice(data, 3, "2"))
            return Ed02;


        int wrongScore = 0;
        if (!CheckSubjective(data, 4, "부검기록"))
        {
            wrongScore++;
        }

        if (!CheckSubjective(data, 5, "박강헌"))
        {
            wrongScore++;
        }

        if (!CheckSubjective(data, 6, "두 남녀의 사진")) 
        { 
            wrongScore++; 
        }

        if (!CheckSubjective(data, 7, "박강헌"))
        {
            wrongScore++;
        }

        bool q8 = CheckMultipleChoice(data, 8, "2");
        bool q9 = CheckMultipleChoice(data, 9, "1");
        bool q10 = CheckMultipleChoice(data, 10, "1");

        if (q8)
        {
            if (!q9 && !q10)
            {
                return Ed03;
            }
            return Ed01;
        }

        else
        {
            if (q9 && !q10)
            {
                return Ed04;
            }

            if (!q9 && q10)
            {
                return Ed05;
            }

            if (q9 && q10)
            {
                return wrongScore > 1 ? Ed06 : Ed07;
            }

            return Ed01;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum EPlace
{
    [Description("K&L 탐정사무소")]
    KLDetectiveAgency,

    [Description("편의점")]
    ConvenienceStore
}

public class PoliceNoteCaseMemoData
{
    [SerializeField] private Dictionary<EPlace, Dictionary<CharacterSO, List<string>>> caseMemo;
    public Dictionary<EPlace,Dictionary<CharacterSO,List<string>>> CaseMemo 
    {
        get { return caseMemo; }
        set { caseMemo = value; }
    }

    public PoliceNoteCaseMemoData()
    {
        CaseMemo = new Dictionary<EPlace, Dictionary<CharacterSO, List<string>>>();
    }

    public void AddCaseMemo(EPlace place, CharacterSO speaker, string conversation)
    {
        if(caseMemo == null)
        {
            caseMemo = new Dictionary<EPlace, Dictionary<CharacterSO, List<string>>>();
        }

        if(!CaseMemo.ContainsKey(place))
        {
            caseMemo[place] = new Dictionary<CharacterSO, List<string>>();
        }

        Dictionary<CharacterSO,List<string>> speakerData = CaseMemo[place];

        if (!speakerData.ContainsKey(speaker))
        {
            speakerData[speaker] = new List<string>();
        }

        //대화 문자열 추가
        speakerData[speaker].Add(conversation);
    }
}

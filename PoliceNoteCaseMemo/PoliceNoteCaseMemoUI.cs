using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EPlaceName.Extensions;
using System.Linq;

public class PoliceNoteCaseMemoUI : UIScene
{
    // �̱��� ���� (�ɼ�)
    public static PoliceNoteCaseMemoUI Instance { get; private set; }

    [SerializeField] private TMP_Dropdown locationDropdown;
    [SerializeField] private SpeakerIconUI[] speakerItems;     // �̸� ��ġ�� ��ȭ�� UI ��� �迭
    [SerializeField] private TMP_Text detailTitleText;

    [SerializeField] private Transform memoSlotParent;
    [SerializeField] private GameObject memoSlotPrefab;

    [SerializeField] private List<CharacterSO> DummyCharacters = new List<CharacterSO>(); //���̵����� ���ۿ� ĳ����

    private PoliceNoteCaseMemoData caseMemoData;

    private List<EPlace> placeList;


    // ���� ���õ� ��ȭ���
    private EPlace currentLocation;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        caseMemoData = new PoliceNoteCaseMemoData();
        BuildDummyData();

        foreach(var speaker in speakerItems)
        {
            speaker.Initialize();
            speaker.OnSpeakerClicked.AddListener(UpdateMemoSlotUI);
        }
        SetLocationDropdown();
        locationDropdown.onValueChanged.AddListener(OnLocationChanged);

        OnLocationChanged(0);
    }

    private void BuildDummyData()
    {
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "�ڰ��� ��, ���̴� ���� �� ��. ��ȥ�� ���� �ڳ�� ���׿�.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "�Ƴ��� ������ ������ ����, ���̴� ���� �ϰ� �쿡 �ǻ缼��.\nƯ���� ����, ������ �� �ƹ����� ������ �� �Ը� �ִ� ���κ����� �����̶�� �ϳ׿�?");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "�� �ڰ��� ���� ��ϴ� NJ24�� ����������� ������, �Ը� ũ�� ���� ���� �������̿���. ");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "��ٷο� ��ħ�� ����, ��糪 ���� ������ �γ��� ���̶�, '��̷� ��ϴ� ����'�� �ȼ������̶�� �ϳ�����.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[1], "�׷��� �����ٰ� ����, �մ����� �Ҹ��� ���� ������, ��絵 ���ڴ�� �ϴ� �ǰ�.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[1], "������ ������, ���ڴ� ���. �ð赵 �ʸ� ��, �� �� ä�� ���� �ٴϰ�.\n�׷� ����� �� ������ �����̳� �ϰ� �ɾ�����?");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "���� �� �ڰ��� ����... �״��� ������ ������δ� ������ �ʾҾ��.\n������ ����ϴ� ����̶����, ���� ����� �����.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "���̼����� �԰� ������... �׳� '���ÿ� ����ϴ� ������ ������'ó�� ���̰� ���� ���� �� ���ƿ�.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "���ÿ� �ڱ� �׷� ����� �����ΰ� �ٸ��ٴ� ������ �ϰ� �;, ��� �ð�� ġ���ϰ� �ְ��. ");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "�Ƴ��� ����� ���� ��... ���� ������ �ƴϵ�, ������ ��� �ͺ��� ������ ���̶� �ؾ� �Ѵٴ� ������ �ǰ��� ������ ���� �ִٰ� �����ؿ�.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "����ó�� å�Ӱ� �ִ� �κη� �̷���� �������� ���̰� ������... ����ó�� ����ϰ� ���̴� �� �ȴ�. ���� �����̶�� �����ؿ�.");
    }

    private void SetLocationDropdown()
    {
        if(locationDropdown == null)
        {
            return;
        }

        placeList = Enum.GetValues(typeof(EPlace)).Cast<EPlace>().ToList();

        var labels = placeList.Select(p => p.GetDescription()).ToList();

        //List<string> locations = new List<string>();

        //if(caseMemoData != null && caseMemoData.CaseMemo != null)
        //{
        //    foreach(EPlace place in caseMemoData.CaseMemo.Keys) 
        //    {
        //        locations.Add(place.ToString());
        //    }
        //}


        locationDropdown.ClearOptions();
        locationDropdown.AddOptions(labels);
    }

    void UpdateSpeakerUI(EPlace place)
    {
        if(caseMemoData == null)
        {
            return;
        }

        List<CharacterSO> speakers = new List<CharacterSO>();


        if(caseMemoData.CaseMemo.ContainsKey(place))
        {
            foreach(var speaker in caseMemoData.CaseMemo[place].Keys)
            {
                speakers.Add(speaker);
            }
        }
        else
        {
            Debug.LogWarning("���õ� ��ҿ� �ش��ϴ� �����Ͱ� �����ϴ�");
        }

        int count = 0;
        foreach (var speaker in speakers)
        {
            if (count < speakerItems.Length)
            {
                speakerItems[count].Setup(speaker);
                speakerItems[count].gameObject.SetActive(true);
            }
            count++;
        }

        // �̸� ��ġ�� UI ��� �� ��ȭ�� �����Ͱ� ���� ������ ��Ȱ��ȭ
        for (int i = count; i < speakerItems.Length; i++)
        {
            speakerItems[i].gameObject.SetActive(false);
        }
    }

    void OnLocationChanged(int index)
    {
        EPlace selected = placeList[index];
        currentLocation = selected;

        UpdateSpeakerUI(selected);
    }

    public void UpdateMemoSlotUI(string speakerName)
    {
        Debug.Log("Update is called");
        ClearMemoSlotUI();

        if (!caseMemoData.CaseMemo.ContainsKey(currentLocation))
        {
            return;
        }

        Dictionary<CharacterSO, List<string>> convData = caseMemoData.CaseMemo[currentLocation];
    
        foreach(var data in convData)
        {
            CharacterSO speaker = data.Key;
            if(speaker.Name == speakerName)
            {
                List<string> convesrsations = data.Value;
                foreach(string conv in convesrsations)
                {
                    GameObject slot = Instantiate(memoSlotPrefab, memoSlotParent);
                    MemoSlotUI slotUI = slot.GetComponent<MemoSlotUI>();
                    if(slotUI != null)
                    {
                        slotUI.Initialize(currentLocation.GetDescription(), speaker, conv);
                    }
                }
            }
        }

        detailTitleText.text = speakerName + " - " + currentLocation.GetDescription();
    }

    private void ClearMemoSlotUI()
    {
        Debug.Log("Hi");
        foreach (Transform child in memoSlotParent)
        {
            Destroy(child.gameObject);
        }
    }
    // ��ȭ��ҿ� �ش��ϴ� ��ȭ�� ����� �̸� ��ġ�� UI ��ҿ� ������Ʈ
    //void UpdateSpeakerList(string location)
    //{
    //    if (conversationDatabase.ContainsKey(location))
    //    {
    //        Dictionary<string, List<ConversationData>> speakerDict = conversationDatabase[location];
    //        List<string> speakerNames = new List<string>(speakerDict.Keys);

    //        for (int i = 0; i < speakerItems.Length; i++)
    //        {
    //            if (i < speakerNames.Count)
    //            {
    //                ConversationData firstConv = speakerDict[speakerNames[i]][0];
    //                // ScriptableObject �� SpeakerData�� �״�� ���
    //                SpeakerData speaker = new SpeakerData(firstConv.speaker.name, firstConv.speaker.speakerSprite);
    //                speakerItems[i].Setup(speaker);
    //                speakerItems[i].gameObject.SetActive(true);
    //            }
    //            else
    //            {
    //                speakerItems[i].gameObject.SetActive(false);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        // �ش� ��ȭ��ҿ� �����Ͱ� ������ ��� ����Ŀ UI ����
    //        foreach (var item in speakerItems)
    //            item.gameObject.SetActive(false);
    //    }
    //}

    // UI���� ��ȭ�� ��ư Ŭ�� �� ȣ��
    //public void DisplayConversationForSpeaker(SpeakerData speaker)
    //{
    //    if (conversationDatabase.ContainsKey(currentLocation) &&
    //        conversationDatabase[currentLocation].ContainsKey(speaker.name))
    //    {
    //        List<ConversationData> convList = conversationDatabase[currentLocation][speaker.name];
    //        MemoSlotManager.Instance.DisplayConversation(convList);
    //    }
    //}
}
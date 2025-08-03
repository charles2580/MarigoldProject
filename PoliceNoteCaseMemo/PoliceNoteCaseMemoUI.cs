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
    // 싱글톤 패턴 (옵션)
    public static PoliceNoteCaseMemoUI Instance { get; private set; }

    [SerializeField] private TMP_Dropdown locationDropdown;
    [SerializeField] private SpeakerIconUI[] speakerItems;     // 미리 배치된 발화자 UI 요소 배열
    [SerializeField] private TMP_Text detailTitleText;

    [SerializeField] private Transform memoSlotParent;
    [SerializeField] private GameObject memoSlotPrefab;

    [SerializeField] private List<CharacterSO> DummyCharacters = new List<CharacterSO>(); //더미데이터 제작용 캐릭터

    private PoliceNoteCaseMemoData caseMemoData;

    private List<EPlace> placeList;


    // 현재 선택된 대화장소
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
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "박강헌 씨, 나이는 서른 세 살. 기혼에 아직 자녀는 없네요.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "아내분 성함은 권지윤 씨고, 나이는 서른 일곱 살에 의사세요.\n특이한 점은, 권지윤 씨 아버지가 지역에 꽤 규모가 있는 개인병원의 원장이라고 하네요?");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "또 박강헌 씨가 운영하는 NJ24는 프랜차이즈긴 하지만, 규모가 크지 않은 영세 편의점이에요. ");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "까다로운 방침도 없고, 행사나 서비스 관리도 널널한 편이라, '취미로 운영하는 가게'로 안성맞춤이라고 하나봐요.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[1], "그래서 귀찮다고 쉬고, 손님한테 소리도 빽빽 지르고, 장사도 제멋대로 하는 건가.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[1], "병원장 사위라, 팔자는 폈네. 시계도 필립 사, 집 한 채를 차고 다니고.\n그런 사람이 왜 편의점 사장이나 하고 앉아있지?");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "제가 본 박강헌 씨는... 그다지 성실한 사람으로는 보이지 않았어요.\n질문에 대답하는 모습이라든지, 영업 방식을 보면요.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "와이셔츠는 입고 있지만... 그냥 '정시에 출근하는 성실한 직장인'처럼 보이고 싶을 뿐인 거 같아요.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "동시에 자긴 그런 평범한 직장인과 다르다는 어필을 하고 싶어서, 비싼 시계로 치장하고 있고요. ");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "아내의 재력을 봤을 때... 본인 의지든 아니든, 집에서 노는 것보단 나가서 일이라도 해야 한다는 쪽으로 의견이 몰렸을 수도 있다고 생각해요.");
        caseMemoData.AddCaseMemo(EPlace.KLDetectiveAgency, DummyCharacters[0], "남들처럼 책임감 있는 부부로 이루어진 가정으로 보이고 싶지만... 남들처럼 평범하게 보이는 건 싫다. 흔한 감정이라고 생각해요.");
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
            Debug.LogWarning("선택된 장소에 해당하는 데이터가 없습니다");
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

        // 미리 배치된 UI 요소 중 발화자 데이터가 없는 슬롯은 비활성화
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
    // 대화장소에 해당하는 발화자 목록을 미리 배치된 UI 요소에 업데이트
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
    //                // ScriptableObject 내 SpeakerData를 그대로 사용
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
    //        // 해당 대화장소에 데이터가 없으면 모든 스피커 UI 숨김
    //        foreach (var item in speakerItems)
    //            item.gameObject.SetActive(false);
    //    }
    //}

    // UI에서 발화자 버튼 클릭 시 호출
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
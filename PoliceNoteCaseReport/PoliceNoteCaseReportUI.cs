using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utage;
using UtageExtensions;

public class PoliceNoteCaseReportUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;

    [SerializeField] private Button prevPageButton;
    [SerializeField] private Button nextPageButton;

    [SerializeField] private Button reportButton;

    public PoliceNoteCaseReportData reportData = new PoliceNoteCaseReportData();
    private int currentPage = 0;

    private IReportEvaluator evaluator;
    public virtual AdvEngine Engine => this.GetAdvEngineCacheFindIfMissing(ref engine);
    protected AdvEngine engine;

    public void Awake()
    {
        evaluator = new Episode1ReportEvaluator();
    }

    public void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        ShowPage(currentPage);
        prevPageButton.onClick.AddListener(PrevPage);
        nextPageButton.onClick.AddListener(NextPage);

        reportButton.onClick.AddListener(OnReportClicked);
    }

    public void ShowPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pages.Count)
        {
            return;
        }

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == pageIndex);
        }

        currentPage = pageIndex;

        var layoutGroup = pages[pageIndex].GetComponentInChildren<VerticalLayoutGroup>();
        if (layoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }
    }
    public void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            ShowPage(currentPage + 1);
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            ShowPage(currentPage - 1);
        }
    }

    private void OnReportClicked()
    {
        string nextScenarioLabel = evaluator.Evaluate(reportData);

        StartCoroutine(CoLaunchScenario(nextScenarioLabel));
    }

    private IEnumerator CoLaunchScenario(string label)
    {

        while (Engine.IsWaitBootLoading)
            yield return null;
    
        Engine.StartGame(label);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoliceNoteCaseReportUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;

    [SerializeField] private Button prevPageButton;
    [SerializeField] private Button nextPageButton;

    public PoliceNoteCaseReportData reportData = new PoliceNoteCaseReportData();
    private int currentPage = 0;

    public void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        ShowPage(currentPage);
        prevPageButton.onClick.AddListener(PrevPage);
        nextPageButton.onClick.AddListener(NextPage);
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
}

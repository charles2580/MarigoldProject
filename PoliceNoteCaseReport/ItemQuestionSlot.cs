using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class ItemQuestionSlot : MonoBehaviour
{
    [SerializeField] private int questionID;
    [SerializeField] private Button selectButton;
    [SerializeField] private Image selectItemImage;

    [SerializeField] private PoliceNoteCaseReportUI ReportUI;

    [SerializeField] private GameObject selectItemPopUp;
    [SerializeField] private RectTransform parent;

    [SerializeField] private GameObject itemSlotPrefab;

    private PoliceNoteInventoryUI inventoryUI;
    private readonly List<ItemSlotUI> selectItemList = new List<ItemSlotUI>();

    public void Start()
    {
        if(inventoryUI == null)
        {
            inventoryUI = PoliceNoteInventoryUI.Instance;
        }
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OpenSelectItemPopUp);
    }

    public void OpenSelectItemPopUp()
    {
        foreach (var slot in selectItemList)
        { 
            Destroy(slot.gameObject);
        }
        selectItemList.Clear();

        selectItemPopUp.gameObject.SetActive(true);

        var inventories = new[] { 
            inventoryUI.GetPossessionInventory(),
            inventoryUI.GetEvidenceInventory()
        };

        foreach (Inventory inventory in inventories)
        {
            if(inventory == null)
            {
                Debug.Log("inventory is empty");
                continue;
            }
            foreach (Item item in inventory.ItemList)
            {
                GameObject newItem = Instantiate(itemSlotPrefab, parent);
                ItemSlotUI slot = newItem.GetComponent<ItemSlotUI>();
                slot.Initialize(item, inventoryUI);

                slot.SetQuestionMode(OnSelectItem);
                selectItemList.Add(slot);
                Debug.Log("create slot");
            }
        }
    }

    private void OnSelectItem(Item item)
    {
        if(selectItemImage == null)
        {
            return;
        }

        selectItemImage.sprite = item.ItemData.Icon;
        selectItemImage.gameObject.SetActive(true);

        selectItemPopUp.SetActive(false);
    }
    
}

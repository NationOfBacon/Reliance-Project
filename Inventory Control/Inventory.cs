using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int allSlots;
    public GameObject[] slot;
    public List<Slot> slotScripts = new List<Slot>();
    private int counter;

    private UIManager uiManager;
    private InventoryManager invManager;
    private GameObject equipInv;

    public int creditCurrency;
    private TextMeshProUGUI creditCurrencyText;
    public int datumCurrency;
    private TextMeshProUGUI datumCurrencyText;

    private void Awake()
    {
        uiManager = GameObject.Find("Persistent Object").GetComponent<UIManager>();
        invManager = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
        equipInv = GameObject.Find("HUBUI/Loadout Panel/Equipment Grid");
        creditCurrencyText = GameObject.Find("HUBUI/Inventory Panel/Currency Value Horizontal Group/# Text").GetComponent<TextMeshProUGUI>();
        datumCurrencyText = GameObject.Find("HUBUI/Inventory Panel/Currency Value Horizontal Group/# Text 1").GetComponent<TextMeshProUGUI>();
        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {

            if (transform.GetChild(i).tag == "Slot")
            {
                slot[i] = transform.GetChild(i).gameObject;
                slotScripts.Add(transform.GetChild(i).GetComponent<Slot>());

                if (slot[i].GetComponent<Slot>().item == null)
                    slot[i].GetComponent<Slot>().empty = true;
            }
            else
                slot[i] = null;
        }
    }

    void Start()
    {
        UpdateSlotUI();
    }

    void Update()
    {
        //if(uiSet == false && invManager.gridFound)
        //{
        //    uiManager.HUBUISet();
        //    uiSet = true;
        //    invManager.inv.uiSet = true;
        //    invManager.inv2.uiSet = true;
        //    invManager.equipInv.uiSet = true;
        //    invManager.sellInv.uiSet = true;
        //}
        creditCurrency = invManager.currency;
        creditCurrencyText.text = creditCurrency.ToString();
        datumCurrency = invManager.currency1;
        datumCurrencyText.text = datumCurrency.ToString();
    }

    public void ItemRetrieve(Item.itemStruct targetItem)
    {
        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().empty)
            {
                //add item to slot
                targetItem.pickedUp = true;

                slot[i].GetComponent<Slot>().item = targetItem.thisObject;
                slot[i].GetComponent<Slot>().icon = targetItem.icon;
                slot[i].GetComponent<Slot>().type = targetItem.type;
                slot[i].GetComponent<Slot>().description = targetItem.description;
                slot[i].GetComponent<Slot>().ID = targetItem.ID;

                slot[i].GetComponent<Slot>().bonusStats[0] = targetItem.strBonus;
                slot[i].GetComponent<Slot>().bonusStats[1] = targetItem.elecBonus;
                slot[i].GetComponent<Slot>().bonusStats[2] = targetItem.agiBonus;
                slot[i].GetComponent<Slot>().bonusStats[3] = targetItem.combatBonus;
                slot[i].GetComponent<Slot>().bonusStats[4] = targetItem.scanBonus;
                slot[i].GetComponent<Slot>().bonusStats[5] = targetItem.effBonus;
                slot[i].GetComponent<Slot>().bonusStats[6] = targetItem.hitBonus;
                slot[i].GetComponent<Slot>().bonusStats[7] = targetItem.retBonus;
                slot[i].GetComponent<Slot>().bonusStats[8] = targetItem.ventBonus;
                slot[i].GetComponent<Slot>().bonusStats[9] = targetItem.objMBonus;
                slot[i].GetComponent<Slot>().bonusStats[10] = targetItem.regenBonus;
                slot[i].GetComponent<Slot>().bonusStats[11] = targetItem.luckBonus;
                slot[i].GetComponent<Slot>().bonusStats[12] = targetItem.teamPBonus;
                slot[i].GetComponent<Slot>().bonusStats[13] = targetItem.infoBonus;

                targetItem.thisObject.SetActive(false);

                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;

                return;
            }
        }
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < allSlots; i++)
        {
            slot[i].GetComponent<Slot>().UpdateSlot();
        }
    }
}

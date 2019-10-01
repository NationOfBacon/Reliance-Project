using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICommands : MonoBehaviour
{
    private UIManager uiMgr;
    public string level1;
    private InventoryManager invMgr;

    private void Awake()
    {
        uiMgr = GameObject.Find("Persistent Object").GetComponent<UIManager>();
        invMgr = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
    }
    public void GoToHUB()
    {
        SceneManager.LoadScene("HUB scene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToLevel1()
    {
        SceneManager.LoadScene(level1); //cant directly load levels from within them without causing UI issues and loss of references
    }

    public void GoToTestScene()
    {
        SceneManager.LoadScene(3);
    }

    public void CloseEscapeMenu()
    {
        uiMgr.escapeMenuOpen = false;
    }

    public void QuitGame()
    {
        Debug.Log("Application was closed");
        Application.Quit();
    }

    public void BuyNewMissions() //called when buying new missions from the mission screen
    {
        var invMgrRef = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
        if(invMgrRef.currency >= 30)
        {
            invMgrRef.currency -= 30;
            invMgrRef.inv.creditCurrency = invMgrRef.currency;
        }
    }

    public void UpdateUI() //called to update the UI when moving on to new screens
    {
        invMgr.inv.UpdateSlotUI();
        invMgr.inv2.UpdateSlotUI();
        invMgr.equipInv.UpdateSlotUI();
        invMgr.UpdateEquipment();
    }
}

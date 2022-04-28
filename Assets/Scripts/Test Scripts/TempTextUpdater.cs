using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempTextUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text textToUpdate;
    [SerializeField] private bool repairCost;
    [SerializeField] private bool currentMoney;
    [SerializeField] private bool currentTime;


    private void Start()
    {
        UpdateText();

        GameManager.OnUpdatePlayerCurrencies += UpdateText;
    }

    private void OnDestroy()
    {
        GameManager.OnUpdatePlayerCurrencies -= UpdateText;
    }

    private void UpdateText()
    {
        if (repairCost)
            textToUpdate.text = DowntimeManager.instance.RepairCost.ToString();

        if (currentMoney)
            textToUpdate.text = GameManager.instance.PlayerBankController.GetPlayerCurrency().ToString();

        if (currentTime)
            textToUpdate.text = GameManager.instance.PlayerBankController.GetPlayerTime().ToString();
    }
}

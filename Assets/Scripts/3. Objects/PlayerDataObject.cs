using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataObject : MonoBehaviour
{
    private float timeLeftToSpend;
    private float currencyToSpend;

    public float TimeLeftToSpend { get => timeLeftToSpend; }
    public float CurrencyToSpend { get => currencyToSpend; }
}

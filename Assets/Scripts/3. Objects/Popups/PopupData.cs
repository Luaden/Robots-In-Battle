using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupData
{
    private string name;
    private string description;
    private Image image;
    private int statValue;
    private int energy;

    public PopupData(string name,
                    string description,
                    Image image)
    {
        this.name = name;
        this.description = description;
        this.image = image;
    }
    public PopupData(string name,
                     string description)
    {
        this.name = name;
        this.description = description;
    }

    public PopupData(int statValue,
                     int energy)
    {
        this.statValue = statValue;
        this.energy = energy;
    }

    public string Name { get => name; }
    public string Description { get => description; }
    public int StatValue { get => statValue; }
    public int Energy { get => energy; }

}

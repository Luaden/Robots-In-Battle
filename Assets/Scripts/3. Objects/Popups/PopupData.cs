using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupData
{
    private string name;
    private string description;
    private Image image;

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

    public string Name { get => name; }
    public string Description { get => description; }

}

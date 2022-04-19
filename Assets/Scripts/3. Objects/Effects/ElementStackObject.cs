using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementStackObject
{
    private ElementType elementType;
    private int elementStacks;

    public ElementType ElementType { get => elementType; set => elementType = value; }
    public int ElementStacks { get => elementStacks; set => elementStacks = value; }
}

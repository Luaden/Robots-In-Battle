using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationQueueObject 
{
    public CharacterSelect firstMech;
    public AnimationType firstAnimation;
    public CharacterSelect secondMech;
    public AnimationType secondAnimation;

    public AnimationQueueObject(CharacterSelect firstMech, AnimationType firstAnimation, CharacterSelect secondMech, AnimationType secondAnimation)
    {
        this.firstMech = firstMech;
        this.firstAnimation = firstAnimation;
        this.secondMech = secondMech;
        this.secondAnimation = secondAnimation;
    }
}

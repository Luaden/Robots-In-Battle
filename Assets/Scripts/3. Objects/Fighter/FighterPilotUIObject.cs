using UnityEngine;

[System.Serializable]
public class FighterPilotUIObject 
{
    [SerializeField] private Sprite fighterHair;
    [SerializeField] private Sprite fighterEyes;
    [SerializeField] private Sprite fighterNose;
    [SerializeField] private Sprite fighterMouth;
    [SerializeField] private Sprite fighterClothes;
    [SerializeField] private Sprite fighterBody;

    public Sprite FighterHair { get => fighterHair; }
    public Sprite FighterEyes { get => fighterEyes; }
    public Sprite FighterNose { get => fighterNose; }
    public Sprite FighterMouth { get => fighterMouth; }
    public Sprite FighterClothes { get => fighterClothes; }
    public Sprite FighterBody { get => fighterBody; }

    public FighterPilotUIObject(Sprite fighterHair, Sprite fighterEyes, Sprite fighterNose, Sprite fighterMouth, Sprite fighterClothes, Sprite fighterBody)
    {
        this.fighterHair = fighterHair;
        this.fighterEyes = fighterEyes;
        this.fighterNose = fighterNose;
        this.fighterMouth = fighterMouth;
        this.fighterClothes = fighterClothes;
        this.fighterBody = fighterBody;
    }
}

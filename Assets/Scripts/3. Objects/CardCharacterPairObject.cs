public class CardCharacterPairObject
{
    public CardChannelPairObject cardChannelPair;
    public CharacterSelect character;
    public int repeatEffect;

    public CardCharacterPairObject(CardChannelPairObject cardChannelPair, CharacterSelect character, int repeatEffect = 1)
    {
        this.cardChannelPair = cardChannelPair;
        this.character = character;
        this.repeatEffect = repeatEffect;
    }
}

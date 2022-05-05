public class CardCharacterPairObject
{
    public CardChannelPairObject cardChannelPair;
    public CharacterSelect character;

    public CardCharacterPairObject(CardChannelPairObject cardChannelPair, CharacterSelect character)
    {
        this.cardChannelPair = cardChannelPair;
        this.character = character;
    }
}

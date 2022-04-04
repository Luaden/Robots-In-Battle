public class CardChannelPairObject
{
    private CardDataObject card;
    private Channels cardChannel; 

    public CardChannelPairObject(CardDataObject selectedCard, Channels selectedCardChannel)
    {
        this.card = selectedCard;
        this.cardChannel = selectedCardChannel;
    }
}

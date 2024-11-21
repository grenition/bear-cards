namespace Project.Gameplay.Battle.Model.Cards
{
    public enum CardOwner
    {
        player,
        enemy
    }
    public enum CardContainer
    {
        deck,
        hand,
        field,
        spells
    }
    
    public struct CardPosition
    {
        public CardContainer container;
        public CardOwner owner;
        public int index;
        
        public CardPosition(CardContainer container, CardOwner owner, int index)
        {
            this.container = container;
            this.owner = owner;
            this.index = index;
        }
    }
}

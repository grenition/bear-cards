namespace Project.Gameplay.Battle.Cards
{
    public enum CardOwner
    {
        player,
        enemy
    }
    public enum CardContainer
    {
        none,
        deck,
        hand,
        field
    }
    
    public struct CardPosition
    {
        public CardContainer container;
        public CardOwner owner;
        public int position;
        
        public CardPosition(CardContainer container, CardOwner owner, int position)
        {
            this.container = container;
            this.owner = owner;
            this.position = position;
        }
    }
}

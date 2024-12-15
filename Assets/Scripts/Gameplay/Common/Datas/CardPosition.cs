using System;

namespace Project.Gameplay.Common.Datas
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
        spells,
        garbage
    }

    [Serializable]
    public struct CardPosition : IEquatable<CardPosition>
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

        public static CardPosition Garbage() => new CardPosition(CardContainer.garbage, default, default);

        public bool IsPlayerField() => container == CardContainer.field && owner == CardOwner.player;
        public bool IsEnemyField() => container == CardContainer.field && owner == CardOwner.enemy;
        public bool IsPlayerHand() => container == CardContainer.hand && owner == CardOwner.player;
        public bool IsEnemyHand() => container == CardContainer.hand && owner == CardOwner.enemy;
        public bool IsPlayerDeck() => container == CardContainer.deck && owner == CardOwner.player;
        public bool IsEnemyDeck() => container == CardContainer.deck && owner == CardOwner.enemy;
        public bool IsPlayerSpells() => container == CardContainer.spells && owner == CardOwner.player;
        public bool IsEnemySpells() => container == CardContainer.spells && owner == CardOwner.enemy;

        public bool Equals(CardPosition other)
        {
            return container == other.container &&
                   owner == other.owner &&
                   index == other.index;
        }

        public override bool Equals(object obj)
        {
            if (obj is CardPosition other)
                return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(container, owner, index);
        }

        public static bool operator ==(CardPosition left, CardPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CardPosition left, CardPosition right)
        {
            return !(left == right);
        }
    }
}
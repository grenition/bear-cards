using System;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIDeck : MonoBehaviour
    {
        [field: SerializeField] public CardOwner Owner { get; protected set; }

        private void Start()
        {
            UIBattle.Instance.RegisterDeck(this);
        }
        private void OnDestroy()
        {
            UIBattle.Instance.UnregisterDeck(this);
        }
    }
}

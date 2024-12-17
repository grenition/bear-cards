using Assets.Scripts.Map;
using Project.Gameplay.Battle.Model.CardPlayers;
using System.Linq;
using UnityEngine;

namespace Project
{
    public class ProvideToBattle : MonoBehaviour
    {
        [SerializeField] private CardPlayerConfig _cardPlayerConfig;
        [SerializeField] private MapCompositionRoot _compositionRoot;

        private void Awake()
        {
            MapStaticData.SetStartDeck(_cardPlayerConfig.Deck.Select(card => card.name).ToArray());
            MapStaticData.LoadData();
            _compositionRoot.Initialize();
        }
    }
}

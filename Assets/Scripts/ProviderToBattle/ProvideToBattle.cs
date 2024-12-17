using Assets.Scripts.Map;
using Project.Gameplay.Battle.Craft;
using Project.Gameplay.Battle.Model.CardPlayers;
using System.Linq;
using UnityEngine;

namespace Project
{
    public class ProvideToBattle : MonoBehaviour
    {
        [SerializeField] private CardPlayerConfig _cardPlayerConfig;
        [SerializeField] private MapCompositionRoot _compositionRoot;

        private CardCraftConfig[] _receptConfigs;
        private void Awake()
        {
            MapStaticData.SetStartDeck(_cardPlayerConfig.Deck.Select(card => card.name).ToArray());
            MapStaticData.LoadData();

            _receptConfigs = Resources.LoadAll<CardCraftConfig>("Gameplay/Crafts/");
            DialoguesStatic.SetRecepts(_receptConfigs.Select(c => c.name).ToArray());
            _compositionRoot.Initialize();

        }
    }
}

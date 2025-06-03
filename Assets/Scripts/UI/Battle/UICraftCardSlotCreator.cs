using Assets.Scripts.Map;
using GreonAssets.Extensions;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common.Datas;
using Project.UI.Battle;
using System;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UICraftCardSlotCreator : MonoBehaviour
    {
        [Header("Positioning")]
        [SerializeField] private CardContainer _cardsContainerType;
        [SerializeField] private CardOwner _cardsOwner;

        [Header("Visual")]
        [SerializeField] private UICardSlot _cardSlotPrefab;

        [SerializeField] private GameObject CardTypeSlotPrefab;
        private Dictionary<string, Transform> CardTypesSlots = new Dictionary<string, Transform>();

        private void Awake()
        {
            transform.DestroyAllChildrens();

            var deck = MapStaticData.LoadData();

            var slots = BattleController.Model.GetSlotsAtPosition(_cardsOwner, _cardsContainerType);
            int slotIndex = 0;

            string[] types = new string[0];

            foreach (string card in deck.Deck)
            {
                types = ExcludingExpandMassive(types, card);
            }

            string[] sorted = new string[deck.Deck.Length];
            Array.Copy(deck.Deck, sorted, sorted.Length);
            for (int i = 0; i < sorted.Length; i++)
            {
                int minimal = i;
                for (int ii = i + 1; ii < sorted.Length; ii++)
                {
                    if (Array.IndexOf(types, sorted[minimal]) > Array.IndexOf(types, sorted[ii]))
                    {
                        minimal = ii;
                    }
                }

                string temp = sorted[minimal];
                sorted[minimal] = sorted[i];
                sorted[i] = temp;
            }

            foreach (string type in sorted)
            {
                if (type.Contains("spell"))
                {
                    continue;
                }

                if(!CardTypesSlots.ContainsKey(type))
                {
                    Transform slot = Instantiate(CardTypeSlotPrefab, transform).transform;
                    CardTypesSlots.Set(type, slot);

                    var uiSlot = Instantiate(_cardSlotPrefab, slot);
                    uiSlot.Init(slots[slotIndex]);
                    slotIndex++;
                }
                else
                {
                    var uiSlot = Instantiate(_cardSlotPrefab, CardTypesSlots[type]);
                    uiSlot.Init(slots[slotIndex]);
                    slotIndex++;
                }
            }

        }

        private T[] ExcludingExpandMassive<T>(T[] origin, T value)
        {
            foreach (T t in origin)
            {
                if (t.Equals(value))
                {
                    return origin;
                }
            }

            T[] newMassive = new T[origin.Length + 1];

            Array.Copy(origin, newMassive, origin.Length);

            newMassive[newMassive.Length - 1] = value;

            return newMassive;
        }
    }
}

using Project;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Behaviour;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Battle.Model.CardSlots;
using Project.Gameplay.Common.Datas;
using System;
using UnityEngine;

public class BattleDymaicTutorial : MonoBehaviour
{
    [SerializeField] private GameObject All;

    [SerializeField] private CardModel SiliciumCard;

    [SerializeField] private GameObject[] Pages;
    [SerializeField] private int CurrentPage;

    private CardModel Kremniy = null;
    private CardModel[] CanBePlaced = new CardModel[0];

    private void Start()
    {
        if (PlayerPrefs.GetInt($"tutorial_played_battle_tutorial") == 1)
        {
            All.SetActive(false);
            return;
        }
        All.SetActive(true);
    }

    public void NextPage()
    {
        CurrentPage++;
        
        for(int i = 0; i < Pages.Length; i++)
        {
            Pages[i].SetActive(CurrentPage == i);
        }
        
        if(CurrentPage == 4)
        {
            CanBePlaced = new CardModel[0];
            foreach (CardSlotModel slot in (BattleController.Behaviour as StandartBattleBehaviour)._PlayerBehaviour.PlayerModel.Hand)
            {
                if(slot == null || slot.Card == null)
                {
                    continue;
                }

                CanBePlaced = ExpandMassive(CanBePlaced, slot.Card);
                slot.Card.OnTransfered += PlaceCard;
            }
        }

        if(CurrentPage == 8)
        {
            (BattleController.Behaviour as StandartBattleBehaviour)._PlayerBehaviour.PlayerModel.Deck.Insert(0, new CardSlotModel(BattleController.Model, new CardPosition(CardContainer.deck, CardOwner.player, 0), CardSlotPermissions.Deck(CardOwner.player)));
        }
        if(CurrentPage == 9)
        {
            BattleController.Behaviour.OnTurnStarted += TurnStarted;
        }
        if(CurrentPage == 15)
        {
            (BattleController.Behaviour as StandartBattleBehaviour)._PlayerBehaviour.PlayerModel.ModifeHandElectrons(18);
            (BattleController.Behaviour as StandartBattleBehaviour)._PlayerBehaviour.PlayerModel.OnLevelElectronsChanged += ElectronPulled;
        }
        if (CurrentPage == 16)
        {
            (BattleController.Behaviour as StandartBattleBehaviour)._PlayerBehaviour.PlayerModel.ModifeHandElectrons(4);
            foreach (CardSlotModel slot in (BattleController.Behaviour as StandartBattleBehaviour)._PlayerBehaviour.PlayerModel.Hand)
            {
                if(slot.Card.Key == "card_kremniy")
                {
                    Kremniy = slot.Card;
                    Kremniy.OnTransfered += PlaceKremniy;
                    break;
                }
            }
        }

        if(CurrentPage >= Pages.Length)
        {
            PlayerPrefs.SetInt($"tutorial_played_battle_tutorial", 1);
            PlayerPrefs.Save();
            All.SetActive(false);
        }
    }

    public void PulledCard()
    {
        if (CurrentPage == 18)
        {
            NextPage();
        }
    }

    public void PlayerPassTurn()
    {
        if(CurrentPage == 8)
        {
            NextPage();
        }
    }

    public void TurnStarted(CardOwner cardOwner)
    {
        if(cardOwner == CardOwner.player && CurrentPage == 9)
        {
            NextPage();
            BattleController.Behaviour.OnTurnStarted -= TurnStarted;
        }
    }

    public void PlaceCard(CardPosition from, CardPosition to)
    {
        if (CurrentPage == 4 && to.container == CardContainer.field)
        {
            foreach(CardModel card in CanBePlaced)
            {
                card.OnTransfered -= PlaceCard;
            }
            CanBePlaced = new CardModel[0];
            NextPage();
        }
    }
    public void PlaceKremniy(CardPosition from, CardPosition to)
    {
        if(CurrentPage == 16 && to.container == CardContainer.field) 
        {
            NextPage();
            Kremniy.OnTransfered -= PlaceKremniy;
        }
    }

    public void ElectronPulled(int count)
    {
        if((BattleController.Behaviour as StandartBattleBehaviour)._PlayerBehaviour.PlayerModel.Level == 3 && CurrentPage == 15)
        {
            NextPage();
            (BattleController.Behaviour as StandartBattleBehaviour)._PlayerBehaviour.PlayerModel.OnLevelElectronsChanged -= ElectronPulled;
        }
    }

    public T[] ExpandMassive<T>(T[] origin, T value)
    {
        T[] newMassive = new T[origin.Length + 1];

        Array.Copy(origin, newMassive, origin.Length);

        newMassive[newMassive.Length - 1] = value;

        return newMassive;
    }
}

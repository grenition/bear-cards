using System;
using Project.Gameplay.Battle;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIPlayerHand : UICardsHand
    {
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                AddCards(BattleController.Model.Player.Hand);
        }
    }
}

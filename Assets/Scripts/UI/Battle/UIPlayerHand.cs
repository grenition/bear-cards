using System;
using System.Linq;
using Project.Gameplay.Battle;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIPlayerHand : UICardsHand
    {
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                AddDynamicSlots(BattleController.Model.Player.Hand);
        }
    }
}

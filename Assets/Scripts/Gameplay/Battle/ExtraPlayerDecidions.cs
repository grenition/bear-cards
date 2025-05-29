using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Behaviour;
using UnityEngine;

namespace Project
{
    public class ExtraPlayerDecidions : MonoBehaviour
    {
        [SerializeField] private BattleController BattleController;
        [SerializeField] private int PullCost;

        public void PullNextCard()
        {
            if (BattleController.Behaviour.Model.Player.HandElectrons >= PullCost)
            {
                BattleController.Behaviour.Model.Player.TransferCardFromDeckToHand();
                BattleController.Behaviour.Model.Player.ModifeHandElectrons(-PullCost);
            }
        }

        public void ElectronsToLevel()
        {
            if (BattleController.Behaviour.Model.Player.HandElectrons >= 1)
            {
                BattleController.Behaviour.Model.Player.ModifyLevelElectrons(1);
                BattleController.Behaviour.Model.Player.ModifeHandElectrons(-1);
            }
        }
    }
}

using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Behaviour;
using UnityEngine;

namespace Project
{
    public class ExtraPlayerDecidions : MonoBehaviour
    {
        [SerializeField] private BattleController BattleController;
        [SerializeField] private int PullCost;

        [Space]
        [SerializeField] private BattleDymaicTutorial BattleDymaicTutorial;

        public void PullNextCard()
        {
            if (BattleController.Behaviour.Model.Player.HandElectrons >= PullCost)
            {
                if(BattleController.Behaviour.Model.Player.TransferCardFromDeckToHand())
                {
                    BattleController.Behaviour.Model.Player.ModifeHandElectrons(-PullCost);
                    BattleDymaicTutorial.PulledCard();
                }
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

using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Behaviour;
using Project.Gameplay.Battle.Model.CardPlayers;
using UnityEngine;
using UnityEngine.Localization;

public class BattleInfoUI : MonoBehaviour
{
    [SerializeField] private BattleController BattleController;

    [SerializeField] private Tipper LevelTipper;
    [SerializeField] private LocalizedString LocalizedLevelTip;

    private CardPlayerModel CardPlayerModel;

    private void Start()
    {
        CardPlayerModel = ((StandartBattleBehaviour)BattleController.Behaviour)._PlayerBehaviour.PlayerModel;
        CardPlayerModel.OnLevelElectronsChanged += ElectronsChanged;

        ElectronsChanged(0);
    }

    public void ElectronsChanged(int count)
    {
        int electronsToNextLevel = CardPlayerModel._ElectronsToNextLevel;
        LevelTipper._Info = LocalizedLevelTip.GetLocalizedString().Replace("{lv}", $"{CardPlayerModel.Level}").Replace("{ec}", $"{CardPlayerModel.LevelElectrons}").Replace("{enl}", $"{(electronsToNextLevel == -1 ? "MAX" : electronsToNextLevel)}");
    }
}

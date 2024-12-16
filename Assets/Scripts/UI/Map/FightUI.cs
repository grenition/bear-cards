using Assets.Scripts.Map;
using UnityEngine;

namespace Project
{
    public class FightUI : MapPanelUI
    {
        public override void InteractComplited()
        {
            Debug.Log("enemy fight");

            ScneneLoaderStatic.LoadSceneAsync("BattleScene");
        }
    }
}

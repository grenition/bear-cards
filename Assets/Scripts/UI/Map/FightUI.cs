using Assets.Scripts.Map;
using System;
using UnityEngine;
using UnityEngine.UI;

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

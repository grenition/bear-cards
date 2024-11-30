using UnityEngine;

namespace Project
{
    public class GiveCardUI : MapPanelUI
    {
        public override void InteractComplited()
        {
            Debug.Log("Card was added");
        }
    }
}

using UnityEngine;

namespace Project.UI.Battle
{
    public class UICardSlot : MonoBehaviour
    {
        public void PlaceCard(UICardMovement card)
        {
            card.transform.position = transform.position;
        }
    }
}

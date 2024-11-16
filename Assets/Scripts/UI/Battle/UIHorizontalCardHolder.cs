using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIHorizontalCardHolder : MonoBehaviour
    {
        [SerializeField] private UICardMovement selectedCard;
        [SerializeReference] private UICardMovement hoveredCard;

        [SerializeField] private GameObject slotPrefab;
        private RectTransform rect;

        [Header("Spawn Settings")]
        [SerializeField] private int cardsToSpawn = 7;
        public List<UICardMovement> cards;

        bool isCrossing = false;
        [SerializeField] private bool tweenCardReturn = true;

        void Start()
        {
            for (int i = 0; i < cardsToSpawn; i++)
            {
                Instantiate(slotPrefab, transform);
            }

            rect = GetComponent<RectTransform>();
            cards = GetComponentsInChildren<UICardMovement>().ToList();

            int cardCount = 0;

            foreach (UICardMovement card in cards)
            {
                card.PointerEnterEvent.AddListener(CardPointerEnter);
                card.PointerExitEvent.AddListener(CardPointerExit);
                card.BeginDragEvent.AddListener(BeginDrag);
                card.EndDragEvent.AddListener(EndDrag);
                card.name = cardCount.ToString();
                cardCount++;
            }

            StartCoroutine(Frame());

            IEnumerator Frame()
            {
                yield return new WaitForSecondsRealtime(.1f);
                for (int i = 0; i < cards.Count; i++)
                {
                    if (cards[i].uiCardVisual != null)
                        cards[i].uiCardVisual.UpdateIndex(transform.childCount);
                }
            }
        }

        private void BeginDrag(UICardMovement card)
        {
            selectedCard = card;
        }


        void EndDrag(UICardMovement card)
        {
            if (selectedCard == null)
                return;
            
            rect.sizeDelta += Vector2.right;
            rect.sizeDelta -= Vector2.right;

        }

        void CardPointerEnter(UICardMovement card)
        {
            hoveredCard = card;
        }

        void CardPointerExit(UICardMovement card)
        {
            hoveredCard = null;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if (hoveredCard != null)
                {
                    Destroy(hoveredCard.transform.parent.gameObject);
                    cards.Remove(hoveredCard);

                }
            }
            
            if (selectedCard == null)
                return;

            if (isCrossing)
                return;

            for (int i = 0; i < cards.Count; i++)
            {

                if (selectedCard.transform.position.x > cards[i].transform.position.x)
                {
                    if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                    {
                        Swap(i);
                        break;
                    }
                }

                if (selectedCard.transform.position.x < cards[i].transform.position.x)
                {
                    if (selectedCard.ParentIndex() > cards[i].ParentIndex())
                    {
                        Swap(i);
                        break;
                    }
                }
            }
        }

        void Swap(int index)
        {
            isCrossing = true;

            Transform focusedParent = selectedCard.transform.parent;
            Transform crossedParent = cards[index].transform.parent;

            cards[index].transform.SetParent(focusedParent);
            selectedCard.transform.SetParent(crossedParent);

            isCrossing = false;

            if (cards[index].uiCardVisual == null)
                return;

            bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
            cards[index].uiCardVisual.Swap(swapIsRight ? -1 : 1);

            //Updated Visual Indexes
            foreach (UICardMovement card in cards)
            {
                card.uiCardVisual.UpdateIndex(transform.childCount);
            }
        }

    }
}

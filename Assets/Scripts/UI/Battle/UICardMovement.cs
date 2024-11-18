using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UICardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        private Canvas canvas;
        private Image imageComponent;
        private UIVisualCardsHandler visualHandler;
        private Vector3 offset;
        private CanvasGroup canvasGroup;
        private Vector3 startPosition;
        private UICardSlot cardSlot;

        [field: Header("Movement")]
        [field: SerializeField] public bool Interactable { get; set; } = true;
        [SerializeField] private float moveSpeedLimit = 50;
        [SerializeField] private bool returnToHoverStartPosition = true;

        [Header("Visual")]
        [SerializeField] private bool instantiateVisual = true;
        [SerializeField] private GameObject cardVisualPrefab;
        [HideInInspector] public UICardVisual uiCardVisual;

        [Header("States")]
        public bool isHovering;
        public bool isDragging;
        [HideInInspector] public bool wasDragged;

        [Header("Events")]
        [HideInInspector] public UnityEvent<UICardMovement> PointerEnterEvent;
        [HideInInspector] public UnityEvent<UICardMovement> PointerExitEvent;
        [HideInInspector] public UnityEvent<UICardMovement, bool> PointerUpEvent;
        [HideInInspector] public UnityEvent<UICardMovement> PointerDownEvent;
        [HideInInspector] public UnityEvent<UICardMovement> BeginDragEvent;
        [HideInInspector] public UnityEvent<UICardMovement> EndDragEvent;
        [HideInInspector] public UnityEvent<UICardMovement, UICardSlot> SlotEnterEvent;
        [HideInInspector] public UnityEvent<UICardMovement, UICardSlot> SlotExitEvent;

        private bool pointerEntered = false;
        private bool pointerPressed = false;
        public UICardSlot slotUnderCursor;
        private GraphicRaycaster raycaster;
        
        void Start()
        {
            canvas = GetComponentInParent<Canvas>();
            imageComponent = GetComponent<Image>();
            canvasGroup = GetComponent<CanvasGroup>();
            raycaster = canvas.GetComponent<GraphicRaycaster>();


            if (!instantiateVisual) return;
            if(!cardVisualPrefab || UIVisualCardsHandler.instance == null) return;
            
            visualHandler = UIVisualCardsHandler.instance;
            uiCardVisual = Instantiate(cardVisualPrefab, visualHandler ? visualHandler.transform : canvas.transform).GetComponent<UICardVisual>();
            uiCardVisual.Initialize(this);
        }

        void Update()
        {
            ClampPosition();

            if (isDragging)
            {
                Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
                Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
                Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
                transform.Translate(velocity * Time.deltaTime);
            }

            if (!isHovering && !isDragging && cardSlot != null)
                transform.position = cardSlot.transform.position;
        }

        void ClampPosition()
        {
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!Interactable)
                return;
            
            BeginDragEvent.Invoke(this);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = mousePosition - (Vector2)transform.position;
            isDragging = true;
            canvas.GetComponent<GraphicRaycaster>().enabled = false;
            imageComponent.raycastTarget = false;

            wasDragged = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var newSlot = GetAvailableCardSlotUnderCursor(eventData);

            if (newSlot == null)
            {
                if(slotUnderCursor != null)
                    SlotExitEvent?.Invoke(this, slotUnderCursor);
                slotUnderCursor = null;
                return;
            }

            if(newSlot == slotUnderCursor) return;
            if(slotUnderCursor != null)
                SlotExitEvent?.Invoke(this, slotUnderCursor);

            slotUnderCursor = newSlot;
            if(Interactable)
                SlotEnterEvent?.Invoke(this, slotUnderCursor);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!Interactable && !isDragging)
                return;
            
            EndDragEvent.Invoke(this);
            isDragging = false;
            canvas.GetComponent<GraphicRaycaster>().enabled = true;
            imageComponent.raycastTarget = true;

            StartCoroutine(FrameWait());

            IEnumerator FrameWait()
            {
                yield return new WaitForEndOfFrame();
                wasDragged = false;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Interactable)
                return;
            
            pointerEntered = true;
            PointerEnterEvent.Invoke(this);
            isHovering = true;
            startPosition = transform.position;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!Interactable && !pointerEntered)
                return;
            
            pointerEntered = false;
            PointerExitEvent.Invoke(this);
            isHovering = false;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable)
                return;
            
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            pointerPressed = true;
            slotUnderCursor = null;
            PointerDownEvent.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!Interactable && !pointerPressed)
                return;
            
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            pointerPressed = false;

            slotUnderCursor = GetAvailableCardSlotUnderCursor(eventData);

            var cardPlaced = false;
            if (slotUnderCursor != null && slotUnderCursor.PlayerCanDropCard)
            {
                cardPlaced = TryPlaceCard(slotUnderCursor);
            }
            if(!cardPlaced)
                transform.DOMove(cardSlot ? cardSlot.transform.position : startPosition, 0.15f).SetEase(Ease.OutBack);
            
            PointerUpEvent?.Invoke(this, false);
        }

        public bool TryPlaceCard(UICardSlot slot)
        {
            if (slot == null) return false;

            if (slot.TryPlaceCard(this))
            {
                cardSlot?.TryRemoveCard(this);
                cardSlot = slot;
                return true;
            }

            return false;
        }
        
        public int SiblingAmount()
        {
            return cardSlot && cardSlot.AllowCardCurvePositioning ? cardSlot.transform.parent.childCount - 1 : 0;
        }

        public int ParentIndex()
        {
            return cardSlot ? cardSlot.transform.GetSiblingIndex() : 0;
        }

        public float NormalizedPosition()
        {
            return cardSlot ? UIExtensionMethods.Remap((float)ParentIndex(), 0, (float)(cardSlot.transform.parent.childCount - 1), 0, 1) : 0;
        }

        private void OnDestroy()
        {
            if(uiCardVisual != null)
                Destroy(uiCardVisual.gameObject);
        }
        private UICardSlot GetAvailableCardSlotUnderCursor(PointerEventData eventData)
        {
            var raycastList = new List<RaycastResult>();
            canvasGroup.blocksRaycasts = false;

            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = eventData.position
            };
            raycaster.Raycast(pointerEventData, raycastList);
            canvasGroup.blocksRaycasts = true;
            
            foreach (var result in raycastList)
            {
                if (result.gameObject.TryGetComponent(out UICardSlot slot) && slot.IsAvailable)
                    return slot;
            }
            
            return null;
        }
        
    }
}

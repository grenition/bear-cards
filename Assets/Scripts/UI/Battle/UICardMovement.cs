using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
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
        [SerializeField] private bool instantiateVisual = true;
        private UIVisualCardsHandler visualHandler;
        private Vector3 offset;
        private CanvasGroup canvasGroup;
        private Vector3 startPosition;

        [Header("Movement")]
        [SerializeField] private float moveSpeedLimit = 50;
        [SerializeField] private bool returnToHoverStartPosition = true;

        [Header("Selection")]
        public bool selected;
        public float selectionOffset = 50;
        private float pointerDownTime;
        private float pointerUpTime;

        [Header("Visual")]
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
        [HideInInspector] public UnityEvent<UICardMovement, bool> SelectEvent;

        void Start()
        {
            canvas = GetComponentInParent<Canvas>();
            imageComponent = GetComponent<Image>();
            canvasGroup = GetComponent<CanvasGroup>();

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
        }

        public void OnEndDrag(PointerEventData eventData)
        {
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
            PointerEnterEvent.Invoke(this);
            isHovering = true;
            startPosition = transform.position;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExitEvent.Invoke(this);
            isHovering = false;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            PointerDownEvent.Invoke(this);
            pointerDownTime = Time.time;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            pointerUpTime = Time.time;

            PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > .2f);

            var raycaster = canvas.GetComponent<GraphicRaycaster>();
            var raycastList = new List<RaycastResult>();
            canvasGroup.blocksRaycasts = false;

            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = eventData.position
            };
            raycaster.Raycast(pointerEventData, raycastList);

            var wasCardSlot = false;
            foreach (var result in raycastList)
            {
                if (result.gameObject.TryGetComponent(out UICardSlot cardSlot))
                {
                    cardSlot.PlaceCard(this);
                    wasCardSlot = true;
                    break;
                }
            }
            if (!wasCardSlot)
            {
                transform.DOMove(startPosition, 0.15f).SetEase(Ease.OutBack);
            }

            canvasGroup.blocksRaycasts = true;
            
            if (pointerUpTime - pointerDownTime > .2f)
                return;

            if (wasDragged)
                return;

            selected = !selected;
            SelectEvent.Invoke(this, selected);

            if (selected)
                transform.localPosition += (uiCardVisual.transform.up * selectionOffset);
            else
                transform.localPosition = Vector3.zero;
        }

        public void Deselect()
        {
            if (selected)
            {
                selected = false;
                if (selected)
                    transform.localPosition += (uiCardVisual.transform.up * 50);
                else
                    transform.localPosition = Vector3.zero;
            }
        }


        public int SiblingAmount()
        {
            return transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
        }

        public int ParentIndex()
        {
            return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
        }

        public float NormalizedPosition()
        {
            return transform.parent.CompareTag("Slot") ? UIExtensionMethods.Remap((float)ParentIndex(), 0, (float)(transform.parent.parent.childCount - 1), 0, 1) : 0;
        }

        private void OnDestroy()
        {
            if(uiCardVisual != null)
                Destroy(uiCardVisual.gameObject);
        }
    }
}

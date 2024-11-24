using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GreonAssets.UI.Extensions;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UICardVisual : MonoBehaviour
    {
        private bool initalize = false;

        [Header("UICardMovement")]
        public UICardMovement parentCard;
        private Transform cardTransform;
        private Vector3 rotationDelta;
        private int savedIndex;
        Vector3 movementDelta;

        [Header("References")]
        public Transform visualShadow;
        [SerializeField] private float shadowOffset = 20;
        private Vector2 shadowDistance;
        private Canvas shadowCanvas;
        [SerializeField] private Transform shakeParent;
        [SerializeField] private Transform tiltParent;
        [SerializeField] private Image cardImage;

        [Header("Follow Parameters")]
        [SerializeField] private float followSpeed = 30;

        [Header("Rotation Parameters")]
        [SerializeField] private float rotationAmount = 20;
        [SerializeField] private float rotationSpeed = 20;
        [SerializeField] private float autoTiltAmount = 30;
        [SerializeField] private float manualTiltAmount = 20;
        [SerializeField] private float tiltSpeed = 20;

        [Header("Scale Parameters")]
        [SerializeField] private bool scaleAnimations = true;
        [SerializeField] private float scaleOnHover = 1.15f;
        [SerializeField] private float scaleOnSelect = 1.25f;
        [SerializeField] private float scaleTransition = .15f;
        [SerializeField] private Ease scaleEase = Ease.OutBack;

        [Header("Select Parameters")]
        [SerializeField] private float selectPunchAmount = 20;

        [Header("Hober Parameters")]
        [SerializeField] private float hoverPunchAngle = 5;
        [SerializeField] private float hoverTransition = .15f;

        [Header("Swap Parameters")]
        [SerializeField] private bool swapAnimations = true;
        [SerializeField] private float swapRotationAngle = 30;
        [SerializeField] private float swapTransition = .15f;
        [SerializeField] private int swapVibrato = 5;

        [Header("Curve")]
        [SerializeField] private UICurveParameters uiCurve;

        [Header("Damage")]
        [SerializeField] private float damageInTime = 0.1f;
        [SerializeField] private float damageOutTime = 0.5f;
        [SerializeField] private float dieAnimation = 0.2f;
        [SerializeField] private float damagePunchAngle = 5;
        [SerializeField] private float dieScale = 0.5f;
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private Color healthColor = Color.green;
        [SerializeField] private Color spellDamageColor = Color.red;

        [Header("Attack")]
        [SerializeField] private float attackTime = 0.3f;
        [SerializeField] private float attackPunchSize = 0.1f;
        [SerializeField] private float attackDistance = 20f;

        private float curveYOffset;
        private float curveRotationOffset;
        private Color startColor;
        private CanvasGroup canvasGroup;
        private Canvas canvas;

        private void Start()
        {
            shadowDistance = visualShadow.localPosition;
            startColor = cardImage.color;
        }

        public void Initialize(UICardMovement target, int index = 0)
        {
            transform.position = target.transform.position;

            //Declarations
            parentCard = target;
            cardTransform = target.transform;
            shadowCanvas = visualShadow.GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            canvas = GetComponent<Canvas>();

            //Event Listening
            parentCard.PointerEnterEvent.AddListener(PointerEnter);
            parentCard.PointerExitEvent.AddListener(PointerExit);
            parentCard.BeginDragEvent.AddListener(BeginDrag);
            parentCard.EndDragEvent.AddListener(EndDrag);
            parentCard.PointerDownEvent.AddListener(PointerDown);
            parentCard.PointerUpEvent.AddListener(PointerUp);
            parentCard.SlotEnterEvent.AddListener(SlotEnter);
            parentCard.SlotExitEvent.AddListener(SlotExit);
            parentCard.Model.OnHealthChange += OnHealthChange;
            parentCard.Model.OnAttack += OnAttack;

            //Initialization
            initalize = true;
        }

        private void OnDestroy()
        {
            parentCard.PointerEnterEvent.RemoveListener(PointerEnter);
            parentCard.PointerExitEvent.RemoveListener(PointerExit);
            parentCard.BeginDragEvent.RemoveListener(BeginDrag);
            parentCard.EndDragEvent.RemoveListener(EndDrag);
            parentCard.PointerDownEvent.RemoveListener(PointerDown);
            parentCard.PointerUpEvent.RemoveListener(PointerUp);
            parentCard.SlotEnterEvent.RemoveListener(SlotEnter);
            parentCard.SlotExitEvent.RemoveListener(SlotExit);
            parentCard.Model.OnHealthChange -= OnHealthChange;
            parentCard.Model.OnAttack -= OnAttack;
        }

        private void SlotEnter(UICardMovement card, UICardSlot slot)
        {
            if (slot == null) return;
            if (parentCard.Model.Type == CardType.Spell)
            {
                var slotsModels = BattleController.Model.GetSlotsForSpell(slot.CardPosition, parentCard.Model.Config.SpellPlacing);
                var slots = UIBattle.Instance.Slots.Where(x => slotsModels.Contains(x.Key)).Select(x => x.Value.GetComponent<RectTransform>());
                UIDynamicSelector.Instance.SetSelection(slots);
                return;
            }

            slot.SetHighlight(true);
        }
        private void SlotExit(UICardMovement card, UICardSlot slot)
        {
            if (slot == null) return;

            if (parentCard.Model.Type == CardType.Spell)
            {
                UIDynamicSelector.Instance.SetSelection(new List<RectTransform>());
                return;
            }

            slot.SetHighlight(false);
        }

        public void UpdateIndex(int length)
        {
            transform.SetSiblingIndex(parentCard.transform.parent.GetSiblingIndex());
        }

        void Update()
        {
            if (!initalize || parentCard == null) return;

            HandPositioning();
            SmoothFollow();
            FollowRotation();
            CardTilt();
        }

        private void HandPositioning()
        {
            curveYOffset = (uiCurve.positioning.Evaluate(parentCard.NormalizedPosition()) * uiCurve.positioningInfluence) * parentCard.SiblingAmount();
            curveYOffset = parentCard.SiblingAmount() < 5 ? 0 : curveYOffset;
            curveRotationOffset = uiCurve.rotation.Evaluate(parentCard.NormalizedPosition());
        }

        private void SmoothFollow()
        {
            Vector3 verticalOffset = (Vector3.up * (parentCard.isDragging ? 0 : curveYOffset));
            transform.position = Vector3.Lerp(transform.position, cardTransform.position + verticalOffset, followSpeed * Time.deltaTime);
        }

        private void FollowRotation()
        {
            Vector3 movement = (transform.position - cardTransform.position);
            movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
            Vector3 movementRotation = (parentCard.isDragging ? movementDelta : movement) * rotationAmount;
            rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -60, 60));
        }

        private void CardTilt()
        {
            savedIndex = parentCard.isDragging ? savedIndex : parentCard.ParentIndex();
            float sine = Mathf.Sin(Time.time + savedIndex) * (parentCard.isHovering ? .2f : 1);
            float cosine = Mathf.Cos(Time.time + savedIndex) * (parentCard.isHovering ? .2f : 1);

            Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float tiltX = parentCard.isHovering ? ((offset.y * -1) * manualTiltAmount) : 0;
            float tiltY = parentCard.isHovering ? ((offset.x) * manualTiltAmount) : 0;
            float tiltZ = parentCard.isDragging ? tiltParent.eulerAngles.z : (curveRotationOffset * (uiCurve.rotationInfluence * parentCard.SiblingAmount()));

            float lerpX = Mathf.LerpAngle(tiltParent.eulerAngles.x, tiltX + (sine * autoTiltAmount), tiltSpeed * Time.deltaTime);
            float lerpY = Mathf.LerpAngle(tiltParent.eulerAngles.y, tiltY + (cosine * autoTiltAmount), tiltSpeed * Time.deltaTime);
            float lerpZ = Mathf.LerpAngle(tiltParent.eulerAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);

            tiltParent.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
        }

        private void Select(UICardMovement card, bool state)
        {
            DOTween.Kill(2, true);
            float dir = state ? 1 : 0;
            shakeParent.DOPunchPosition(shakeParent.up * selectPunchAmount * dir, scaleTransition, 10, 1);
            shakeParent.DOPunchRotation(Vector3.forward * (hoverPunchAngle / 2), hoverTransition, 20, 1).SetId(2);

            if (scaleAnimations)
                transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
        }

        public void Swap(float dir = 1)
        {
            if (!swapAnimations)
                return;

            DOTween.Kill(2, true);
            shakeParent.DOPunchRotation((Vector3.forward * swapRotationAngle) * dir, swapTransition, swapVibrato, 1).SetId(3);
        }

        private void BeginDrag(UICardMovement card)
        {
            if (scaleAnimations)
                transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);
        }

        private void EndDrag(UICardMovement card)
        {
            transform.DOScale(1, scaleTransition).SetEase(scaleEase);
        }

        private void PointerEnter(UICardMovement card)
        {
            if (scaleAnimations)
                transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);

            DOTween.Kill(2, true);
            shakeParent.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
        }

        private void PointerExit(UICardMovement card)
        {
            if (!parentCard.wasDragged)
                transform.DOScale(1, scaleTransition).SetEase(scaleEase);
        }

        private void PointerUp(UICardMovement card, bool longPress)
        {
            if (scaleAnimations)
                transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);

            visualShadow.localPosition = shadowDistance;
            shadowCanvas.overrideSorting = true;
            canvas.overrideSorting = false;
        }

        private void PointerDown(UICardMovement card)
        {
            if (scaleAnimations)
                transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);

            visualShadow.localPosition += (-Vector3.up * shadowOffset);
            shadowCanvas.overrideSorting = false;
            canvas.overrideSorting = true;
        }

        private async void OnHealthChange(int health)
        {
            shakeParent.DOPunchRotation(Vector3.forward * damagePunchAngle, damageInTime, 20, 1);
            shakeParent.DOPunchScale(Vector3.one * 0.07f * Math.Sign(health), damageInTime + damageOutTime, 1, 1);

            var changeColor = health > 0 ? healthColor : damageColor;
            await cardImage
                .DOColor(parentCard.Model.Type == CardType.Card ? changeColor : spellDamageColor, damageInTime)
                .SetEase(Ease.OutQuad)
                .AsyncWaitForCompletion();

            await cardImage
                .DOColor(startColor, damageOutTime)
                .SetEase(Ease.Linear)
                .AsyncWaitForCompletion();
        }

        public async void DeathAnimationAndDestroy()
        {
            canvasGroup
                .DOFade(0f, dieAnimation)
                .SetEase(Ease.OutBack);
            
            await transform
                .DOScale(dieScale, dieAnimation)
                .SetEase(Ease.OutBack)
                .AsyncWaitForCompletion();

            cardImage.DOKill();
            shakeParent.DOKill();

            Destroy(gameObject);
        }
        private void OnAttack(CardPosition position)
        {
            var direction = position.owner == CardOwner.player ? Vector3.down : Vector3.up;

            shakeParent.DOPunchScale(Vector3.one * attackPunchSize, attackTime, 1, 1).SetEase(Ease.OutBack);
            shakeParent.DOPunchPosition(direction * attackDistance, attackTime, 1, 1).SetEase(Ease.OutQuad);
        }
    }
}

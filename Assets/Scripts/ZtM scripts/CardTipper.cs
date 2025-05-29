using Project.Gameplay.Battle.Model.Cards;
using UnityEngine;
using Project.UI.Battle;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardTipper : Tipper
{
    [SerializeField] private UICardMovement CardMovement;
    [SerializeField] private Graphic[] Graphics; 

    public CardModel _Card => CardMovement.Model;

    private void Start()
    {
        _Card.OnDeath += Death;
    }

    private void Death()
    {
        foreach(Graphic graphic in Graphics)
        {
            Destroy(graphic);
        }

        _Card.OnDeath -= Death;
        Destroy(this);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(_Card.Type == Project.Gameplay.Common.Datas.CardType.Spell)
        {
            return;
        }

        base.OnPointerEnter(eventData);
    }
}

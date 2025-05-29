using UnityEngine;
using UnityEngine.EventSystems;
using Project.Gameplay.Battle;

public class Tipper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField][Multiline] protected string Info;
    
    public event SimpleVoid OnInfoChange = null;

    public string _Info
    {
        get
        {
            return Info;
        }
        set
        {
            Info = value;

            if (OnInfoChange != null)
            {
                OnInfoChange.Invoke();
            }
        }
    }

    protected virtual void OnDisable()
    {
        TipManager.ShowTip(this, true);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        TipManager.ShowTip(this, false);
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        TipManager.ShowTip(this, true);
    }
}

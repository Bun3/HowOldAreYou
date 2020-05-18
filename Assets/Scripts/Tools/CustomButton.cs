using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class OnClickEvent : UnityEvent { }

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Transform m_ts;

    [SerializeField]
    [Range(0.8f, 1f)]
    private float m_ScopeVal = 0.9f;

    [Space(40)]

    [SerializeField]
    protected OnClickEvent clickEvent;

    private void Awake()
    {
        m_ts = transform;

        if(clickEvent == null)
            clickEvent = new OnClickEvent();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_ts.localScale = Vector3.one * m_ScopeVal;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_ts.localScale = Vector3.one;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickEvent?.Invoke();
    }
}

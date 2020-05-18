using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnMouseDownEvent : UnityEvent { }

public class RabbitMouseBox : MonoBehaviour
{
    private static RabbitCtrl rabbitCtrl = null;

    [SerializeField] Vector2Int Vector = Vector2Int.zero;
    [SerializeField] OnMouseDownEvent downEvent = null;

    private void Awake()
    {
        if (rabbitCtrl == null) rabbitCtrl = transform.parent.GetComponent<RabbitCtrl>();
        if (downEvent == null) downEvent = new OnMouseDownEvent();
    }

    private void OnMouseDown()
    {
        Vector2 mPos = Input.mousePosition;
        mPos = Camera.main.ScreenToWorldPoint(mPos);
        Vector2 dis = ((Vector2)rabbitCtrl.transform.position - mPos).normalized;
        rabbitCtrl.dir = Mathf.Abs(dis.x) > Mathf.Abs(dis.y) ? Vector2Int.left * System.Convert.ToInt32(dis.x) : Vector2Int.up * System.Convert.ToInt32(dis.y);
        downEvent.Invoke();
    }

}

using UnityEngine;

public class ReadingGlass : MonoBehaviour
{
    private Vector2 m_OriginPos = new Vector2(-0.1f, -0.2f);

    private void Start()
    {
        transform.position = m_OriginPos;
    }

#if UNITY_ANDROID || UNITY_IOS

    private void Update()
    {
        if(Input.touchCount.Equals(1))
        {
            Touch currentTouch = Input.GetTouch(0);
            Vector3 touchPos;
            switch (currentTouch.phase)
            {
                case TouchPhase.Began:
                    touchPos = Camera.main.ScreenToWorldPoint(currentTouch.position);
                    touchPos = GetWallCheckedPos(touchPos, FirstGame.Instance.m_Left, FirstGame.Instance.m_Top, FirstGame.Instance.m_Right, FirstGame.Instance.m_Bottom);
                    transform.position = new Vector3(touchPos.x, touchPos.y, 0);
                    break;
                case TouchPhase.Moved:
                    touchPos = Camera.main.ScreenToWorldPoint(currentTouch.position);
                    touchPos = GetWallCheckedPos(touchPos, FirstGame.Instance.m_Left, FirstGame.Instance.m_Top, FirstGame.Instance.m_Right, FirstGame.Instance.m_Bottom);
                    transform.position = new Vector3(touchPos.x, touchPos.y, 0);
                    break;
                default:
                    break;
            }
        }
    }

#endif

#if UNITY_EDITOR

    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Clamp(mousePos.x, FirstGame.Instance.m_ScreenLeft, FirstGame.Instance.m_ScreenRight);
        mousePos.y = Mathf.Clamp(mousePos.y, FirstGame.Instance.m_ScreenBottom, FirstGame.Instance.m_ScreenTop);

        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

#endif

}

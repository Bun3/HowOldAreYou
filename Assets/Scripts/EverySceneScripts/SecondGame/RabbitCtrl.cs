using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCtrl : MonoBehaviour
{
    private Transform m_ts = null;
    private Animator animator = null;

    [HideInInspector] public Sprite sprite = null;
    public Vector2Int dir = Vector2Int.zero;
    public Vector2Int look = Vector2Int.down;
    private void Awake()
    {
        m_ts = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        sprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void TurnLeft() => m_ts.transform.localEulerAngles += Vector3.forward * 90f;
    public void TurnRight() => m_ts.transform.localEulerAngles += Vector3.back * 90f;
    public void TurnBack() => m_ts.transform.localEulerAngles += Vector3.forward * 180f;

    public void MoveFoward()
    {
        Move();
        SecondGame.Instance.MoveRabbit(dir);
    }

    public void Move() => animator.SetTrigger("Move");

}

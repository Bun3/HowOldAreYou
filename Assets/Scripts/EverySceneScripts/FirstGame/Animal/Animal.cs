using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    protected Rigidbody2D m_RigidBody2D;
    protected SpriteRenderer m_SpriteRenderer;
    protected MovementAnimStateMachine m_Movement;
    protected Animator m_Animator;

    protected Vector2 normalizedVec;

    [SerializeField]
    protected float moveSpeed = 1f;

    protected virtual void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        m_Movement = m_Animator.GetBehaviour<MovementAnimStateMachine>();
        m_Movement.OnAnimationEnd += OnAnimationEnd;

        StartCoroutine(IMovement());
    }

    protected virtual void OnAnimationEnd() { }

    private IEnumerator IMovement()
    {
        int movementSec = Random.Range(3, 7);
        normalizedVec = Random.insideUnitCircle;

        normalizedVec.Normalize();

        float timer = 0f;
        while (timer <= movementSec)
        {
            m_SpriteRenderer.flipX = normalizedVec.x > 0; 
            m_RigidBody2D.velocity = normalizedVec * moveSpeed;

            timer += Time.smoothDeltaTime;

            yield return null;
        }

        yield return StartCoroutine(IMovement());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("BoundBox"))
        {
            Vector2 normalVec = collision.contacts[0].normal;
            Vector2 reflectVec = Vector2.Reflect(normalizedVec.normalized, normalVec);
            normalizedVec = reflectVec;
        }
    }

}

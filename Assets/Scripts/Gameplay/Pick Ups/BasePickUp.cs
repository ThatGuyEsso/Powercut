using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public abstract class BasePickUp : MonoBehaviour, IBreakVFX
{

    protected Rigidbody2D rb;
    virtual protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InitStateManager.instance.OnStateChange += EvaluateNewState;
    }
 
    virtual protected void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {
        

            case InitStates.PlayerDead:
                ObjectPoolManager.Recycle(gameObject);
                break;
            case InitStates.LoadTitleScreen:
                ObjectPoolManager.Recycle(gameObject);
                break;
            case InitStates.LoadMainMenu:
                ObjectPoolManager.Recycle(gameObject);
                break;
            case InitStates.Credits:
                ObjectPoolManager.Recycle(gameObject);
                break;
            case InitStates.ExitLevel:
                ObjectPoolManager.Recycle(gameObject);

                break;

        }
    }
    virtual public void AddBreakForce(Vector2 dir, float force)
    {
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

    

    public void Break(Vector2 dir, float force)
    {
        //Can't break;
    }

    private void OnEnable()
    {
        InitStateManager.instance.OnStateChange -= EvaluateNewState;
    }
    private void OnDisable()
    {
        InitStateManager.instance.OnStateChange -= EvaluateNewState;
    }


}

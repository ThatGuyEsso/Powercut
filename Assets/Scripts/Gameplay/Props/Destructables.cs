using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour, IBreakVFX, IHurtable
{
    private int nBrokenParts;
    [SerializeField] private List<GameObject> brokenParts = new List<GameObject>();

    [SerializeField] private float spread;
    [SerializeField] private float spreadAngle;

    private float health;
    [SerializeField] private float maxhHalth = 20.0f;

    [SerializeField] private List<SpriteRenderer> gfxs;
    [SerializeField] private List<Collider2D> colliders;
    [SerializeField] private List<GameObject> pieces;
    private float knockBack;
    private Vector3 direction;
    bool isBroken = false;
    private void Awake()
    {
        BindToInitManager();
        GetAllColliders();
        GetAllGFXs();
        health = maxhHalth;
    }
    public void Break(Vector2 dir, float force)
    {
        Vector3[] partDir = EssoUtility.GetVectorsInArc(dir, brokenParts.Count, spreadAngle, spread);
        for (int i = 0; i < brokenParts.Count; i++)
        {

            Vector3 randPosition = transform.position + Random.insideUnitSphere * 0.5f;
            GameObject part = ObjectPoolManager.Spawn(brokenParts[i], randPosition, transform.rotation);
            pieces.Add(part);
            part.GetComponent<IBreakVFX>().AddBreakForce(partDir[i], force);

            Debug.DrawRay(transform.position, partDir[i], Color.yellow, 10f);

        }

        DisableObject();
    }

    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        health -= damage;
        if (health <= 0 && !isBroken)
        {
            isBroken = true;
            Break(knockBackDir, knockBack);
        }
    }

    public void BindToInitManager()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
    }

    private void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {

            case InitStates.RespawnPlayer:
                ResetDestructable();

                break;



        }
    }


    public void AddBreakForce(Vector2 dir, float force)
    {
        throw new System.NotImplementedException();
    }

    public void GetAllGFXs()
    {
        SpriteRenderer[] parentsGFX = gameObject.GetComponents<SpriteRenderer>();

        foreach (SpriteRenderer renderer in parentsGFX)
        {
            gfxs.Add(renderer);
        }
        SpriteRenderer[] childGFX = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in childGFX)
        {
            gfxs.Add(renderer);
        }
    }
    public void GetAllColliders()
    {
        Collider2D[] parentsColliders = gameObject.GetComponents<Collider2D>();

        foreach (Collider2D collider in parentsColliders)
        {
            colliders.Add(collider);
        }
        Collider2D[] childColliders = gameObject.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in childColliders)
        {
            colliders.Add(collider);
        }
    }

    private void DisableObject()
    {
        if (colliders.Count > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }

            if (gfxs.Count > 0)
            {
                foreach (SpriteRenderer sprite in gfxs)
                {
                    sprite.enabled = false;
                }


            }

        }
    }

    private void EnableObject()
    {
        if (colliders.Count > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = true;
            }

            if (gfxs.Count > 0)
            {
                foreach (SpriteRenderer sprite in gfxs)
                {
                    sprite.enabled = true;
                }


            }

        }
    }

    void RecyclePieces()
    {
        if (pieces.Count <= 0) return;
        foreach (GameObject piece in pieces)
        {
            ObjectPoolManager.Recycle(piece);
        }

        pieces.Clear();
    }

    public void ResetDestructable()
    {
        RecyclePieces();
        EnableObject();
        health = maxhHalth;
        isBroken = false;
    }

    private void OnDestroy()
    {
        if (isBroken)
        {
            RecyclePieces();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Destructables : MonoBehaviour, IBreakVFX, IHurtable
{
    private int nBrokenParts;
    [SerializeField] private List<GameObject> brokenParts = new List<GameObject>();

    [SerializeField] private float spread;
    [SerializeField] private float spreadAngle;
    [SerializeField] private LootTable table;
    private float health;
    [SerializeField] private float maxhHalth = 20.0f;
    protected AudioSource aSource;
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
        aSource = GetComponent<AudioSource>();
        Sound sound = AudioManager.instance.GetSound("ObjectSmash");

        aSource.clip = sound.clip;
        aSource.outputAudioMixerGroup = sound.mixerGroup;
        aSource.volume = sound.volume;
        aSource.pitch = sound.pitch;
        aSource.loop = sound.loop;
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

           

        }
        if (table)
        {
            GameObject loot = table.ReturnLoot();
            if (loot)
            {
                Vector3 randPosition = transform.position + Random.insideUnitSphere * 0.5f;
                ObjectPoolManager.Spawn(loot, randPosition, transform.rotation).GetComponent<IBreakVFX>().AddBreakForce(dir, force);
            }
        }
         

        aSource.Play();
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
             
                if(collider == true)
                    collider.enabled = true;
            }

            if (gfxs.Count > 0)
            {
                foreach (SpriteRenderer sprite in gfxs)
                {
                    if (sprite == true)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{


    private PlayerBehaviour playerRef;
    private Animator lowerBodyAnim;
    public Sprite shotgunSprite,pistolSprite,unarmedSprite;
    public Vector3 shotgunSpritePos, pistolSpritePos, unarmedSpritePos;
    public Vector2 unarmedCollisionOffset, pistolCollisionOffset, shotgunCollisionOffset;
    public Vector2  pistolWeaponColliderOffset, shotgunWeaponColliderOffset;
    private SpriteRenderer upperBodySprite;
    public CapsuleCollider2D playerCollider;
    public CapsuleCollider2D weaponCollider;
    private void Awake()
    {
        playerRef = gameObject.GetComponent<PlayerBehaviour>();
        lowerBodyAnim = transform.Find("LowerBodyGFX").GetComponent<Animator>();
        upperBodySprite = transform.Find("TopBodyGFX").GetComponent<SpriteRenderer>();
   
    }


    public void UpdatePlayergun()
    {
        if (GameStateManager.instance.GetCurrentGameState() != GameStates.MainPowerOn)
        {
            weaponCollider.enabled = true;
            switch (WeaponManager.instance.GetActiveGun()){
                case GunTypes.Pistol:
                    upperBodySprite.sprite = pistolSprite;
                    playerCollider.offset = pistolCollisionOffset;
                    upperBodySprite.transform.localPosition = pistolSpritePos;
                    weaponCollider.offset = pistolWeaponColliderOffset;
                    break;
                case GunTypes.Shotgun:
                    upperBodySprite.sprite = shotgunSprite;
                    upperBodySprite.transform.localPosition = shotgunSpritePos;
                    playerCollider.offset = shotgunCollisionOffset;
                    weaponCollider.offset = shotgunWeaponColliderOffset;
                    break;
            }
        }
        else
        {
            playerCollider.offset = unarmedCollisionOffset;
            weaponCollider.enabled = false;
            upperBodySprite.sprite = unarmedSprite;
            upperBodySprite.transform.localPosition = unarmedSpritePos;
        }
    }

    public void PlayWalkAnim()
    {
        lowerBodyAnim.Play("Walk");
    }

    public void StopWalkAnim()
    {
        lowerBodyAnim.Play("idle");
    }
}

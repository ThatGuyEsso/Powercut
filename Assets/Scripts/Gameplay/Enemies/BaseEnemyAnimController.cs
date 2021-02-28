using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAnimController : MonoBehaviour
{
    private Animator anim;

 
    public void Init()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
      
    }


    public void PlayAnim(string animationName)
    {
        if (anim.gameObject.activeSelf)
        {
            anim.Play(animationName);


        }
    }

  

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObjectAfterAnimation : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        Destroy(animator.gameObject, stateInfo.length);
        Entity playerEntity;
        playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        playerEntity.giveCoin(1);
    }

}
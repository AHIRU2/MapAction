using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition_Walk_through : PlayerConditionBase
{
    private SpriteRenderer spriteRenderer;

    private float origineAlpha = 1.0f;

    protected override void OnEnterCondition()
    {
        if(mapMoveController.TryGetComponent(out spriteRenderer))
        {
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, conditionValue);
        }

        base.OnEnterCondition();

    }

    protected override void OnExitCondition()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, origineAlpha);


        base.OnExitCondition();
    }


}

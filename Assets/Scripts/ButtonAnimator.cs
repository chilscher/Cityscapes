//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimator : MonoBehaviour {

    private float scale;
    private readonly float pressScale = 0.85f;
    private readonly float popScale = 1.1f;

    public void Start(){
        scale = transform.localScale.x;
    }
    
    public void AnimatePress(){
        transform.DOKill();
        transform.DOScale(pressScale * scale * Vector3.one, 0.2f).OnComplete(AnimatePop);
    }

    private void AnimatePop(){
        transform.DOScale(popScale * scale * Vector3.one, 0.2f).OnComplete(AnimateReturn);
    }

    private void AnimateReturn() {
        transform.DOScale(scale * Vector3.one, 0.1f);
    }
}

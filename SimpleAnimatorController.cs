using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPS.Common {
public class SimpleAnimatorController : MonoBehaviour
{
    [Header("Simple Animator Controller Component Refs")]
    [SerializeField]
    private Animator animator;


    [Header("GameObject Refs")]
    [Tooltip("Used when destroying the animation on end. \nIf the object should be destroyed at the end of its lifecycle, if this value is non-null, destroy it instead.")]
    [SerializeField]
    private GameObject parentGameObject;

    public GameObject ParentGameObject { set => this.parentGameObject = value; }

    [Header("Simple Animator Controller Configurations")]
    [Tooltip("Determines whether this game object should be destroyed at the end of the animation")]
    [SerializeField]
    private bool destroyOnAnimationEnd = false;

    private Action OnAnimationProgressEvent = delegate {};

    private Action OnDestroyEvents = delegate {};

    private Action OnAnimationEndEvent = delegate {};

    void Awake() {
        if (this.parentGameObject == null)
            this.parentGameObject = this.gameObject;
        // this.ClearCallBacks();
    }

    void DuringAnimationCallBack() {
        Debug.Log("INVOKING ANIMATION CALLBACKS!");
        this.OnAnimationProgressEvent?.Invoke();
    }

    void AnimationEndCallBack() {
        Debug.Log("INVOKING ANIMATION CALLBACKS!");
        this.OnAnimationEndEvent?.Invoke();
    }

    void DestroyAnimation() {
        this.OnDestroyEvents?.Invoke();
        this.ClearCallBacks();
        // this.SetAnimationEnded();
        if (!this.destroyOnAnimationEnd) {
            return;
        }
        try {
            Destroy(this.parentGameObject);
        } catch (Exception e) {
            Debug.LogWarning(e.Message);
            Destroy(this.gameObject);
        }
    }

    void ClearCallBacks() {
        this.OnAnimationProgressEvent = delegate {};
        this.OnDestroyEvents = delegate {};
    }

    public void AddAnimatorProgressEvent(Action callBack) {
        
        this.OnAnimationProgressEvent += callBack;
    }

    public void AddAnimatorEndEvent(Action callBack) {
        this.OnAnimationEndEvent += callBack;
    }

    public void AddDestroyEvent(Action callBack)
    {
        this.OnDestroyEvents += callBack;
    }

}
}

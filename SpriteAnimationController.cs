using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPS.Common;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public Animator Animator { get => _animator; }

    [SerializeField] private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer { get => _spriteRenderer; }

    [SerializeField]
    private Material[] spriteMaterials;

    [SerializeField]
    [Tooltip("The game object for the main camera")]
    private GameObject mainCamera;

    // [SerializeField]
    // [Tooltip("Camera override")]
    // private GameObject overrideCamera;

    [SerializeField]
    [Tooltip("The parent game object for the player animation controller")]
    private GameObject parentGO;

    public GameObject ParentGO { set => this.parentGO = value; get => this.parentGO;}

    [SerializeField]
    [Tooltip("Determines whether to flip the sprite when changing directions")]
    private bool flipSprite = true;

    [SerializeField]
    [Tooltip("Determines the intensity of the glow for objects that can glow")]
    private float glowIntensity;



    [SerializeField]
    [Tooltip("The animation parameters that exist for the animator attached to this game object")]
    private GenericDictionary<string, bool> animationParameters;

    
    [SerializeField]
    [Tooltip("The glow game object to be used for glow effects")]
    private GameObject glow;


    [Tooltip("The casting circle to be used for casted moves")]
    [SerializeField]
    private GameObject castingCircle;

    [Tooltip("The game object for the poise break")]
    [SerializeField]
    private GameObject poiseBreak;

    [Tooltip("The game object for thinking")]
    [SerializeField]
    private GameObject thinking;

    [Tooltip("The damp timee between different animations")]
    private readonly float dampTime = 0.0f;

    #nullable enable
    [SerializeField] private System.Action? attackCallBack;

    // [SerializeField]
    // private float dampTime = 0.2f;



    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        for (int i = 0; i < this._animator.parameters.Length; i++) {
            this.animationParameters.Add(this._animator.parameters[i].name, true);
        }
        if (this._spriteRenderer != null) {
            this.spriteMaterials = this._spriteRenderer.materials;
        }
    }

    protected virtual void Update() {
        // this.overrideCamera = GameObject.FindGameObjectWithTag("Camera");
    }

    protected virtual void LateUpdate() {
        if (!this.IsReady()) {
            this.GetMainCamera();
            return;
        }

        this.GetAnimDirections();
    }



    public virtual bool IsReady() {
        return this.parentGO != null && (this.mainCamera != null) && this._animator != null;
    }

    private void GetMainCamera() {
        if(this.mainCamera != null) {
           return;
        }
        this.mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void GetAnimDirections() {
        GameObject camera = this.mainCamera;

        Vector3 cameraVector = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z);
        Vector3 parentGOVector = new Vector3(this.parentGO.transform.forward.x, 0, this.parentGO.transform.forward.z);

        float angle = Vector3.SignedAngle(cameraVector, parentGOVector, this.parentGO.transform.up);
        float animX = 0;
        float animY = 0;

        if(angle <= 30f && angle >= -30f) {
            animY = -1f;
            animX = 0;
        } else if (angle >= -150f && angle <= -30f) {
            animY = 0;
            animX = -1f;
        } else if (angle <= 150f && angle >= 30f) {
            animY = 0;
            animX = 1f;
        } else if (angle > 150f) {
            animY = 1f;
            animX = 0;
        }

        this.SetAnimFloatValues(animX, animY);
    }

    private void SetAnimFloatValues(float animX, float animY) {
        if (!this.animationParameters.ContainsKey("AnimX") || !this.animationParameters.ContainsKey("AnimY")) {
            return;
        }
        this._animator.SetFloat("AnimX", animX, this.dampTime, Time.deltaTime);
        this._animator.SetFloat("AnimY", animY, this.dampTime, Time.deltaTime);
    }

    //Depends on the creation of the sprite
    public void DetermineSpriteFacing(bool isFacingLeft)
    {
        if (!flipSprite) {
            return;
        }
        _spriteRenderer.flipX = !isFacingLeft;
    }

    public virtual void EndMovement() {
        if (this.animationParameters.ContainsKey("isRunning") && this._animator.GetBool("isRunning")) {
            this._animator.SetBool("isRunning", false);
        }
        if (this.animationParameters.ContainsKey("isWalking") && this._animator.GetBool("isWalking")) {
            this._animator.SetBool("isWalking", false);
        }
    }

    public virtual void SetMovement(bool isRunning = false, bool isWalking = false) {
        if (!this.animationParameters.ContainsKey("isWalking") || !this.animationParameters.ContainsKey("isWalking")) {
            return;
        }
        this._animator.SetBool("isRunning", isRunning);
        this._animator.SetBool("isWalking", isWalking);
    }

    public void TriggerSpecialAnimation(string specialAnimation) {
        if (this._animator == null) {
            return;
        }
        this._animator.SetTrigger(specialAnimation);
        return;
    }

    public void TriggerAnimation(AnimationTrigger animationTrigger) {
        switch(animationTrigger) {
            case AnimationTrigger.ATTACK:
                this._animator.SetTrigger("attack_1");
                break;
            case AnimationTrigger.CASTING:
                this._animator.SetTrigger("casting");
                break;
            case AnimationTrigger.THROW:
                this._animator.SetTrigger("throw");
                break;
            case AnimationTrigger.DAMAGED:
                this._animator.SetTrigger("damage");
                break;

        }
    }

    public void SetAnimation(AnimationStates animationState, bool disableCurrentStates = true) {

        if (disableCurrentStates) {
            this.DisableAllSpecialAnimations(); 
        }
        

        if (animationState == AnimationStates.Default) {
            return;
        }

        switch(animationState) {
            case AnimationStates.Defend:
                this._animator.SetBool("isDefending", true);
                break;
            case AnimationStates.PoiseBreak:
                this._animator.SetBool("isPoiseBroken", true);
                this.TogglePoiseBreak(true);
                break;
            case AnimationStates.Ko:
                this._animator.SetBool("isKOed", true);
                break;
            case AnimationStates.Fall:
                this._animator.SetBool("isFalling", true);
                break;
            case AnimationStates.Jump:
                this._animator.SetBool("isJumping", true);
                break;
        }
    }

    public void ToggleEffect(AnimationEffect animationEffect, bool toggle) {
        switch(animationEffect) {
            case AnimationEffect.Thinking:
                this.ToggleThinking(toggle);
                break;
            case AnimationEffect.Casting:
                this.ToggleCastingCircle(toggle);
                break;
        }
    }

    public void SetAnimation(string animationState) {
        this.DisableAllSpecialAnimations();
    }

    public void DisableAllSpecialAnimations() {
        this._animator.SetBool("isKOed", false);
        this._animator.SetBool("isPoiseBroken", false);
        this.TogglePoiseBreak(false);
        this._animator.SetBool("isDefending", false);
        this._animator.SetBool("isFalling", false);
        this._animator.SetBool("isJumping", false);
        this.ToggleCastingCircle(false);
        this.ToggleThinking(false);
    }



    public void EndAttack() {
        if (this.attackCallBack != null) {
            this.attackCallBack();
            this.attackCallBack = null;
        }
        _animator.SetBool("isAttacking", false);
    }


    public void FireAttack(System.Action? attackCallBack = null) {
        if (attackCallBack != null) {
            this.attackCallBack = attackCallBack;
        }
        _animator.SetBool("isAttacking", true);
    }

    public bool IsAttacking() {
        if (!this.animationParameters.ContainsKey("isAttacking")) {
            return false;
        }
        return this._animator.GetBool("isAttacking");
    }

    public void ToggleBackLight(bool activateBackLight) {
        if (this.glow == null || (this.glow.activeSelf == activateBackLight)) {
            return;
        }

        this.glow.SetActive(activateBackLight);
    }


    private void ToggleCastingCircle(bool toggleCastingCircle) {
        if (this.castingCircle == null || (this.castingCircle.activeSelf == toggleCastingCircle)) {
            return;
        }

        this.castingCircle.SetActive(toggleCastingCircle); 
    }

    private void TogglePoiseBreak(bool poiseBreak) {
        if (this.poiseBreak == null || (this.poiseBreak.activeSelf == poiseBreak)) {
            return;
        }

        this.poiseBreak.SetActive(poiseBreak);
    }

    private void ToggleThinking ( bool toggleThinking) {
        if (this.thinking == null || (this.thinking.activeSelf == toggleThinking)) {
            return;
        }

        this.thinking.SetActive(toggleThinking);
    }
    #nullable disable
}

public enum AnimationTrigger {
    NONE,
    ATTACK,   //Basic Melee Attack
    CASTING,   
    THROW,
    DAMAGED,
}


public enum AnimationStates {
    Default,
    Ko,
    PoiseBreak,
    Defend,
    Fall,
    Jump,
}

public enum AnimationEffect {
    Casting,
    Thinking
}
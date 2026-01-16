using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPS.Common {
public class CameraTrackerController : MonoBehaviour
{
    #nullable enable
    [SerializeField]
    private Transform? _target;

    public Transform? Target { get => this._target; }

    [Tooltip("Affects camera's tracking speed")]
    [Range(10f, 30f)]
    [SerializeField]
    private float cameraTrackingSpeed = 15f;

    [SerializeField]
    private bool _isSettingTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        this.Startup();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate() {
        this.MoveCamera();
    }

    void Startup() {
        if (this._target != null) {
            this.transform.position = this._target.transform.position;
        }
    }

    // public void SetTarget(GameObject gameObject, bool instantMove = false) {
    //     if (gameObject == null) {
    //         return;
    //     }
    //     this.target = gameObject.transform;
    // }   

    public void SetTarget(Transform targetTransform, bool trackRotation, bool instantMove) {
        if (targetTransform == null) {
            return;
        }
        this._isSettingTarget = true;

        this._target = targetTransform;

        if(trackRotation) {
            this.transform.rotation = this._target.rotation;
        }
        if(instantMove)
        {
            this.transform.position = this._target.position;
        }
        this._isSettingTarget = false;
    }
    #nullable enable

    public void MoveCamera()  {
        if (this._target == null) {
            return;
        }

        if(this._isSettingTarget)
        {
            return;
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, this._target.position, this.cameraTrackingSpeed * Time.deltaTime);
        this.transform.rotation = this.transform.rotation;
    }
    
}
}
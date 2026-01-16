using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DPS.Common
{
    public class BillBoardSpriteController : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The main Camera in the scene.  Can be modified if necessary.")]
    private GameObject mainCamera;

    [SerializeField]
    [Tooltip("Determines if the sprite should bend to face the camera should its raise or lower")]
    private bool billBoardVertically;

    [SerializeField]
    [Tooltip("The update mode that the bill board should update in")]
    private BillBoardUpdateMode billBoardUpdateMode = BillBoardUpdateMode.LateUpdate;

    // [SerializeField]
    // [Range(0f, 15f)]
    private float billBoardAngle = 0f;


    void Awake() {
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update() {
        if( mainCamera == null) {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            return;
        }
        if (this.billBoardUpdateMode != BillBoardUpdateMode.Update) {
            return;
        }
        if (this.billBoardVertically) {
            this.PerformVerticalBillBoard();
            return;
        }
        this.PerformStandardbillBoard();
    }

    void FixedUpdate() {
        if( mainCamera == null) {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            return;
        }
        if (this.billBoardUpdateMode != BillBoardUpdateMode.FixedUpdate) {
            return;
        }
        if (this.billBoardVertically) {
            this.PerformVerticalBillBoard();
            return;
        }
        this.PerformStandardbillBoard();
    }

    // Update is called once per frame
    void LateUpdate() {
        if( mainCamera == null) {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            return;
        }
        if (this.billBoardUpdateMode != BillBoardUpdateMode.LateUpdate) {
            return;
        }
        if (this.billBoardVertically) {
            this.PerformVerticalBillBoard();
            return;
        }
        this.PerformStandardbillBoard();
    }

    private void PerformVerticalBillBoard() {
        // Vector3 targetPosition = new Vector3((float) Math.Floor(mainCamera.transform.position.x), (float) Math.Floor(mainCamera.transform.position.y) , (float) Math.Floor(mainCamera.transform.position.z));
        // Vector3 currentPosition = new Vector3((float) Math.Floor(transform.position.x), (float) Math.Floor(transform.position.y), (float) Math.Floor(transform.position.z));
        // transform.LookAt(2 * currentPosition - targetPosition);
        transform.rotation = mainCamera.transform.rotation;
    }

    private void PerformStandardbillBoard() {
        Vector3 rotation = mainCamera.transform.eulerAngles;
        rotation.x = billBoardAngle;
        rotation.z = 0;
        transform.eulerAngles = rotation;
    }

    

    private enum BillBoardUpdateMode {
        Update,
        FixedUpdate,
        LateUpdate
    }
}
}


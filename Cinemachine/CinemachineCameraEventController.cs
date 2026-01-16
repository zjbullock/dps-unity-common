using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using System;

public class CinemachineCameraEventController : MonoBehaviour
{


    [SerializeField]
    private CinemachineVirtualCameraBase cinemachineVirtualCamera;

    public CinemachineVirtualCameraBase CinemachineCamera { get => this.cinemachineVirtualCamera; }

    [SerializeField]
    protected CinemachineBrainController cinemachineBrainController;

    private void AddCineMachineEvent(ICinemachineCamera camera, System.Action callBack) {
        if (this.cinemachineBrainController is null) {
            return;
        }
        this.cinemachineBrainController.AddBlendCompleteEvent(camera, callBack);
    }

    protected virtual void Awake() {
        this.cinemachineVirtualCamera = GetComponent<CinemachineVirtualCameraBase>();
        try {
            this.cinemachineBrainController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrainController>();

        } catch (Exception e) {
            Debug.LogError(e);
            Debug.LogError("Unable to get the camera brain controller");
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public void SetCameraPriority(bool prioritize) {
        if (this.cinemachineVirtualCamera == null) {
            Debug.LogError("No cinemachine Virtual camera!");
            return;
        }
        
        if(prioritize) {
            this.cinemachineVirtualCamera.Prioritize();
        }
        // this.cinemachineVirtualCamera.Priority.Value = prioritize ? 1000 : -1;
    }


    public void AddCinemachineEvent(Action callBack) {
        this.AddCineMachineEvent(this.cinemachineVirtualCamera, callBack);
    }

}

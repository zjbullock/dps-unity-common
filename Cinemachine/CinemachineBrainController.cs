using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;

/**
    Special thanks to Thsbrown @ https://github.com/Breakstep-Studios/toms-cinemachine-tools/tree/master for this excellent tool
*/
public class CinemachineBrainController : MonoBehaviour
{
     /// <summary>
    /// The cinemachine brain this class will help with
    /// </summary>
    [Tooltip("The cinemachine brain this class will help.")]
    [SerializeField]
    private CinemachineBrainEvents cinemachineBrainEvents;

    [SerializeField]
    private CinemachineBrain _cinemachineBrain;

    /// <summary>
    /// Invoked when the camera has completely blended from one camera to another. Signature = toCamera, fromCamera.
    /// </summary>
    [Space]
    // [Tooltip("Invoked when the camera has completely blended from one camera to another. Signature = toCamera, fromCamera.")]
    // public CinemachineBrain.VcamActivatedEvent onCameraBlendCompleteEvent;
    
    // [Space]
    // [Tooltip("Invoked when the camera has completely blended from one camera to another for temporary actions. Signature = toCamera, fromCamera.")]
    // public CinemachineBrain.VcamActivatedEvent onCameraBlendCompleteEventTemp;
    /// <summary>
    /// In initial camera we were blending from. In other words the camera that started the blend sequence. 
    /// </summary>
    private ICinemachineCamera fromCamera;
    
    /// <summary>
    /// The polling rate at which we will check if the IsBlending flag is set to false.
    /// </summary>
    [SerializeField]
    private const float isBrainBlendingFrequency = 0.1f;
    

    public void AddBlendCompleteEvent(ICinemachineCamera toCamera, System.Action callBack) {
        void newEvent(ICinemachineMixer camera1, ICinemachineCamera camera2)
        {
            if (toCamera != camera1)
            {
                return;
            }
            Debug.Log("calling camera blend callback");
            callBack();

            this.cinemachineBrainEvents.BlendFinishedEvent.RemoveListener(newEvent);
        }


        // this.cinemachineBrainEvents.BlendFinishedEvent.AddListener(newEvent);
    }


    private void AddCineMachineBrainListener() {
        if (this.cinemachineBrainEvents == null) {
            return;
        }
        // this.cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivatedEventHandler);
    }
    

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpController : MonoBehaviour {
    public float floatingNumberBoxTimer = 0.5f;
    public float floatingNumberSpeed = 2f;
    private bool timerIsRunning;

    [SerializeField]
    private RectTransform rectTransform;

    private TextMeshPro textMesh;

    void Awake() {
        timerIsRunning = false;
        textMesh = GetComponent<TextMeshPro>();
        this.rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(timerIsRunning) {
            if(floatingNumberBoxTimer > 0) {
                transform.position += new Vector3(0, floatingNumberSpeed, 0) * Time.deltaTime;
                floatingNumberBoxTimer -= Time.deltaTime;
            } else {
                Destroy(gameObject);
            }
        }
    }

    public void TriggerPopup(string text, Color color) {
        textMesh.text = text;
        textMesh.color = color;
        this.timerIsRunning = true;
    }

    public void TriggerPopup(string text, Vector2 startingPosition, Color color) {
        if (this.rectTransform == null) {
            return;
        }
        textMesh.text = text;
        textMesh.color = color;
        this.rectTransform.anchoredPosition = startingPosition;
        this.timerIsRunning = true; 
    }
}

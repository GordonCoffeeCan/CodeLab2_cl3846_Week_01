using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour {

    [SerializeField]
    private Transform[] playerGraphic;

    private int graphicIndex = 0;

    [HideInInspector]
    public const float MOVE_INTERVAL = 0.64f;

    private Quaternion targetRotation;
    private Vector3 targetPosition;

    private bool readyToRotate = true;
    private Transform graphicClone;

    private void Awake() {
        if (playerGraphic == null) {
            Debug.LogError("No graphic reference in Player Graphic!");
        }
    }

    // Use this for initialization
    void Start () {
        CreateGraphic();
    }
	
	// Update is called once per frame
	void Update () {
        if(GameManager.instance.gameState == GameManager.GameState.Restarting) {
            return;
        }
        ChangeAndRotate();
        Controller();
    }

    private void ChangeAndRotate() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            targetRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.RoundToInt(targetRotation.eulerAngles.z) - 180));
            CreateGraphic();
            readyToRotate = false;
        }

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, 0.2f);

        /*if (Mathf.RoundToInt(this.transform.rotation.eulerAngles.z) % 90 == 0) {
            readyToRotate = true;
        }*/
    }

    private void Controller() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            targetPosition.x -= MOVE_INTERVAL;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            targetPosition.x += MOVE_INTERVAL;
        }

        if(targetPosition.x < -GameManager.instance.ringsCount / 2 * MOVE_INTERVAL + MOVE_INTERVAL) {
            targetPosition.x += MOVE_INTERVAL;
        }else if (targetPosition.x > GameManager.instance.ringsCount / 2 * MOVE_INTERVAL - MOVE_INTERVAL) {
            targetPosition.x -= MOVE_INTERVAL;
        }

        targetPosition.y = this.transform.position.y;

        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, 0.35f);
    }

    private void CreateGraphic() {
        if (graphicClone != null) {
            Destroy(graphicClone.gameObject);
        }
        graphicIndex++;
        if (graphicIndex >= playerGraphic.Length) {
            graphicIndex = 0;
        }
        graphicClone = Instantiate(playerGraphic[graphicIndex], this.transform);
    }
}

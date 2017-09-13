using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingsController : MonoBehaviour {

    private float moveTimer = 2;
    private float currentMoveTimer = 0;

    private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
        currentMoveTimer = moveTimer;
        moveTimer = 0;
        targetPosition = this.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        moveTimer -= GameManager.instance.gameSettings.gameSpeed * Time.deltaTime;
		if(moveTimer <= 0) {
            targetPosition = new Vector3(this.transform.position.x, this.transform.position.y - 1.28f, 0);
            moveTimer = currentMoveTimer;
        }

        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, 0.4f);

        if (this.transform.position.y <= -GameManager.instance.screenPosition.y - 1.28f) {
            Destroy(this.gameObject);
        }
	}
}

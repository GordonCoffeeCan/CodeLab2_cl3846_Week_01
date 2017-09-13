using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRings : MonoBehaviour {

    [SerializeField]
    private Transform ring;
    [SerializeField]
    private Ball[] balls;
    private int ringWallLength = 0;
    private float stepDistance = 0.64f;

    private int colorBallIndex = 0;
    private int colorBallIndexInRings = 0;

    private void Awake() {
        if (ring == null) {
            Debug.LogError("No ring prefab assigned!");
        }

        if (balls == null) {
            Debug.LogError("No ball prefab assigned!");
        }
    }

    // Use this for initialization
    void Start () {
        ringWallLength = GameManager.instance.ringsCount;
        colorBallIndex = Random.Range(0, balls.Length);

        colorBallIndexInRings = Random.Range(0, ringWallLength - 1);

        for (int i = 0; i < ringWallLength; i++) {
            if(i == colorBallIndexInRings || i == colorBallIndexInRings + 1) {
                Instantiate(balls[colorBallIndex], new Vector3(stepDistance * i, this.transform.localPosition.y, 0), Quaternion.identity, this.transform);
            } else {
                Instantiate(ring, new Vector3(stepDistance * i, this.transform.localPosition.y, 0), Quaternion.identity, this.transform);
            }
            
        }

        this.transform.localPosition = new Vector3(-ringWallLength / 2 * stepDistance + 0.32f, this.transform.localPosition.y, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

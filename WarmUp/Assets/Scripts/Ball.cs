using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private Rigidbody2D rig;
    private CircleCollider2D circleCollider;

    private bool exploded = false;

	// Use this for initialization
	void Start () {
        rig = this.GetComponent<Rigidbody2D>();
        circleCollider = this.GetComponent<CircleCollider2D>();
        rig.bodyType = RigidbodyType2D.Kinematic;
        circleCollider.isTrigger = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.gameState == GameManager.GameState.Restarting && exploded == false) {
            rig.bodyType = RigidbodyType2D.Dynamic;
            circleCollider.isTrigger = false;

            Vector2 _forceDir = new Vector2(Random.Range(-4f, 4f), Random.Range(0, 5f));
            _forceDir.Normalize();

            rig.AddForce(_forceDir * Random.Range(2, 8), ForceMode2D.Impulse);

            exploded = true;
        }

        if(this.transform.position.y > GameManager.instance.screenPosition.y + 1) {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D _col) {
        if (_col.gameObject.name != this.gameObject.name) {
            GameManager.instance.gameState = GameManager.GameState.Dead;
        } else {
            GameManager.instance.isScored = true;
        }
    }
}

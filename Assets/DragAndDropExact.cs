using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragAndDropExact : MonoBehaviour {
	private Vector3 screenPoint;
	private bool dragging = false;
	private int LAYER = 30; // Use layer 30 to test collision with
	private int LAYER_MASK = 1 << 30; // Use layer 30 to test collision with

	public Rigidbody2D draggableRigidBody;
	[Tooltip("The collider must be a circle with a trigger")]
	public CircleCollider2D dragCollider;

	void Start() {
		// Can you tell the real circlecollider difference?  It's a trigger :)
		CircleCollider2D[] components = dragCollider.GetComponentsInParent<CircleCollider2D> ();
		foreach (CircleCollider2D component in components) {
			if (component.isTrigger) {
				dragCollider = component;
				break;
			}
		}

		// Create a new collider in its own layer attached to the game object
		GameObject colliderHolder = new GameObject("Drag Collider");
		colliderHolder.transform.parent = dragCollider.gameObject.transform;
        colliderHolder.transform.localPosition = new Vector3 (0,0,0);
		colliderHolder.layer = LAYER;
		CircleCollider2D newCollider = colliderHolder.AddComponent<CircleCollider2D>();
		newCollider.radius = dragCollider.radius;
		newCollider.center = dragCollider.center;
        newCollider.isTrigger = true;
		Destroy (dragCollider);
        dragCollider = newCollider;
	}


	public void Update(){
		if (Input.GetMouseButtonDown (0)) {
			//offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit2D hit = Physics2D.Raycast (mousePoint, Vector2.zero, Mathf.Infinity, LAYER_MASK);
            Debug.Log (hit.collider);
            if (hit.collider) {
                Vector3 offset = gameObject.transform.position - mousePoint;
                StartCoroutine ("DragObject", offset);
            }
		}
	}
    IEnumerator DragObject(Vector3 offset) {
        // Stop the ragdoll from interacting with the environment
        draggableRigidBody.isKinematic = true;

        int maxTicks = 5;
        TailList<Tuple<float, Vector3>> ticks = new TailList<Tuple<float, Vector3>> (maxTicks);

		while (Input.GetMouseButton(0)) {
            // Move the ragdoll to the mouse location
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;

            // Store his location
            ticks.Add (new Tuple<float, Vector3>(Time.time, curPosition));

			yield return null;
        }

        // Determine velocity based off of last few ticks during the throw
        Tuple<float, Vector3> lastTick = null;
        Vector3 velocityVector = new Vector3();
        int velocitySamples = 0;
        foreach (Tuple<float, Vector3> tick in ticks) {
            if (lastTick != null) {
                ++velocitySamples;
                velocityVector += (tick.Item2-lastTick.Item2) / (tick.Item1-lastTick.Item1);
            }
            lastTick = tick;
        }
        if (velocitySamples != 0)
            velocityVector /= velocitySamples;

        draggableRigidBody.velocity = velocityVector * 3;
        draggableRigidBody.isKinematic = false;
	}
}
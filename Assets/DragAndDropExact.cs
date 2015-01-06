using UnityEngine;
using System.Collections;

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

		Debug.Log(draggableRigidBody);
		Debug.Log(dragCollider.isTrigger);

		// Create a new collider in its own layer attached to the game object
		GameObject colliderHolder = new GameObject("Drag Collider");
		colliderHolder.transform.parent = dragCollider.gameObject.transform;
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

			Vector3 offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			StartCoroutine ("DragObject", offset);
		}
	}
    IEnumerator DragObject(Vector3 offset) {
        draggableRigidBody.isKinematic = true;

        Vector3 startPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
        int ticks = 0;
		while (Input.GetMouseButton(0)) {
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;
            ++ticks;
			yield return null;
        }

        draggableRigidBody.velocity = (transform.position - startPosition) / ticks * 300;
        Debug.Log (ticks);
        Debug.Log (transform.position - startPosition);
        Debug.Log (draggableRigidBody.velocity);
        draggableRigidBody.isKinematic = false;
	}
}
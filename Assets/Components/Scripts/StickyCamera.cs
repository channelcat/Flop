using UnityEngine;
using System.Collections;

public static class BoxCollider2DExtensions
{
    public static bool containsPoint(this BoxCollider2D box, Vector2 point)
    {
        Vector2 halfSize = new Vector2 (box.bounds.size.x / 2.0f, box.bounds.size.y / 2.0f);
        Vector2 middle = box.bounds.center;
        Vector2 topLeft = middle - halfSize;
        Vector2 bottomRight = middle + halfSize;

        return ( point.x >= topLeft.x && point.x <= bottomRight.x ) && ( point.y >= topLeft.y && point.y <= bottomRight.y );
    }
}

public class StickyCamera : MonoBehaviour {

    private enum MODES : int { CONTROL, FOLLOW };
    private int mode;
    private Vector3 moveVelocity = Vector3.zero;
    private Vector3 zoomVelocity = Vector3.zero;
    private Vector3 currentTargetPosition;
    private float targetSize;
    private float currentTargetSize;
    private Tuple<BoxCollider2D, float>[] anchorPoints;

    public float dampTime = 0.15f;
    public Transform target;

	// Use this for initialization
	void Start () {
        mode = (int)MODES.FOLLOW;

        Messenger.AddListener<GameObject>( "StartDrag", startDrag );
        Messenger.AddListener<GameObject>( "StopDrag", stopDrag );

        // Precalculate camera size
        Vector3 cameraSize = camera.ViewportToWorldPoint (new Vector3 (1, 1)) - camera.ViewportToWorldPoint (new Vector3 ());
        float cameraHypotenuse = Mathf.Sqrt(cameraSize.x*cameraSize.x+cameraSize.y*cameraSize.y);
        float hypotenuseToCameraSize = camera.orthographicSize/cameraHypotenuse;

        // Store original camera size
        targetSize = camera.orthographicSize;

        // Get all anchors
        GameObject[] anchorObjects = GameObject.FindGameObjectsWithTag("CameraAnchor");
        int anchorPoint = 0;
        anchorPoints = new Tuple<BoxCollider2D, float>[anchorObjects.Length];
        foreach (GameObject anchor in anchorObjects) {
            BoxCollider2D _box = anchor.GetComponent<BoxCollider2D>();
            Vector3 _boxSize = _box.bounds.size;
            float hypotenuse = Mathf.Sqrt(_boxSize.x*_boxSize.x + _boxSize.y*_boxSize.y);

            anchorPoints[anchorPoint++] = new Tuple<BoxCollider2D, float>(_box, hypotenuse*hypotenuseToCameraSize);
        }

        if (!target)
            Debug.LogError ("Camera target is missing!");
    }
    
    void startDrag (GameObject draggedObject)
    {
        mode = (int)MODES.CONTROL;
    }
    
    void stopDrag (GameObject draggedObject)
    {
        mode = (int)MODES.FOLLOW;
    }

    // Update is called once per frame
    void Update () 
    {
        if (!target)
            return;

        if (mode == (int)MODES.FOLLOW) { 
            currentTargetPosition = target.position;
            currentTargetSize = targetSize;

            // Check if the subject is within a camera anchor
            foreach (Tuple<BoxCollider2D, float> anchorPoint in anchorPoints) {
                BoxCollider2D box = anchorPoint.Item1;

                if (box.containsPoint(target.position)) {
                    currentTargetPosition = box.bounds.center;
                    currentTargetSize = anchorPoint.Item2;
                    break;
                }
            }
        } else if (mode == (int)MODES.CONTROL) {
            currentTargetPosition = transform.position;
        }

        Vector3 point = camera.WorldToViewportPoint(currentTargetPosition);
        Vector3 delta = currentTargetPosition - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref moveVelocity, dampTime);
        camera.orthographicSize = Vector3.SmoothDamp(new Vector3(camera.orthographicSize,0), new Vector3(currentTargetSize,0), ref zoomVelocity, dampTime).x;
    }
}

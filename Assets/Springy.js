#pragma strict

private var springJoint : SpringJoint2D;

function Start()
{
    springJoint = GetComponent(SpringJoint2D);
    springJoint.connectedAnchor.x = this.gameObject.transform.position.x;
    springJoint.connectedAnchor.y = this.gameObject.transform.position.y;
}

function Update () {

}
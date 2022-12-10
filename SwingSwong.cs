using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSwong : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpringJoint2D joint;
    private Vector2 anchorPoint;
    private GameObject anchor;
    private Vector2 directionOfSwing;
    private LineRenderer lr;

    public float gravity = -9.8f;
    public float terminalVelocity = -25f;
    public float launchForce = 25f;
    public float maxSpeed = 45f;

    public GameObject anchorPrefab;
    public LayerMask whatIsAnchorable;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            StartSwing();
        }else if(Input.GetMouseButtonUp(0)){
            StopSwing();
        }

        if(joint != null){
            DrawDebugInfo();
            RotateSprite();
        }

        directionOfSwing = rb.velocity.normalized;
    }

    void LateUpdate(){
        if(joint != null){
            lr.enabled = true;
            lr.SetPosition(0,transform.position);
            lr.SetPosition(1,anchorPoint);
        }else{
            lr.enabled = false;
        }
    }

    void FixedUpdate(){
        AddGravity();
        rb.velocity = Vector2.ClampMagnitude(rb.velocity,maxSpeed);
    }

    void DrawDebugInfo(){
        Debug.DrawLine(joint.anchor,joint.connectedAnchor,Color.green);
        Debug.DrawRay(transform.position,directionOfSwing,Color.red);
    }

    void RotateSprite(){
        transform.up = (anchorPoint - (Vector2)transform.position).normalized;
    }

    void AddGravity(){
        if(rb.velocity.y >= terminalVelocity){
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y + gravity * Time.fixedDeltaTime);
        }else{
            rb.velocity = new Vector2(rb.velocity.x,terminalVelocity);
        }
    }

    void StartSwing(){
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, GetDirectionToMouse(transform.position),GetDistanceToMouse(transform.position),whatIsAnchorable);

        if(hit.collider != null){
            anchorPoint = hit.point;
            anchor = Instantiate(anchorPrefab,anchorPoint,anchorPrefab.transform.rotation);
            
            this.gameObject.AddComponent<SpringJoint2D>();
            joint = GetComponent<SpringJoint2D>();
            joint.connectedBody = anchor.GetComponent<Rigidbody2D>();
            joint.autoConfigureDistance = false;

            joint.distance = 8f;
            joint.dampingRatio = 1f;
            joint.frequency = 0.8f;
            joint.breakForce = Mathf.Infinity;
        }
    }

    void StopSwing(){
        Destroy(joint);
        Destroy(anchor);
        rb.AddForce(directionOfSwing * (launchForce * (rb.velocity.magnitude/maxSpeed)),ForceMode2D.Impulse);
    }

    Vector2 GetDirectionToMouse(Vector2 startingPosition){
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionFromStartToMouse = mousePosition - startingPosition;
        directionFromStartToMouse = directionFromStartToMouse.normalized;
        return directionFromStartToMouse;
    }

    float GetDistanceToMouse(Vector2 startingPosition){
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distanceFromStartToMouse = Vector2.Distance(startingPosition,mousePosition);
        return distanceFromStartToMouse;
    }

}

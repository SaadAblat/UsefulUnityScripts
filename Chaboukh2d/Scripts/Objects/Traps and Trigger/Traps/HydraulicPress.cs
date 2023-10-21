using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraulicPress : TrapsBase
{
    [SerializeField] Rigidbody2D metalBarBody;
    [SerializeField] float force;
    [SerializeField] float speed;
    Vector3 startPos;
    Vector3 ReturnDirection;
    bool facingRight;
    bool ReturnToStartPos;


    private void Start()
    {
        startPos = metalBarBody.transform.localPosition;
        // push direction is left
        if (Mathf.Sign(force) < 0)
        {
            ReturnDirection = Vector3.right;
            facingRight = false;
        }

        // push direction is right
        else if (Mathf.Sign(force) > 0)
        {
            ReturnDirection = Vector3.left;
            facingRight = true;
        }
    }

    public override void ActivateTrap()
    {
        if (!ReturnToStartPos)
        {
            base.ActivateTrap();
            ReturnToStartPos = true;
            metalBarBody.AddForce(Vector2.right * force, ForceMode2D.Impulse);
        }

    }

    public override void BehaviorOnUpdate()
    {
        if (ReturnToStartPos)
        {
            if (facingRight)
            {

                metalBarBody.transform.position += speed * Time.deltaTime * ReturnDirection;
                if (metalBarBody.transform.localPosition.x <= startPos.x - 0.1f)
                {
                    metalBarBody.transform.localPosition = startPos;
                    ReturnToStartPos = false;
                }
            }
            else
            {
                metalBarBody.transform.position += speed * Time.deltaTime * ReturnDirection;
                if (metalBarBody.transform.localPosition.x >= startPos.x + 0.1f)
                {
                    metalBarBody.transform.localPosition = startPos;
                    ReturnToStartPos = false;
                }
            }
        }

    }
}

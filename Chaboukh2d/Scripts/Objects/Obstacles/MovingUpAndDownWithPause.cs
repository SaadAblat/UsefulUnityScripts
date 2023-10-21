using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUpAndDownWithPause : MonoBehaviour
{

    [SerializeField] float pauseTime;
    [SerializeField] float speed;
    [SerializeField] Transform graphic;

    [SerializeField] Transform up;
    [SerializeField] Transform down;
    [SerializeField] bool IsUp;
    bool inPause =true;
    float pauseTimer = 0;


    private void Start()
    {
        if (IsUp)
        {
            graphic.position = up.position;
        }
        else
        {
            graphic.position = down.position;

        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inPause)
        {
            if (IsUp)
            {

                graphic.transform.position += speed * Time.fixedDeltaTime * Vector3.down;
                if (graphic.transform.position.y <= down.position.y)
                {
                    IsUp = false;
                    inPause = true;
                    pauseTimer = 0;
                }
            }
            else
            {
                graphic.transform.position += speed * Time.fixedDeltaTime * Vector3.up;
                if (graphic.transform.position.y >= up.position.y)
                {
                    IsUp = true;
                    inPause = true;
                    pauseTimer = 0;
                }
            }
        }
        else
        {
            pauseTimer += Time.fixedDeltaTime;
            if (pauseTimer >= pauseTime)
            {
                inPause = false;
            }
        }
    }
}

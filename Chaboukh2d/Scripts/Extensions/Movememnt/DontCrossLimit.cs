using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontCrossLimit : MonoBehaviour
{
    [SerializeField] Transform RightLimit;
    [SerializeField] Transform LeftLimit;
    internal bool hasCrossLimit;
    GameObject player;
    // Start is called before the first frame update
    private void Start()
    {
        hasCrossLimit = false;
        player = PlayerScript.Instance.gameObject;
    }

    void Update()
    {
        if (player != null)
        {
            if (player.transform.position.x > RightLimit.position.x || player.transform.position.x < LeftLimit.position.x)
            {
                hasCrossLimit = true;
            }
            else
            {
                hasCrossLimit = false;
            }
        }

    }
}

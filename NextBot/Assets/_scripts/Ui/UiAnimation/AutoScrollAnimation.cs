using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class AutoScrollAnimation : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 1.0f;
    public bool scrollHorizontally = true;
    public List<Transform> imagesToScale; // This should be a list of the image objects you want to scale.
    public float maxScale = 2.0f; // The maximum scale at the center.
    public float minScale = 1.0f; // The minimum scale at the extremes.
    public float scaleSpeed = 1.0f; // Adjust the speed of scaling.


    void Update()
    {
        if (scrollHorizontally)
        {
            float newPosition = scrollRect.horizontalNormalizedPosition + (scrollSpeed * Time.deltaTime);
            if (newPosition > 1.0f)
            {
                newPosition = 0.0f;
            }
            scrollRect.horizontalNormalizedPosition = newPosition;
        }


      

    }
}

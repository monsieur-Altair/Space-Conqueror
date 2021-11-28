using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    public Text multiTouchInfoDisplay;
    private int maxTapCount = 0;
    private string multiTouchInfo;

    private Touch theTouch;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        multiTouchInfo = string.Format("Max tap count: {0}\n", maxTapCount);

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                theTouch = Input.GetTouch(i);
		
                multiTouchInfo +=
                    string.Format("\nTouch {0} \nPosition {1} \nTap Count: {2} \nFinger ID: {3}\nRadius: {4} ({5}%)\n",
                i, theTouch.position, theTouch.tapCount, theTouch.fingerId, theTouch.radius,
                ((theTouch.radius/(theTouch.radius + theTouch.radiusVariance)) * 100f).ToString("F1"));

                if (theTouch.tapCount > maxTapCount)
                {
                    maxTapCount = theTouch.tapCount;
                }
            }
        }

        multiTouchInfoDisplay.text = multiTouchInfo;
    }
}
using UnityEngine;
using System.Collections;
using Leap;

public class GestureHands : MonoBehaviour {

    Controller controller;

	// Use this for initialization
	void Start () {
        controller = new Controller();
        controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        controller.Config.SetFloat("Gesture.Swipe.MinLength", 200.0f);
        controller.Config.SetFloat("Gesture.Swipe.MinVelocity", 750f);
        controller.Config.Save();

        controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
        controller.Config.SetFloat("Gesture.KeyTap.MinDownVelocity", 10.0f);
        controller.Config.SetFloat("Gesture.KeyTap.HistorySeconds", 0.3f);
        controller.Config.SetFloat("Gesture.KeyTap.MinDistance", 0.5f);
        controller.Config.Save();

        controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
        controller.Config.SetFloat("Gesture.ScreenTap.MinForwardVelocity", 10.0f);
        controller.Config.SetFloat("Gesture.ScreenTap.HistorySeconds", 0.3f);
        controller.Config.SetFloat("Gesture.ScreenTap.MinDistance", 0.5f);
        controller.Config.Save();


	}
	
	// Update is called once per frame
	void Update () {
	    //Need to access the frames of the leap motion
        Frame frame = controller.Frame();
        GestureList gestures = frame.Gestures();
        //string poser = frame.Hands[0].PalmPosition.z.ToString();//frame.Finger(1).TipPosition.x.ToString();
       
        
        /*
        Pointable pointable = frame.Pointables.Frontmost;
        Pointable.Zone zone = pointable.TouchZone;
        float distance = pointable.TouchDistance;

        Vector stabilizedPosition = pointable.StabilizedTipPosition;
        Vector3 g = stabilizedPosition.ToUnityScaled();
        InteractionBox iBox = frame.InteractionBox;
        Vector normalizedPosition = iBox.NormalizePoint(stabilizedPosition);
        Vector3 tmpPos = Camera.main.WorldToScreenPoint(transform.position);
        float x = normalizedPosition.x * UnityEngine.Screen.width;
        float y = normalizedPosition.y * UnityEngine.Screen.height;


        Debug.Log(x + " | " + y); */
        //Debug.Log(g.x*2 + " | " + (g.y*3-1f) + " | " + g.z*3);

        //Debug.Log(poser);

        for (int i = 0; i < gestures.Count; i++)
        {
            Gesture gestureN = gestures[i];
         /*   if (gestureN.Type == Gesture.GestureType.TYPE_SWIPE)
            {
                SwipeGesture Swipe = new SwipeGesture(gestureN);
                Vector swipeDirection = Swipe.Direction;

                Finger finger = new Finger(Swipe.Pointable);

                if (Finger.FingerType.TYPE_THUMB == finger.Type)
                {
                    if (swipeDirection.x < 0)
                    {
                        Debug.Log("Left");
                    }
                    else if (swipeDirection.x > 0)
                    {
                        Debug.Log("Right");
                    }
                }
            }*/
            if (gestureN.Type == Gesture.GestureType.TYPE_KEY_TAP)
            {
                KeyTapGesture thumbTap = new KeyTapGesture(gestureN);
                Vector thumbTapDir = thumbTap.Direction;
                Finger finger = new Finger(thumbTap.Pointable);
                //Pointable tappingPointable = thumbTap.Pointable;
                //Finger finger = thumbTap.Pointable as Finger;
                if (Finger.FingerType.TYPE_THUMB == finger.Type)
                {
                    Debug.Log("THUMB TIME" + Random.Range(0f,20f));
                }
            }
            if (gestureN.Type == Gesture.GestureType.TYPE_SCREEN_TAP)
            {
                ScreenTapGesture thumbTap = new ScreenTapGesture(gestureN);
                Vector thumbTapDir = thumbTap.Direction;
                Finger finger = new Finger(thumbTap.Pointable);
                //Pointable tappingPointable = thumbTap.Pointable;
                //Finger finger = thumbTap.Pointable as Finger;
                if (Finger.FingerType.TYPE_THUMB == finger.Type)
                {
                    Debug.Log("THUMBSCREEB TIME" + Random.Range(0f, 20f));
                }
            }
        }






	}
}

using UnityEngine;
using System.Collections;
using Leap;

public class DepthColor : MonoBehaviour {

    public Color color1 = Color.white;
    public Color color2 = Color.black;
    public float duration = 3.0f;
    public float place = 0f; // will be 1 when in complete white. 0 otherwise


    Camera camera;
    float a;
    Canvas c;
    UnityEngine.UI.Text text;

    HandController noob;
    SkeletalHand bob;
    Vector3 index_tip;


	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;

       // HandModel hand_model = GameObject.Find("HandModel").GetComponent<HandModel>();
       noob = GameObject.Find("HandController").GetComponent<HandController>();
       bob = GameObject.Find("KnobbyHand").GetComponent<SkeletalHand>();

        

        //GameObject.Find("HandController").GetLeapController();

        //GUI.backgroundColor = Color.blue;
        //a = GameObject.Find("KnobbyHand").GetComponent<Pointable>().TipPosition.z;//GetComponent<Transform>().position.z;
        //a = GameObject.Find("KnobbyFinger 1").GetComponent<Pointable>().TipPosition.z;//GetComponent<Transform>().position.z;
       text = GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>();
	}

	
	// Update is called once per frame
	void Update () {
        Frame frame = noob.GetFrame();
        color1 = Color.white;
        color2 = Color.black;
        
        //float t = Mathf.PingPong(Time.time, duration) / duration;
       // a = GameObject.Find("KnobbyFinger 1").GetComponent<Pointable>().TipPosition.z;
        //a = GameObject.Find("KnobbyFinger 1").GetComponent<Transform>().position.z;
        text = GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>();
        index_tip = bob.fingers[1].GetTipPosition();
        
        //text.text = frame.Hands[0].PalmPosition.z.ToString();//frame.Finger(1).TipPosition.x.ToString();//index_tip.x.ToString();//bob.GetComponent<Transform>().position.x.ToString();
        a = frame.Hands[0].PalmPosition.z;

        if (a * 0.05f + 0.3f < 1)
        {
            place = 1f;
        }
        else
        {
            place = 0f;
        }

        if (GameObject.Find("Sphere").GetComponent<TouchNumber>().activeToggle == 0)
        {
            camera.backgroundColor = Color.Lerp(color1, color2, a * 0.05f + 0.3f);
        }
        else
        {
            camera.backgroundColor = Color.black;
        }
    }
}

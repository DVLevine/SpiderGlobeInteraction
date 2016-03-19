using UnityEngine;
using System.Collections;
using Leap;


/**
 * Behavior for a single Sphere. Each sphere is cloned as a unique target with ID at program runtime.
 * 
 * Encodes LeapMotion interaction and target behavior.
 * 
 * Communicates with the FittsLaw script to create measurable timed targets
 * 
 * */

public class TouchNumber : MonoBehaviour {

    public float scaleFactor = 0.85f;
    public float r = 0.5f;
    public float g = 0.5f;
    public float b = 0.5f;

    private int m_ID;
    public int activeToggle = 0; // Activated when Depth Color goes white. When hand opened, active toggle is released
    public int mouseToggle = 0; // Activated when GUI mouse button pressed. For testing Fitts law with mouse

    private int selectState = 0; // If Sphere is in Selection Zone
    private int z_state = 0; // z component
    private float z_factor = 0f; // z modification of sensitivity

    private float dist;
    private float whiteLevel = 0;

    private int m_FittsTarget = 0;
    private float m_FittsDistance = 0;

    HandController noob;

    Vector3 prevHandCenter;
    Vector3 HandCenter;
    Vector3 SphereCenter;


	// Use this for initialization
	void Start () {
        Vector3 sphereRadius = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        double geebs = GetComponent<SphereCollider>().radius;
        transform.localScale = sphereRadius;

        Color color = new Color(r, g, b);
        GetComponent<Renderer>().material.SetColor("_Color", color);
	}

    public void assignID(int ID)
    {
        m_ID = ID;
    }

    public void setFittsTarget()
    {
        m_FittsTarget = 1;
    }


    // Method for computing distance from fitts law target init
    public void updateFittsDistance(Vector2 newPos, Vector2 oldPos)
    {
        m_FittsDistance += Vector2.Distance(newPos,oldPos);
    }

    public void resetFittsDistance()
    {
        m_FittsDistance = 0;
    }


    /**  Dan's Original Method   **/

    // Grab Pan
    void GrabPan(Frame frame, HandController noob) {

        if (prevHandCenter == null)
        {
            prevHandCenter = noob.transform.TransformPoint(frame.Hands[0].Fingers[1].TipPosition.ToUnityScaled(false));
            HandCenter = prevHandCenter;
        }
        else
        {
            prevHandCenter = HandCenter;
            HandCenter = noob.transform.TransformPoint(frame.Hands[0].Fingers[1].TipPosition.ToUnityScaled(false));
        }

        // z modification of sensitivity
        z_factor = 1.0f - Mathf.Min(1.0f, HandCenter.z);


        if (frame.Hands[0].GrabStrength == 1){
            float diff = Vector3.Distance(HandCenter, prevHandCenter);
            Vector3 dir = Vector3.Normalize(HandCenter - prevHandCenter);

            GetComponent<Transform>().position += new Vector3((dir.x * diff) * (7 * z_factor), (dir.y * diff) * (7 * z_factor), 0f);
        }
    }

    // Determines color of the sphere, based off on proximity to cursor and whether or not fitts target
    void colorSelect(Frame frame, HandController noob, int isFittsTarget)
    {
        SphereCenter = GetComponent<Transform>().position;
        dist = Vector2.Distance(Vector2.zero, new Vector2(SphereCenter.x,SphereCenter.y))-scaleFactor;

        if (m_FittsTarget == 0)
        {
            GetComponent<Renderer>().material.color = Color.Lerp(Color.cyan, Color.yellow, (dist));
            GetComponent<Transform>().localScale *= (0.8f + (0.8f - Mathf.Min(1.0f, dist)));
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.Lerp(Color.HSVToRGB(0.25f, 0.56f, 0.69f), Color.green, (dist));
            GetComponent<Transform>().localScale *= (1.0f + (0.8f - Mathf.Min(1.0f, dist)));
        }
            
    }

    // Registers whether or not a mouse beam is present - highlight behavior
    void RegisterHit(Frame frame, HandController noob, float distance)
    {
        SphereCenter = GetComponent<Transform>().position;
        if ((SphereCenter.x < .35f && SphereCenter.x > -.35f) && (SphereCenter.y < .35f && SphereCenter.y > -.35f))//(Mathf.Min(0.001f, distance) == distance)
        {

            if (m_FittsTarget == 1)
            {
                whiteLevel += 0.14f;
                if (whiteLevel >= 1f){
                    whiteLevel = 1f;
                }
                GetComponent<Renderer>().material.color = Color.Lerp(Color.cyan, Color.white, whiteLevel);
            }

            //Compute relative difference and trigger color
            prevHandCenter = HandCenter;
            HandCenter = noob.transform.TransformPoint(frame.Hands[0].Fingers[1].TipPosition.ToUnityScaled(false));

            if (whiteLevel == 1)
            {

                if (m_FittsTarget == 1)
                {
                    GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = Color.HSVToRGB(0.25f, 0.56f, 0.69f);
                    Debug.Log("YESSS" + " | " + m_ID);

                    // Send Confirmation of hit. Set back to standard target.
                    GameObject.Find("GameObject").GetComponent<FittsLaw>().triggerConfirmation(m_ID);
                    m_FittsTarget = 0;
                }
                else
                {
                    GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = Color.HSVToRGB(0.56f, 0.25f, 0.69f);
                    Debug.Log("NOOO" + " | " + m_ID);
                }
            }

            //Communication back from Depth Color
        }
        else
        {
            whiteLevel = 0f;
        }
    }

    //** Method Modified for Mouse **/

    void mouseColorSelect(int isFittsTarget)
    {
        if (isFittsTarget == 1)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    void mouseRegisterHit()
    {
        if (Input.GetMouseButtonDown(0))
        {
          
            Ray ray = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.GetComponentInParent<TouchNumber>().m_ID == m_ID)
                    {
                        GetComponent<Renderer>().material.color = Color.white;
                        GameObject.Find("GameObject").GetComponent<FittsLaw>().triggerConfirmation(m_ID);
                        m_FittsTarget = 0;
                    }
                }

            }
        }
    }

	// Update is called once per frame
	void Update () {
        Vector3 sphereRadius = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        transform.localScale = sphereRadius;
        Color color = new Color(r, g, b);
        GetComponent<Renderer>().material.SetColor("_Color", color);

        noob = GameObject.Find("HandController").GetComponent<HandController>();
        Frame frame = noob.GetFrame();


        // Toggle Behavior
        if (GameObject.Find("Main Camera").GetComponent<DepthColor>().place == 1f && frame.Hands[0].GrabStrength == 1){
            activeToggle = 1;
        }

        // Turn Off Grab if leave
        if (frame.Hands[0].GrabStrength == 0)
        {
            activeToggle = 0;
        }

        // Grab is On
        if (activeToggle == 1)
        {
            GrabPan(frame, noob);
            colorSelect(frame, noob, m_FittsTarget);
            RegisterHit(frame, noob, dist);
        }

        // Mouse Mode is On
        if (mouseToggle == 1)
        {
            mouseColorSelect(m_FittsTarget);
            mouseRegisterHit();
        }

	}
}

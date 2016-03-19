using UnityEngine;
using System.Collections;

public class RadialNubsCreator : MonoBehaviour {

    // Instantiates a prefab in a circle
    public GameObject CircleNub;
    public float radius = 2f;
    public int numberOfObjects = 25;
    public float m_scaleFactor = 0.5f;

	// Initialization
	void Start () {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 pos = new Vector3(Mathf.Cos(angle)*radius, Mathf.Sin(angle) * radius,0);
            GameObject Bingo = (GameObject)Instantiate(CircleNub, pos, Quaternion.identity);
            
            // Set Individual Sphere Properties
            Bingo.name = i.ToString();
            Bingo.GetComponent<TouchNumber>().scaleFactor = m_scaleFactor;
            Bingo.GetComponent<TouchNumber>().assignID(i);
        }

	}

    // Update is called once per frame
    void Update()
    {
	       
	}
}

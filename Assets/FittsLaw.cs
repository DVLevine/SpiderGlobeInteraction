using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FittsLaw : MonoBehaviour {

    // Sends Signals to individual Spheres with IDs
    // Spheres send signal back.
    // 25 is 6
    // 1 is 18

    int[] fittsPattern = new int[] { 18, 5, 17, 4, 16, 3, 15, 2, 14, 1, 13, 0, 12, 24, 11, 23, 10, 22, 9, 21, 8, 20, 7, 19, 6 };
    float[] fittsTimes = new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
    int counter = 0; // goes up to 25
    int sent = 0; // state. Have we sent or not?

	// Use this for initialization
	void Start () {
	}
	
    // Send "You are target" Signal
    void sendSignal(int ID)
    {
        GameObject.Find(ID.ToString()).GetComponent<TouchNumber>().setFittsTarget();
    }

    // To be called by sphere
    // Sends confirmation. Ends timer

    // Update - should also send center of target with location when affirmed.
    // Update - should also include distance travelled from previous target

    // public void triggerConfirmation(int ID, Vector 2f centerPos, Vector2f selectPos, float distanceTraveled)
    public void triggerConfirmation(int ID)
    {
        counter += 1;
        sent = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter < 25 && sent == 0){
            // Start time for new
            fittsTimes[counter] = Time.time;

            if (counter > 0){
            // Stop time for old
            fittsTimes[counter - 1] = Time.time - fittsTimes[counter - 1];
            GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text + (counter - 1).ToString() + "-" + counter.ToString() +
            ": " + fittsTimes[counter-1].ToString() + "\n";
            }

            sendSignal(fittsPattern[counter]);
            sent = 1;
        }
        else if (sent == 1)
        {
            // wait
        }
        else if (counter == 25)
        {
            fittsTimes[counter - 1] = Time.time - fittsTimes[counter - 1];
            Debug.Log("GREAT SUCCESS");
        }
        else
        {
            //wait
        }
	
	}
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Run", menuName = "Dinamic/Run")]
public class Run_Data : ScriptableObject {

    public AllLocations_DataScript Locations;

    public List<string> LocationsPull = new List<string>();
    public int CurrentLocation = -1;



   

    //
    public void RunStart() {
        Debug.Log("RUN START");
        CurrentLocation = -1;
        LoadNewLocationScene();
    }

    public void RunEnd() {
        Debug.Log("RUN END");
        SceneManager.LoadScene(1);

    }


    //
    public void LoadNewLocationScene() {

        // Increase Location Number
        CurrentLocation++;
        Debug.Log("LOAD LOCATION" + CurrentLocation.ToString());

        // Disable Controller
        GameObject.Find("Player").GetComponent<PlayerController>().ControllingData.Disable();


        // End
        if (CurrentLocation >= LocationsPull.Count) { RunEnd(); }
        // New Location
        else {
            SceneManager.LoadScene(Locations.LOCATIONS_DATA[LocationsPull[CurrentLocation]].SceneID);

        }
    }
}

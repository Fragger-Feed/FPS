using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Run", menuName = "Dinamic/Run")]
public class Run_Data : ScriptableObject {

    public Locations_Data Locations;

    public List<string> LocationsPull = new List<string>();
    public int CurrentLocation;



    //
    public void RunStart() {
        CurrentLocation = 0;
        Debug.Log("RUN START");
    }

    public void RunEnd() {
        Debug.Log("RUN END");
    }


    //
    public void LoadNewLocationScene() {
        Debug.Log("LOAD LOCATION" + CurrentLocation.ToString());

        CurrentLocation++;
        SceneManager.LoadScene(Locations.LocationsData[LocationsPull[CurrentLocation - 1]].SceneID);
        if (CurrentLocation >= LocationsPull.Count - 1) { RunEnd(); }
    }
}

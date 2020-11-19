using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The controller class that presides over the town map screen.
/// </summary>
public class TownMapController : MonoBehaviour
{
    private LandSquare[,] landMap;
    private List<Building> buildingsOnMap = new List<Building>();

    /// <summary>
    /// When creating the map, this object is created repeatedly to instansiate the grid squares.
    /// </summary>
    public LandSquare landSquarePrototype;

    //Temp variables for creating the starting map. Later I figure they'll come from a singleton database, sor of like the ClassDatabase singleton.
    public Building tempFieldPrototype;
    public Building tempHousingPrototype;
    public Building tempBarracksPrototype;

    /// <summary>
    /// An invisible GO representing where the camera is currently looking. Because we're 
    /// using cinemachine, we move the target instead of moving the camera.
    /// </summary>
    public Transform camTarget;

    /// <summary>
    /// The camera the player is using. Used to figure out where to move the camera target as the player WASDs.
    /// </summary>
    public CinemachineFreeLook virtualCamera;

    void Start()
    {
        landMap = new LandSquare[110,110];
        for (int x = 0; x < 110; x++)
        {
            for (int y = 0; y < 110; y++)
            {
                landMap[x, y] = GameObject.Instantiate<LandSquare>(landSquarePrototype);
                landMap[x, y].Init(new Vector2Int(x,y), false, false, this);
            }
        }

        if (true) //TODO: If starting new game
        {
            SetupStartingMap();
        }
        else
        {
            LoadMap(); //To implement after we've got game persistency going.
        }
    }

    private void SetupStartingMap()
    {
        for (int x = 40; x < 54; x++)
        {
            for (int y = 40; y < 51; y++)
            {
                landMap[x, y].Own();
            }
        }

        Building farm = GameObject.Instantiate<Building>(tempFieldPrototype);
        farm.Init(this, 40,46);
        buildingsOnMap.Add(farm);

        Building housing = GameObject.Instantiate<Building>(tempHousingPrototype);
        housing.Init(this, 40, 41);
        buildingsOnMap.Add(housing);

        Building barracks = GameObject.Instantiate<Building>(tempBarracksPrototype);
        barracks.Init(this, 44, 45);
        buildingsOnMap.Add(barracks);
    }

    internal void SetLandVisible(int x, int y)
    {
        if (x>0&&x<100&&y>0&&y<100)
        {
            landMap[x, y].visible = true;
        }
    }

    private void LoadMap()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        Vector3 dir = virtualCamera.transform.forward;
        dir.y = 0;
        camTarget.Translate(dir * Input.GetAxis("Vertical"));
        Vector3 sideDir = new Vector3(dir.z, 0, -dir.x);
        camTarget.Translate(sideDir * Input.GetAxis("Horizontal"));
    }
}

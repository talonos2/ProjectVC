using Cinemachine;
using PathFind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The controller class that presides over the combat screen.
/// </summary>
public class AutoChessManager : MonoBehaviour
{
    /// <summary>
    /// A pointer to the green bar that represents the ratio of enemy power to player power.
    /// </summary>
    public Image bar;

    /// <summary>
    /// How wide the map is.
    /// </summary>
    public int width = 0;

    /// <summary>
    /// How tall the map is.
    /// </summary>
    public int height = 0;

    /// <summary>
    /// The grid that represents which squares contain which soldiers.
    /// </summary>
    public ChessGrid grid;

    /// <summary>
    /// The camera that looks at the soldiers.
    /// </summary>
    public CinemachineTargetGroup groupCam;

    internal float speedNormalizationDivisor = 1;
    private List<Soldier> allSoldiers = new List<Soldier>();

    void Start()
    {
        float[,] costs = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                costs[x, y] = 1.0f;
            }
        }
        grid = new ChessGrid(width, height, costs);
    }

    internal IEnumerable<Soldier> GetAllEnemiesOf(Owner owner)
    {
        List<Soldier> toReturn = new List<Soldier>();
        foreach (Soldier s in allSoldiers)
        {
            if (s.owner != owner)
            {
                toReturn.Add(s);
            }
        }
        return toReturn;
    }

    internal void AddSoldierToBattle(Soldier toAdd)
    {
        allSoldiers.Add(toAdd);
        toAdd.WaitABit();
        int totalSpeed = 0;
        int numberOfFriendlies = 0;
        foreach (Soldier s in allSoldiers)
        {
            if (s.owner == Owner.PLAYER)
            {
                totalSpeed += s.stats.Spd;
                numberOfFriendlies++;
            }
        }

        if (numberOfFriendlies > 0)
        {
            speedNormalizationDivisor = (float)totalSpeed/ (float)numberOfFriendlies;
        }

        groupCam.AddMember(toAdd.transform,1,1);
    }

    void Update()
    {
        float enemyPower = 0;
        float playerPower = 0;
        foreach (Soldier s in allSoldiers)
        {
            if (!s.IsDead)
            {
                if (s.owner == Owner.PLAYER)
                {
                    playerPower += s.GetCurrentPower();
                }
                else
                {
                    enemyPower += s.GetCurrentPower();
                }
            }
        }
        bar.fillMethod = Image.FillMethod.Horizontal;
        bar.type = Image.Type.Filled;
        bar.fillAmount = playerPower / (playerPower + enemyPower);
    }

    internal void RemoveSoldierFromBattle(Soldier toRemove)
    {
        allSoldiers.Remove(toRemove);
    }
}

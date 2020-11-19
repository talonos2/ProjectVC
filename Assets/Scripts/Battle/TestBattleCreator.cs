using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Debug class to set up troops to run a mock battle.
/// 
/// Would never be used in an actual game; instead, the battle would be set up according to values
/// obtained from the metagame progression.
/// </summary>
public class TestBattleCreator : MonoBehaviour
{
    public Soldier soldierPrefab;
    public AutoChessManager manager;
    public Texture catgirl;
    public Texture catgirlSwap;
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                // Originally, all units were Gatandra Ferals before I started implementing the Aifrenburg classes.
                // Yes, the Gatandra were a race of cat people. And I haven't bothered to change the GO names since.
                CreateAndPlaceTestSoldier(x, y+4, Owner.PLAYER, "Good Catgirl "+(1+x+y*3), .2f, x + y * 3, 0, new MeleeAI());
                CreateAndPlaceTestSoldier(x + 25, y + 24, Owner.ENEMY, "Evil Catgirl " + (1 + x + y * 3), .8f, x + y * 3, 0, new MeleeAI());
                CreateAndPlaceTestSoldier(x + 3, y + 4, Owner.PLAYER, "Good Catgirl " + (1 + x + y * 3), .0f, x + y * 3, 1, new ArcherAI());
                CreateAndPlaceTestSoldier(x + 20, y + 24, Owner.ENEMY, "Evil Catgirl " + (1 + x + y * 3), 0.86f, x + y * 3, 1, new ArcherAI());
                CreateAndPlaceTestSoldier(x + 6, y + 4, Owner.PLAYER, "Good Catgirl " + (1 + x + y * 3), 0, x + y * 3, 2, new MeleeAI());
                CreateAndPlaceTestSoldier(x + 16, y + 24, Owner.ENEMY, "Evil Catgirl " + (1 + x + y * 3), .44f, x + y * 3, 2, new MeleeAI());
                CreateAndPlaceTestSoldier(x + 9, y + 4, Owner.PLAYER, "Good Catgirl " + (1 + x + y * 3), .2f, x + y * 3, 3, new MeleeAI());
                CreateAndPlaceTestSoldier(x + 12, y + 24, Owner.ENEMY, "Evil Catgirl " + (1 + x + y * 3), 0, x + y * 3, 3, new MeleeAI());
                //CreateAndPlaceTestSoldier(x + 12, y + 4, Owner.PLAYER, "Good Catgirl " + (1 + x + y * 3), 0, x + y * 3, 4, new BallistaAI());
                //CreateAndPlaceTestSoldier(x + 8, y + 24, Owner.ENEMY, "Evil Catgirl " + (1 + x + y * 3), .5f, x + y * 3, 4, new BallistaAI());
                CreateAndPlaceTestSoldier(x + 15, y + 4, Owner.PLAYER, "Good Catgirl " + (1 + x + y * 3), .2f, x + y * 3, 6, new MeleeAI());
                CreateAndPlaceTestSoldier(x + 4, y + 24, Owner.ENEMY, "Evil Catgirl " + (1 + x + y * 3), 0, x + y * 3, 6, new MeleeAI());
            }
            CreateAndPlaceTestSoldier(x + 18, 4, Owner.PLAYER, "Good Catgirl " + (1 + x), 0f, x, 5, new AvatarAI());
            CreateAndPlaceTestSoldier(x + 0, 24, Owner.ENEMY, "Evil Catgirl " + (1 + x), .5f, x, 5, new AvatarAI());
        }

    }

    /// <summary>
    /// Messy debug method to create a mock solder from thin air. Will never be called in the real game; soldiers would be placed
    /// According to the metagame progression.
    /// </summary>
    private void CreateAndPlaceTestSoldier(int x, int y, Owner owner, string name, float hueShift, float talentLevel, int job, SoldierAI ai)
    {
        Soldier test = GameObject.Instantiate<Soldier>(soldierPrefab);
        float talent = .1f;
        string charName = "";
        switch(talentLevel)
        {
            case 0:
                talent = 1.201f;
                charName = "Colleen Hunter";
                break;
            case 1:
                talent = 1.158f;
                charName = "Kiriko Nekoyama";
                break;
            case 2:
                talent = 1.121f;
                charName = "Rose Diamond";
                break;
            case 3:
                talent = 1.094f;
                charName = "Vicki Tonkinese";
                break;
            case 4:
                talent = 1.073f;
                charName = "Misty Aurata";
                break;
            case 5:
                talent = 1.053f;
                charName = "Heather Iberia";
                break;
            case 6:
                talent = 1.035f;
                charName = "Morrigan Kella";
                break;
            case 7:
                talent = 1.017f;
                charName = "Joanna Lynx";
                break;
            case 8:
                talent = 1.00f;
                charName = "Chamomile Tigon";
                break;
        }
        test.Init(ai, new Vector2Int(x, y), new SoldierStats(ClassDatabase.Instance.classes[job], talent, charName), owner, manager);
        test.InitGraphics(hueShift);
        test.name = name;
        manager.AddSoldierToBattle(test);
    }
}

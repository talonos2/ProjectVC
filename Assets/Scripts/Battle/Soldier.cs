using PathFind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This represents the non-persistent data of a soldier on the battle screen. It only applies
/// in a battle, and is not saved between battles. For attributes that persist across multiple battles,
/// see the SoldierStats class.
/// </summary>
public class Soldier : MonoBehaviour
{
    //Connections to the world
    private ChessGrid grid;
    private Vector2Int position;
    private AutoChessManager manager;

    //Graphics
    private Renderer soldierRenderer;
    private Material soldierMaterial;
    private Material hpBarMat;

    //AI and state
    private SoldierAI ai;
    private AIAction currentAction;
    private bool isDead;
    private int cachedHP;
    private int cachedMP;

    //For viewing or accessing
    public Owner owner;
    public SoldierStats stats;
    public Hitsplat hitsplatTemplate;
    public int currentHP;
    public int currentMP;

    //For actually setting in the inspector

    /// <summary>
    /// The HP bar to change as this soldier takes damage.
    /// </summary>
    public Transform hpBar;

    /// <summary>
    /// The renderer to change color as this soldier takes damage.
    /// </summary>
    public MeshRenderer hpBarRenderer;

    /// <summary>
    /// What projectile this soldier fires.
    /// </summary>
    public RangedProjectile basicRangedProjectile;

    //Encapsulation properties

    /// <summary>
    /// Where this soldier is located.
    /// </summary>
    public Vector2Int Position { get => position;}

    /// <summary>
    /// Whether this soldier is dead.
    /// </summary>
    public bool IsDead { get => isDead;}

    internal float GetModifiedSpeed()
    {
        return stats.Spd / manager.speedNormalizationDivisor;
    }

    internal bool CanMoveInDirection(Direction direction)
    {
        return (grid.nodes[position.x + direction.Vector().x, position.y + direction.Vector().y].Walkable);
    }

    internal void WaitABit()
    {
        currentAction = new WaitAction(UnityEngine.Random.value, this);
    }

    /// <summary>
    /// Moves the soldier.
    /// </summary>
    /// <param name="offset">How much the soldier moves by. Often a cardinal unit vector, but doesn't need to be.</param>
    internal void MovePositionBy(Vector2Int offset)
    {
        if (grid.nodes[position.x+offset.x, position.y+offset.y].occupiedBy != null)
        {
            Debug.LogError("Tried to move into an already occupied position!");
            return;
        }
        if (!grid.nodes[position.x, position.y].occupiedBy == this)
        {
            //TODO: for some reason, this triggers about one time in ten on the first move any soldier makes. Then it fixes itself. :/
            Debug.LogError("Tried to move from a position that was not occupied by me!");
            return;
        }
        grid.nodes[position.x, position.y].occupiedBy = null;
        grid.nodes[position.x + offset.x, position.y + offset.y].occupiedBy = this;
        position = position + offset;
    }

    internal void CreateHitsplat(HitsplatInfo info)
    {
        Hitsplat hitsplat = GameObject.Instantiate<Hitsplat>(hitsplatTemplate);
        hitsplat.Init(info, this);
    }

    internal float GetCurrentPower()
    {
        return stats.GetTotalPower() * currentHP / stats.Hp;
    }

    internal void InitPosition(Vector2Int newPosition)
    {
        if (grid.nodes[position.x, position.y].occupiedBy != null)
        {
            Debug.LogError("Tried to init into an already occupied position! ("+newPosition+")");
            return;
        }
        grid.nodes[newPosition.x, newPosition.y].occupiedBy = this;
        position = newPosition;
    }

    void Start()
    {
        hpBarRenderer.material = new Material(hpBarRenderer.material);
        hpBarMat = hpBarRenderer.material;
        currentHP = stats.Hp;
        cachedHP = currentHP;
        currentMP = stats.Mp;
        cachedMP = currentMP;
    }

    void Update()
    {
        if (IsDead)
        {
            return;
        }
        if (currentAction == null || currentAction.IsComplete())
        {
            currentAction = ai.GetNextAction();
        }
        else
        {
            currentAction.DoUpdate();
        }

        if (cachedHP != currentHP)
        {
            float oldHpRatio = (float)cachedHP / (float)stats.Hp;
            float hpRatio = (float)currentHP / (float)stats.Hp;
            PainStateRedifier redifier = this.gameObject.AddComponent<PainStateRedifier>();
            redifier.Init(oldHpRatio-hpRatio, soldierMaterial);
            cachedHP = currentHP;
            if (hpRatio <= 0)
            {
                hpRatio = 0;
                currentHP = 0;
                isDead = true;
                SoldierDeadifier deadifier = this.gameObject.AddComponent<SoldierDeadifier>();
                deadifier.Init(soldierMaterial);
                manager.RemoveSoldierFromBattle(this);
                grid.nodes[position.x, position.y].occupiedBy = null;
            }
            hpBar.localScale = new Vector3(.1f, hpRatio / 2, .1f);
            hpBar.localPosition = new Vector3(hpRatio / 2-.5f, -.5f, 0);
            hpBarMat.SetFloat("_hppercent", hpRatio);
        }
    }

    /// <summary>
    /// Handles animating the Soldier as it moves from one square to another.
    /// </summary>
    /// <param name="direction">The direction you're moving in.</param>
    /// <param name="t">How far through the looping process you are.</param>
    internal void PlaceAtLerpedPosition(Direction direction, float t)
    {
        this.transform.localPosition = Vector3.Lerp(grid.GridToWorldSpace(position), grid.GridToWorldSpace(position+direction.Vector()), t);
        Debug.DrawLine(grid.GridToWorldSpace(position), grid.GridToWorldSpace(position)+Vector3.up);
        this.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    /// <summary>
    /// Sets up the soldier. A constructor, basically.
    /// </summary>
    /// <param name="ai">What AI this soldier uses.</param>
    /// <param name="position">Where this soldier begins.</param>
    /// <param name="stats">The SoldierStats of this Soldier. (The persistent data.)</param>
    /// <param name="owner">The team that owns this soldier.</param>
    /// <param name="manager">A pointer to the scene's AutoChessManager.</param>
    public void Init(SoldierAI ai, Vector2Int position, SoldierStats stats, Owner owner, AutoChessManager manager)
    {
        this.stats = stats;
        this.manager = manager;
        this.grid = manager.grid;
        this.ai = ai;
        ai.SetControlledPawn(this);
        ai.SetStats(stats);
        ai.SetGrid(grid);
        ai.SetManager(manager);
        this.InitPosition(position);
        this.owner = owner;
        this.PlaceAtPosition();
    }

    private void PlaceAtPosition()
    {
        this.transform.localPosition = grid.GridToWorldSpace(position);
        this.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    /// <summary>
    /// Sets the portrait for this Soldier. TODO: We can entirely get rid of this and merge it with Init if the friend/enemy hue 
    /// shifts floats are properties of the SoldierClassPrototype class.
    /// </summary>
    /// <param name="hue"></param>
    public void InitGraphics(float hue)
    {
        this.soldierRenderer = this.GetComponent<Renderer>();
        soldierRenderer.material = new Material(soldierRenderer.material);
        soldierMaterial = soldierRenderer.material;
        soldierMaterial.SetFloat("_hue", hue);
        soldierMaterial.SetTexture("_face", stats.GetPrototype().image);
        soldierMaterial.SetTexture("_swap", stats.GetPrototype().swap);
    }
}

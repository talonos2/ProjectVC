using System;
using UnityEngine;

/// <summary>
/// A component that goes on ranged projectiles. Handles movement and damage and destroying itself upon impact.
/// TODO: This is a good candidate for an object pool if we ever see performance issues.
/// </summary>
public class RangedProjectile : MonoBehaviour
{
    float speed;
    private HitsplatInfo info;
    private int damage;
    Vector3 start;
    Soldier target;
    float timeSoFar;
    float totalTime;
    float initialYVel;
    float gravity;

    void Update()
    {
        if (target == null)
        {
            GameObject.Destroy(this.gameObject);
        }
        timeSoFar += Time.deltaTime;
        float t = timeSoFar / totalTime;
        if (t > 1)
        {
            Impact();
        }

        //A parametric equation to figure out the position of the projectile based on how much time has passed.
        //I swear this was not an attempt to meet the qualifications on your resume. I had just *literally* coded
        //this two days before I saw your job application. If you look at the rest of the program, you can see
        //that yeah, this code is just an incidental part of a WAY larger program.
        Vector3 oldPosit = this.transform.position;
        this.transform.position = Vector3.Lerp(start, target.transform.position, t);
        this.transform.position += new Vector3(0, .5f * gravity * timeSoFar * timeSoFar + initialYVel * timeSoFar);
        Vector3 direction = this.transform.position - oldPosit;
        this.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    /// <summary>
    /// Initializes all the properties of this component.
    /// </summary>
    /// <param name="speed">The speed of the projectile.</param>
    /// <param name="start">The start location of this projectile.</param>
    /// <param name="target">The target of this projectile. (Arrows home onto their targets like in most RTS games.</param>
    /// <param name="info">Info to produce a hitsplat on impact.</param>
    /// <param name="damage">The damage this projectile deals.</param>
    /// <param name="gravMult">A modifier on how much gravity should affect this projectile.</param>
    public void Init(float speed, Vector3 start, Soldier target, HitsplatInfo info, int damage, float gravMult = 1)
    {
        this.info = info;
        this.damage = damage;
        this.start = start;
        this.target = target;
        this.speed = speed;
        this.gravity = Physics.gravity.y * gravMult;
        this.totalTime = Vector3.Distance(start, target.transform.position)/speed;
        initialYVel = gravity * -.5f * totalTime;
    }

    private void Impact()
    {
        target.currentHP -= damage;
        target.CreateHitsplat(info);
        GameObject.Destroy(this.gameObject);
    }
}
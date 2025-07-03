using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SpellEffect", menuName = "Scriptable Objects/SpellEffect")]
public abstract class SpellEffect : ScriptableObject
{
    protected int duration;

    protected int[] targets;

    protected int caster;

    protected GameManager gameManager;

    public void InitializeSpell(int newCaster, int[] newTargets)
    {
        targets = new int[newTargets.Length];
        Array.Copy(newTargets, targets, newTargets.Length);
        this.caster = caster;
        this.gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public abstract void OnHit(SpellEffect spell);

    public abstract void OnTurn(SpellEffect spell);

    public abstract void OnCast(SpellEffect spell);

    public void BlockSpell()
    {
        this.duration = 0;
    }

    public bool HasEnded()
    {
        return this.duration <= 0;
    }
}

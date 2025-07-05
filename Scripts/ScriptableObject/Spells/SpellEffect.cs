using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SpellEffect", menuName = "Scriptable Objects/SpellEffect")]
public abstract class SpellEffect : ScriptableObject
{
    protected int duration;         // duration determining spell logic

    protected int[] targets;

    protected int caster;

    protected SpellManager spellManager;

    protected int targetsNumber;

    public void InitializeSpell(int newCaster, int[] newTargets, SpellManager spellManager)
    {
        this.caster = newCaster;
        targets = new int[newTargets.Length];
        Array.Copy(newTargets, targets, newTargets.Length);
        this.spellManager = spellManager;
    }

    public abstract void OnHit(SpellEffect spell);

    public abstract void OnTurn(SpellEffect spell);

    public abstract void OnCast(SpellEffect spell);

    public abstract void Effect(SpellEffect spell, int index);

    public void EndSpell()
    {
        this.duration = 0;
    }

    public bool HasEnded()
    {
        return this.duration <= 0;
    }

    public int GetTargetsNumber()
    {
        return this.targetsNumber;
    }
}

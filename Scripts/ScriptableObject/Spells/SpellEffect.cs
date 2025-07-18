using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SpellEffect", menuName = "Scriptable Objects/SpellEffect")]
public abstract class SpellEffect : ScriptableObject
{
    protected int duration;         // duration of spell in terms of turns

    protected int spellEffectsCount;    // how many times spell effect can be triggered

    protected int[] targets;

    protected int caster;

    protected SpellManager spellManager;

    protected int targetsNumber;

    protected int spellType;        // 0 - spells that defend, 1 - special ability spells, 2 - spells that gives card

    protected bool SelfCasted;

    public virtual void InitializeSpell(int newCaster, int newTarget, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.targets = new int[newTargets.Length];
        targets[0] = newTarget;
        this.spellManager = spellManager;
    }

    public virtual void OnHit(SpellEffect spell);

    public virtual void OnTurn();

    public virtual void OnCast();

    public abstract void Effect(SpellEffect spell, int target, int caster);

    public void EndSpell()
    {
        this.duration = 0;
        this.spellEffectsCount = 0;
    }

    public bool HasEnded()
    {
        return (this.duration <= 0) || (this.spellEffectsCount <= 0);
    }

    public int GetTargetsNumber()
    {
        return this.targetsNumber;
    }

    public int GetCasterIndex()
    {
        return this.caster;
    }

    public int[] GetTargetsIndexes()
    {
        return this.targets;
    }

    public int GetSpellType()
    {
        return this.spellType;
    }

    public bool IsSelfCasted()
    {
        return this.SelfCasted;
    }

    public void Block(int index)
    {
        for (int i; i < targetsNumber; i++)
        {
            if (targets[i] == index)
            {
                this.targets[i] = -1;
            }
        }
    }
}

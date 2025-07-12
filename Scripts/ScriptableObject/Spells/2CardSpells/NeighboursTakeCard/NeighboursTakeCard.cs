using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NeighboursTakeCard", menuName = "Scriptable Objects/NeighboursTakeCard")]
public class NeighboursTakeCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 0;
        this.spellType = 2;
        this.spellEffectsCount = 1;
    }

    public override void InitializeSpell(int newCaster, int[] newTargets, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        this.GetNeighbours();
    }

    void GetNeighbours()
    {
        int[] targets = new int[this.spellManager.GetNeighbours(caster).Length];
        Array.Copy(this.spellManager.GetNeighbours(caster), targets, this.spellManager.GetNeighbours(caster).Length);
    }

    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn() { }

    public override void OnCast()
    {
        if (!this.HasEnded())
        {
            foreach (int index in targets)
            {
                this.Effect(null, index, caster);
            }
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.GiveCardToPlayer(target);
    }
}

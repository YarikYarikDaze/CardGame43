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
        this.targets = new int[2];
        Array.Copy(this.spellManager.GetNeighbours(caster), this.targets, 2);
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

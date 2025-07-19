using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NeighboursTakeCard", menuName = "Scriptable Objects/NeighboursTakeCard")]
public class NeighboursTakeCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 2;
        this.spellType = 2;
        this.spellEffectsCount = 1;
        this.SelfCasted = true;
    }

    public override void InitializeSpell(int newCaster, int target, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        this.GetNeighbours();
    }

    void GetNeighbours()
    {
        this.targets = new int[2];
        Array.Copy(this.spellManager.GetNeighbours(caster), this.targets, this.targetsNumber);
    }

    public override void OnCast()
    {
        if (!HasEnded())
        {
            foreach (int index in targets)
            {
                if (index != -1)
                {
                    this.Effect(null, index, caster);
                }
            }
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.GiveCardToPlayer(target);
    }
}

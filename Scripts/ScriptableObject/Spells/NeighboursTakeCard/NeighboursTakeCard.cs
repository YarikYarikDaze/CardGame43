using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NeighboursTakeCard", menuName = "Scriptable Objects/NeighboursTakeCard")]
public class NeighboursTakeCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 0;
    }

    void GetNeighbours()
    {
        int[] targets = new int[this.spellManager.GetNeighbours(caster).Length];
        Array.Copy(this.spellManager.GetNeighbours(caster), targets, this.spellManager.GetNeighbours(caster).Length);
    }

    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn(SpellEffect spell) { }

    public override void OnCast(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            this.GetNeighbours();
            foreach (int index in targets)
            {
                this.Effect(spell, index);
            }
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int index)
    {
        this.spellManager.GiveCardToPlayer(index);
    }
}

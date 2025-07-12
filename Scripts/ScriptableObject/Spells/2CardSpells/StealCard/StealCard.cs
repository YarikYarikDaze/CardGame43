using UnityEngine;

[CreateAssetMenu(fileName = "StealCard", menuName = "Scriptable Objects/StealCard")]
public class StealCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 1;
        this.spellEffectsCount = 1;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn() { }

    public override void OnCast()
    { 
        if (!HasEnded())
        {
            this.Effect(null, targets[0], caster);
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.StealCard(caster, target);
    }
}

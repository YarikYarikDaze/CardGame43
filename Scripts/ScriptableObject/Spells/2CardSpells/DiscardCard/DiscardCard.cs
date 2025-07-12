using UnityEngine;

[CreateAssetMenu(fileName = "DiscardCard", menuName = "Scriptable Objects/DiscardCard")]
public class DiscardCard : SpellEffect
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
        this.spellManager.DiscardCard(target);
    }
}

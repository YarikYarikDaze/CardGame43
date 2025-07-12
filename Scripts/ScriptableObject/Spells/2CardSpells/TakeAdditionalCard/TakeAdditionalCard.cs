using UnityEngine;

[CreateAssetMenu(fileName = "TakeAdditionalCard", menuName = "Scriptable Objects/TakeAdditionalCard")]
public class TakeAdditionalCard : SpellEffect
{
    void Awake()
    {
        this.duration = 2;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 2;
    }
    public override void OnHit(SpellEffect spell)
    { 
        if(!this.HasEnded() && spell.GetSpellType()==2) {
            this.Effect(null, targets[0], caster);
            this.spellEffectsCount--;
        }
    }

    public override void OnTurn()
    {
        this.duration--;
    }

    public override void OnCast() { }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.GiveCardToPlayer(target);
    }
}

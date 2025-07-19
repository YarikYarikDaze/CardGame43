using UnityEngine;

[CreateAssetMenu(fileName = "SuperTakeAdditionalCard", menuName = "Scriptable Objects/SuperTakeAdditionalCard")]
public class SuperTakeAdditionalCard : SpellEffect
{
    void Awake()
    {
        this.duration = 2;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 2;
        this.SelfCasted = false;
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
        this.spellManager.GiveCardToPlayer(target);
    }
}

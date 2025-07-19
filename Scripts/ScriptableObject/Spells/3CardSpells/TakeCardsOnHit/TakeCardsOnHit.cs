using UnityEngine;

[CreateAssetMenu(fileName = "TakeCardsOnHit", menuName = "Scriptable Objects/TakeCardsOnHit")]
public class TakeCardsOnHit : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
    }
    public override void OnHit(SpellEffect spell)
    { 
        if(!this.HasEnded()) {
            this.Effect(null, targets[0], caster);
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

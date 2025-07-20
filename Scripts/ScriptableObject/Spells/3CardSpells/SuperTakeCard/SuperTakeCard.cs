using UnityEngine;

[CreateAssetMenu(fileName = "SuperTakeCard", menuName = "Scriptable Objects/SuperTakeCard")]
public class SuperTakeCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn() { }

    public override void OnCast()
    {
        if (!this.HasEnded())
        {
            this.Effect(null, targets[0], caster);
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.GiveCardToPlayer(target);
        this.spellManager.GiveCardToPlayer(target);
        SendIdToClients();
    }
}

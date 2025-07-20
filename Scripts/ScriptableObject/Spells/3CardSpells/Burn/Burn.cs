using UnityEngine;

[CreateAssetMenu(fileName = "Burn", menuName = "Scriptable Objects/Burn")]
public class Burn : SpellEffect
{
    void Awake()
    {
        this.duration = 4;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 3;
        this.SelfCasted = false;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn()
    {
        if (!this.HasEnded() && duration % 2 == 1)
        {
            this.Effect(null, targets[0], caster);
            this.duration--;
            this.spellEffectsCount--;
        }
    }

    public override void OnCast()
    {
        if (!this.HasEnded())
        {
            this.Effect(null, targets[0], caster);
            this.spellEffectsCount--;
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.GiveCardToPlayer(target);
        SendIdToClients();
    }
}

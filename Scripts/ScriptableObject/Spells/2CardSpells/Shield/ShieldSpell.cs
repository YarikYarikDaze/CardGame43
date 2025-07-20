using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSpell", menuName = "Scriptable Objects/ShieldSpell")]
public class ShieldSpell : SpellEffect
{
    void Awake()
    {
        this.duration = 2;
        this.targetsNumber = 1;
        this.spellType = 0;
        this.spellEffectsCount = 2;
        this.SelfCasted = true;
    }
    public override void OnHit(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            this.Effect(spell, targets[0], caster);
            this.spellEffectsCount--;
        }
    }

    public override void OnTurn()
    {
        this.duration--;
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        spell.Block(target);
        SendIdToClients();
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Reflect", menuName = "Scriptable Objects/Reflect")]
public class Reflect : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 0;
        this.spellEffectsCount = 1;
        this.SelfCasted = true;
        this.spellIndex = 6;
    }
    public override void OnHit(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            this.Effect(spell, spell.GetCasterIndex(), caster);
        }
    }

    public override void OnTurn()
    {
        this.EndSpell();
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        spell.Effect(this, target, caster);
        SendIdToClients();
    }
}

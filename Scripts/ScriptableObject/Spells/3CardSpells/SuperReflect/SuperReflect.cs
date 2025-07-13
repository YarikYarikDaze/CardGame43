using UnityEngine;

[CreateAssetMenu(fileName = "SuperReflect", menuName = "Scriptable Objects/SuperReflect")]
public class SuperReflect : SpellEffect
{
    void Awake()
    {
        this.duration = 2;
        this.targetsNumber = 0;
        this.spellType = 0;
        this.spellEffectsCount = 1;
    }
    public override void OnHit(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            this.Effect(spell, spell.GetCasterIndex(), caster);
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
        spell.Effect(this, target, caster);
        spell.EndSpell();
    }
}

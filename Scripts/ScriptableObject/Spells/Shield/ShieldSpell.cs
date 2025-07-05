using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSpell", menuName = "Scriptable Objects/ShieldSpell")]
public class ShieldSpell : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 0;
    }
    public override void OnHit(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            this.Effect(spell, caster);
            this.duration--;
        }
    }

    public override void OnTurn(SpellEffect spell)
    {
        this.EndSpell();
    }

    public override void OnCast(SpellEffect spell) { }

    public override void Effect(SpellEffect spell, int index)
    {
        spell.EndSpell();
    }
}

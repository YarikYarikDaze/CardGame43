using UnityEngine;

[CreateAssetMenu(fileName = "ClearEffects", menuName = "Scriptable Objects/ClearEffects")]
public class ClearEffects : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn(SpellEffect spell) { }

    public override void OnCast(SpellEffect spell)
    {
        this.Effect(spell, caster);
        this.EndSpell();
    }

    public override void Effect(SpellEffect spell, int index)
    {
        this.spellManager.ClearPlayerEffects(index);
    }
}

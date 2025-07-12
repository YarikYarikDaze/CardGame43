using UnityEngine;

[CreateAssetMenu(fileName = "ClearEffects", menuName = "Scriptable Objects/ClearEffects")]
public class ClearEffects : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 0;
        this.spellType = 1;
        this.spellEffectsCount = 1;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn() { }

    public override void OnCast()
    {
        this.Effect(null, targets[0], caster);
        this.EndSpell();
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.ClearPlayerEffects(target);
    }
}

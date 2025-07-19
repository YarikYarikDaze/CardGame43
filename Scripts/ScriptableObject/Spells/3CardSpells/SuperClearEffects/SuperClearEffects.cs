using UnityEngine;

[CreateAssetMenu(fileName = "SuperClearEffects", menuName = "Scriptable Objects/SuperClearEffects")]
public class SuperClearEffects : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 1;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
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

using UnityEngine;

[CreateAssetMenu(fileName = "ClearEffects", menuName = "Scriptable Objects/ClearEffects")]
public class ClearEffects : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 1;
        this.spellEffectsCount = 1;
        this.SelfCasted = true;
    }

    public override void OnCast()
    {
        this.Effect(null, targets[0], caster);
        this.EndSpell();
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.ClearPlayerEffects(target);
        SendIdToClients();
    }
}

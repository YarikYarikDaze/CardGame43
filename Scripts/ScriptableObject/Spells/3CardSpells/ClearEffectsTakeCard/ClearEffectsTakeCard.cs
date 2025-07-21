using UnityEngine;

[CreateAssetMenu(fileName = "ClearEffectsTakeCard", menuName = "Scriptable Objects/ClearEffectsTakeCard")]
public class ClearEffectsTakeCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
        this.spellIndex = 17;
    }

    public override void OnCast()
    {
        this.Effect(null, targets[0], caster);
        this.EndSpell();
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.ClearPlayerEffects(target);
        this.spellManager.GiveCardToPlayer(target);
        SendIdToClients();
    }
}

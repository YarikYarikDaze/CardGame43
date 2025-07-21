using UnityEngine;

[CreateAssetMenu(fileName = "SuperBlock", menuName = "Scriptable Objects/SuperBlock")]
public class SuperBlock : SpellEffect
{
    void Awake()
    {
        this.duration = 4;
        this.targetsNumber = 1;
        this.spellType = 0;
        this.spellEffectsCount = 3;
        this.SelfCasted = true;
        this.spellIndex = 35;
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

    public override void OnCast() { }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        spell.EndSpell();
        SendIdToClients();
    }
}

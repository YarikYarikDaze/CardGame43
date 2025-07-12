using UnityEngine;

[CreateAssetMenu(fileName = "TakeCard", menuName = "Scriptable Objects/TakeCard")]
public class TakeCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 1;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn() { }

    public override void OnCast()
    {
        if (!this.HasEnded())
        {
            this.Effect(null, targets[0], caster);
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.GiveCardToPlayer(target);
    }
}

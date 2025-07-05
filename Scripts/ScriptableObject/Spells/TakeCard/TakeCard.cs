using UnityEngine;

[CreateAssetMenu(fileName = "TakeCard", menuName = "Scriptable Objects/TakeCard")]
public class TakeCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn(SpellEffect spell) { }

    public override void OnCast(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            this.Effect(spell, targets[0]);
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int index)
    {
        this.spellManager.GiveCardToPlayer(index);
    }
}

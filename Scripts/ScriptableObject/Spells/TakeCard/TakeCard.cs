using UnityEngine;

[CreateAssetMenu(fileName = "TakeCard", menuName = "Scriptable Objects/TakeCard")]
public class TakeCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn(SpellEffect spell) { }

    public override void OnCast(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            this.spellManager.GiveCardToPlayer(targets[0]);
            this.duration = 0;
        }
    }
}

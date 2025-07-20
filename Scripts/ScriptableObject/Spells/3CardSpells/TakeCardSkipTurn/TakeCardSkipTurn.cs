using UnityEngine;

[CreateAssetMenu(fileName = "TakeCardSkipTurn", menuName = "Scriptable Objects/TakeCardSkipTurn")]
public class TakeCardSkipTurn : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
    }

    public override void OnTurn()
    {
        if (!this.HasEnded())
        {
            foreach (int index in targets)
            {
                if (index != -1)
                {
                    this.Effect(null, index, caster);
                }
            }
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.GiveCardToPlayer(target);
        this.spellManager.EndPlayerTurn(target);
        SendIdToClients();
    }
}

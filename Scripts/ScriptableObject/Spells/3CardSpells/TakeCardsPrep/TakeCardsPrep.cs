using UnityEngine;

[CreateAssetMenu(fileName = "TakeCardsPrep", menuName = "Scriptable Objects/TakeCardsPrep")]
public class TakeCardsPrep : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 1;
        this.SelfCasted = true;
    }

    public override void InitializeSpell(int newCaster, int target, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        this.GetRandomPlayer();
    }

    void GetRandomPlayer()
    {
        this.targets = new int[1];
        this.targets[0] = spellManager.GetRandomPlayer();
    }
    public override void OnHit(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            this.Effect(null, targets[0], caster);
            this.EndSpell();
        }
    }

    public override void OnTurn()
    {
    }

    public override void OnCast() { }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.GiveCardsPrep(target);
        SendIdToClients();
    }
}

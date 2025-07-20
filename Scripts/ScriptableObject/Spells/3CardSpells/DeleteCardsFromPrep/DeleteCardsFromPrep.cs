using UnityEngine;

[CreateAssetMenu(fileName = "DeleteCardsFromPrep", menuName = "Scriptable Objects/DeleteCardsFromPrep")]
public class DeleteCardsFromPrep : SpellEffect
{

    int InitialTarget;
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 6;
        this.spellType = 2;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
    }
    public override void InitializeSpell(int newCaster, int target, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        InitialTarget = target;
        GetAllPlayers(target);
    }

    void GetAllPlayers()
    {
        this.targets = new int[spellManager.GetAllPlayers().Length];
        Array.Copy(spellManager.GetAllPlayers(), targets, spellManager.GetAllPlayers().Length);
        this.targetsNumber = targets.Length;
    }
    public override void OnCast()
    {
        if (!HasEnded())
        {
            foreach (int index in targets)
            {
                if (index != -1)
                {
                    if (index == InitialTarget)
                    {
                        this.Effect(null, index, caster);
                    }
                    else
                    {
                        this.AdditionalEffect(index);
                    }
                }
            }
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        spellManager.ReturnCardsToHand(target, 1);
    }

    void AdditionalEffect(int target) {
        this.spellManager.DiscardCard(target);
    }
}

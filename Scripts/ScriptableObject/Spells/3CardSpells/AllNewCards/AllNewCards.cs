using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "AllNewCards", menuName = "Scriptable Objects/AllNewCards")]
public class AllNewCards : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 6;
        this.spellType = 1;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
    }
    public override void InitializeSpell(int newCaster, int target, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        GetAllPlayers();
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
                    this.Effect(null, index, caster);
                }
            }
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        spellManager.RenewCardsInHands(target);
        SendIdToClients();
    }
}

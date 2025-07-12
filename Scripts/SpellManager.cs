using UnityEngine;
using System;
using System.Collections.Generic;
public class SpellManager : MonoBehaviour
{
    GameManager gameManager;

    string[,,] effectsArray;

    public void InitializeItself(GameManager newGameManager)
    {
        this.gameManager = newGameManager;
        this.InitializeEffects();
    }

    void InitializeEffects()
    {
        effectsArray = new string[3, 3, 4]
        {
            {
                {
                "TakeCard",
                "NULL",
                "NULL",
                "NULL"
                },
                {
                "NeighboursTakeCard",
                "NULL",
                "NULL",
                "NULL"
                },
                {
                "TakeAdditionalCard",
                "NULL",
                "NULL",
                "NULL"
                }
            },
            {
                {
                "DiscardCard",
                "NULL",
                "NULL",
                "NULL"
                },
                {
                "SkipTurn",
                "NULL",
                "NULL",
                "NULL"
                },
                {
                "StealCard",
                "NULL",
                "NULL",
                "NULL"
                }
            },
            {
                {
                "Reflect",
                "NULL",
                "NULL",
                "NULL"
                },
                {
                "ClearEffects",
                "NULL",
                "NULL",
                "NULL"
                },
                {
                "ShieldSpell",
                "NULL",
                "NULL",
                "NULL"
                }
            }
        };
    }

    //
    //SpellManager Logic
    //

    SpellEffect TraverseEffectsOnHit(SpellEffect newSpell, int index)
    {
        List<SpellEffect> playerEffects = new List<SpellEffect>(gameManager.GetEffectsOnPlayer(index));

        foreach (SpellEffect spell in playerEffects)
        {
            spell.OnHit(newSpell);
        }

        gameManager.SetEffectsOnPlayer(index, playerEffects);

        this.DeleteEndedSpells(index);

        return newSpell;
    }

    public void TraverseEffectsOnTurn(int index)
    {
        List<SpellEffect> playerEffects = new List<SpellEffect>(gameManager.GetEffectsOnPlayer(index));

        foreach (SpellEffect spell in playerEffects)
        {
            spell.OnTurn();
        }

        gameManager.SetEffectsOnPlayer(index, playerEffects);

        this.DeleteEndedSpells(index);
    }

    public void CreateSpell(int index, int[] playerCards)
    {
        SpellEffect newSpell = (SpellEffect)ScriptableObject.CreateInstance(this.effectsArray[playerCards[0] - 1, playerCards[1] - 1, playerCards[2]]);
        int[] targets = gameManager.GetTargets(index, newSpell.GetTargetsNumber());
        newSpell.InitializeSpell(index, targets, this);

        this.HandleNewSpell(newSpell, index, targets);
    }
    void HandleNewSpell(SpellEffect newSpell, int index, int[] targets)
    {
        foreach (int target in targets)
        {
            SpellEffect spellAfterHandling = this.TraverseEffectsOnHit(newSpell, target);

            spellAfterHandling.OnCast();

            gameManager.AddEffect(target, spellAfterHandling);

            this.DeleteEndedSpells(target);
        }
    }

    void DeleteEndedSpells(int index)
    {
        List<SpellEffect> endedSpells = new List<SpellEffect>();

        List<SpellEffect> spellsOnPlayer = new List<SpellEffect>(gameManager.GetEffectsOnPlayer(index));

        foreach (SpellEffect spell in spellsOnPlayer)
        {
            if (spell.HasEnded())
            {
                endedSpells.Add(spell);
            }
        }

        foreach (SpellEffect spell in endedSpells)
        {
            spellsOnPlayer.Remove(spell);
        }

        gameManager.SetEffectsOnPlayer(index, spellsOnPlayer);
    }

    //
    //Spells Logic
    //

    public void GiveCardToPlayer(int index)
    {
        gameManager.GiveCardToPlayer(index);
    }

    public void EndPlayerTurn(int index)
    {
        gameManager.ForceEndPlayerTurn();
    }

    public void ClearPlayerEffects(int index)
    {
        List<SpellEffect> newList = new List<SpellEffect>();
        this.gameManager.SetEffectsOnPlayer(index, newList);
    }

    public int[] GetNeighbours(int index)
    {
        int[] neighbours = new int[2] { gameManager.PreviousPlayer(index), gameManager.NextPlayer(index) };
        return neighbours;
    }

    public void StealCard(int indexCaster, int indexTarget)
    {
        int cardNumber = ChooseCard(indexTarget);
        int color = gameManager.RemoveCardFromPrep(indexTarget, cardNumber);
        gameManager.AddCardToPrep(indexCaster, 0, color);
    }

    public void DiscardCard(int indexTarget)
    {
        int cardNumber = ChooseCard(indexTarget);
        gameManager.RemoveCardFromPrep(indexTarget, cardNumber);
    }

    public int ChooseCard(int index)
    {
        int cardNumber = gameManager.ChooseCard(index);
        return cardNumber;
    }
}
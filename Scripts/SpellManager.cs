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
                "SuperTakeCard",
                "NULLSpell",
                "NULLSpell"
                },
                {
                "NeighboursTakeCard",
                "NULLSpell",
                "NULLSpell",
                "NULLSpell"
                },
                {
                "TakeAdditionalCard",
                "NULLSpell",
                "NULLSpell",
                "NULLSpell"
                }
            },
            {
                {
                "DiscardCard",
                "NULLSpell",
                "NULLSpell",
                "NULLSpell"
                },
                {
                "SkipTurn",
                "NULLSpell",
                "NULLSpell",
                "NULLSpell"
                },
                {
                "StealCard",
                "NULLSpell",
                "NULLSpell",
                "NULLSpell"
                }
            },
            {
                {
                "Reflect",
                "NULLSpell",
                "NULLSpell",
                "NULLSpell"
                },
                {
                "ClearEffects",
                "NULLSpell",
                "NULLSpell",
                "NULLSpell"
                },
                {
                "ShieldSpell",
                "NULLSpell",
                "NULLSpell",
                "SuperBlock"
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

    public void InstantiateSpell(int index, int[] playerCards)
    {
        SpellEffect newSpell = (SpellEffect)ScriptableObject.CreateInstance(this.effectsArray[playerCards[0] - 1, playerCards[1] - 1, playerCards[2]]);
        gameManager.GetTarget(index, newSpell);
    }

    public void InitializeSpell(SpellEffect spell, int caster, int target)
    {
        spell.InitializeSpell(caster, target, this);

        gameManager.ClearPrepOfAPlayer(caster);

        this.HandleNewSpell(spell, caster, spell.GetTargetsIndexes());
    }
    void HandleNewSpell(SpellEffect newSpell, int index, int[] targets)
    {
        Debug.Log(targets[0]);
        Debug.Log(targets[1]);
        foreach (int target in targets)
        {
            SpellEffect spellAfterHandling = this.TraverseEffectsOnHit(newSpell, target);

            spellAfterHandling.OnCast();

            gameManager.AddEffect(target, spellAfterHandling);

            this.DeleteEndedSpells(target);
        }
        gameManager.SpellCasted();
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
        if (cardNumber == -1)
        {
            return;
        }
        int color = gameManager.RemoveCardFromPrep(indexTarget, cardNumber);
        gameManager.AddCardToPrep(indexCaster, 0, color);
    }

    public void DiscardCard(int indexTarget)
    {
        int cardNumber = ChooseCard(indexTarget);
        if (cardNumber == -1)

        {
            GiveCardToPlayer(indexTarget);
            return;
        }
        gameManager.RemoveCardFromPrep(indexTarget, cardNumber);
    }

    public int ChooseCard(int index)
    {
        int cardNumber = gameManager.ChooseCard(index);
        return cardNumber;
    }

    public void GiveCardsPrep(int target)
    {
        int CardsCount = gameManager.GetCardsCountPrep(target);
        for (int i = 0; i < CardsCount; i++)
        {
            this.GiveCardToPlayer(target);
        }
    }

    public int GetRandomPlayer()
    {
        return gameManager.GetRandomPlayer();
    }

    public void ReturnSpellToPrep(int target)
    {
        gameManager.ReturnSpellToPrep(target);
    }

    public int[] GetTwoNextPlayers(int index, int caster)
    {
        int[] targets = new int[3];
        targets[0] = index;
        targets[1] = gameManager.NextPlayer(index);
        if (targets[1] == caster)
        {
            targets[1] = gameManager.NextPlayer(targets[1]);
        }
        targets[2] = gameManager.NextPlayer(targets[1]);
        if (targets[2] == caster)
        {
            targets[2] = gameManager.NextPlayer(targets[2]);
        }
        return targets;
    }

    public void CreateRandomSpell(int caster)
    {
        System.Random random = new System.Random();
        SpellEffect newSpell = (SpellEffect)ScriptableObject.CreateInstance(this.effectsArray[random.Next(3), random.Next(3), random.Next(4)]);

        if (newSpell.IsSelfCasted())
        {
            newSpell.InitializeSpell(caster, caster, this);
        }
        else
        {
            newSpell.InitializeSpell(caster, GetRandomPlayer(), this);
        }

        gameManager.ClearPrepOfAPlayer(caster);

        this.HandleNewSpell(newSpell, caster, newSpell.GetTargetsIndexes());
    }

    public int[] GetAllPlayers()
    {
        int[] targets = new int[gameManager.GetPlayerCount()];
        for (int i = 0; i < gameManager.GetPlayerCount(); i++)
        {
            targets[i] = i;
        }
        return targets;
    }

    public void ReturnCardsToHand(int target, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int cardNumber = ChooseCard(target);
            if (cardNumber == -1)
            {
                return;
            }
            int color = gameManager.RemoveCardFromPrep(target, cardNumber);
            gameManager.GiveSpecificCardToPlayer(target, color);
        }
    }

    public void RenewCardsInHands(int target)
    {
        gameManager.RenewCardsInHands(target);
    }
}
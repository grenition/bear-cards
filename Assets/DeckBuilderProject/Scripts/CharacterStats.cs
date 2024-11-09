using SinuousProductions;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Character characterStartData;

    public string cardName;
    public List<Card.ElementType> cardType;
    public int health;
    public int damageMin;
    public int damageMax;
    public List<Card.ElementType> damageType;
    public int range;
    public Card.AttackPattern attackPattern;
    public Card.PriorityTarget priorityTarget;

    private bool statsSet = false;

    void Update()
    {
        if (!statsSet && characterStartData != null)
        {
            SetStartStats();
        }
    }

    private void SetStartStats()
    {
        cardName = characterStartData.cardName;
        cardType = characterStartData.cardType;
        health = characterStartData.health;
        damageMin = characterStartData.damageMin;
        damageMax = characterStartData.damageMax;
        damageType = characterStartData.damageType;
        range = characterStartData.range;
        attackPattern = characterStartData.attackPattern;
        priorityTarget = characterStartData.priorityTarget;
        statsSet = true;
    }
}

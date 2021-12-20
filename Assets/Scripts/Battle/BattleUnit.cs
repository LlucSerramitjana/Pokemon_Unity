using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit; //To know if it is a player or an enemy

    public Pokemon pokemon { get; set; }
    public void Setup()
    {
        new Pokemon(_base, level);
        if (isPlayerUnit)
            GetComponent<Image>().sprite = pokemon.Base.BackSprite;
        else
            GetComponent<Image>().sprite = pokemon.Base.FrontSprite;
    }
}

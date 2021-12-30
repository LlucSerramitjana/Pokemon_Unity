using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl" + pokemon.Level;
        _pokemon = pokemon;
        float calc = (float)pokemon.HP / pokemon.MaxHP;
        hpBar.SetHP(calc);
    }
}

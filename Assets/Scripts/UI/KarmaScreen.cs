
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KarmaScreen : MonoBehaviour
{
	public UILabel life_label;
	public UILabel life_increase_label;
	public UILabel shield_label;
	public UILabel shield_increase_label;
	public UILabel dark_label;
	public UILabel dark_increase_label;
	public UILabel light_label;
	public UILabel light_increase_label;
	public UILabel rock_label;
	public UILabel rock_increase_label;
	public UILabel fire_label;
	public UILabel fire_increase_label;


	public UILabel total_karma_label;

	void Update()
	{
		life_label.text = "Life : " + Player.instance.current_life.ToString();
		life_increase_label.text = Player.instance.life_bonus_per_karma.ToString();

		shield_label.text = "Shield : " + Player.instance.GetEffectiveMaxShield().ToString();
		shield_increase_label.text = Player.instance.shield_bonus_per_karma.ToString();

		dark_label.text = "Dark : " + Player.instance.GetEffectiveElementAffinity(Element.DARK);
		dark_increase_label.text = Player.instance.element_affinity_bonus_per_karma.ToString();

		light_label.text = "Light : " + Player.instance.GetEffectiveElementAffinity(Element.LIGHT);
		light_increase_label.text = Player.instance.element_affinity_bonus_per_karma.ToString();

		rock_label.text = "Rock : " + Player.instance.GetEffectiveElementAffinity(Element.ROCK);
		rock_increase_label.text = Player.instance.element_affinity_bonus_per_karma.ToString();

		fire_label.text = "Fire : " + Player.instance.GetEffectiveElementAffinity(Element.FIRE);
		fire_increase_label.text = Player.instance.element_affinity_bonus_per_karma.ToString();

		total_karma_label.text = Player.instance.current_karma.ToString();


	}

	void KarmaLife()
	{
		Player.instance.UseKarmaForLife();
	}
	void KarmaShield()
	{
		Player.instance.UseKarmaForShield();
	}
	void KarmaDark()
	{
		Player.instance.UseKarmaForElementAffinity(Element.DARK);
	}
	void KarmaLight()
	{
		Player.instance.UseKarmaForElementAffinity(Element.LIGHT);
	}
	void KarmaRock()
	{
		Player.instance.UseKarmaForElementAffinity(Element.ROCK);
	}
	void KarmaFire()
	{
		Player.instance.UseKarmaForElementAffinity(Element.FIRE);
	}

}
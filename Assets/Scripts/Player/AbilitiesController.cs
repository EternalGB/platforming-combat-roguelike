using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilitiesController : MonoBehaviour
{

	public float[] abInput;
	public Ability[] actives;
	public Ability[] upgrades;
	public Ability[] passives;
	public List<Ability> allAbilities;
	public Transform channeller;
	bool inputAllowed = true;

	void Awake()
	{
		abInput = new float[4];

	}

	void Start()
	{
		if(allAbilities == null)
			allAbilities = new List<Ability>();
		for(int i = 0; i < actives.Length; i++)
			if(actives[i] != null)
				allAbilities.Add(actives[i]);
	}

	void Update()
	{
		if(inputAllowed) {
			abInput[0] = Input.GetAxisRaw("A1");
			abInput[1] = Input.GetAxisRaw("A2");
			abInput[2] = Input.GetAxisRaw("A3");
			abInput[3] = Input.GetAxisRaw("A4");
		}


		for(int i = 0; i < abInput.Length; i++) {
			if(abInput[i] > 0 && actives[i] != null) {
				actives[i].triggerActive(transform);
			}
		}
	}

	public void AddActive(Ability ab, int slot)
	{
		if(!allAbilities.Contains(ab))
			allAbilities.Add(ab);
		if(slot >= 0)
			SetActive(allAbilities.IndexOf(ab),slot);
	}

	public void AddUpgrade(Ability upg, int slot)
	{
		if(!allAbilities.Contains(upg))
			allAbilities.Add(upg);
		if(slot >= 0) {
			SetUpgrade(allAbilities.IndexOf(upg),slot);
		}
	}

	public void AddPassive(Ability pass, int slot)
	{
		if(!allAbilities.Contains(pass))
			allAbilities.Add(pass);
		if(slot >= 0) {
			SetPassive(allAbilities.IndexOf(pass),slot);
		}
	}

	public void RemoveUpgrade(int abIndex)
	{
		Ability active = actives[abIndex];
		Ability upg = upgrades[abIndex];
		if(active)
			active.resetAbility();
		if(upg)
			upg.resetAbility();
		upgrades[abIndex] = null;
	}

	public void RemovePassive(int passIndex)
	{
		Ability p;
		if(p = passives[passIndex]) {
			p.undoPassive(PlayerController.GlobalPlayerInstance.transform);
			passives[passIndex] = null;
		}
	}

	public void SetActive(int abIndex, int activeIndex)
	{
		if(actives[activeIndex] && upgrades[activeIndex]) {
			RemoveUpgrade(abIndex);
		}
		actives[activeIndex] = allAbilities[abIndex];
	}

	public void SetUpgrade(int abIndex, int upgIndex)
	{
		Ability active = actives[upgIndex];
		Ability upgrade = allAbilities[abIndex];
		if(active && upgrade) {
			if(upgrades[upgIndex])
				RemoveUpgrade(upgIndex);
			active.getUpgradedBy(upgrade);
			upgrades[upgIndex] = upgrade;
		}
	}

	public void SetPassive(int abIndex, int passIndex)
	{
		if(passives[passIndex])
			RemovePassive(passIndex);
		passives[passIndex] = allAbilities[abIndex];
		passives[passIndex].applyPassive(PlayerController.GlobalPlayerInstance.transform);
	}

	public bool IsActive(Ability ab)
	{
		foreach(Ability active in actives) {
			if(active != null && ab.abilityName == active.abilityName)
				return true;
		}
		return false;
	}

	public bool IsUpgrade(Ability ab)
	{
		foreach(Ability upg in upgrades) {
			if(upg != null && upg.abilityName == ab.abilityName) {
				return true;
			}
		}

		return false;
	}

	public bool IsPassive(Ability ab)
	{
		foreach(Ability pass in passives) {
			if(pass != null && pass.abilityName == ab.abilityName) {
				return true;
			}
		}
		return false;
	}

	public bool InUse(Ability ab)
	{
		return IsActive(ab) || IsUpgrade(ab) || IsPassive(ab);
	}

	public void ToggleInput()
	{
		inputAllowed = !inputAllowed;
		ClearInput();
	}

	void ClearInput()
	{
		for(int i = 0; i < abInput.Length; i++) {
			abInput[i] = 0;
		}
	}

}


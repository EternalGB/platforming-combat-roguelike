using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public abstract class Ability : MonoBehaviour
{

	public string abilityName;
	public TextAsset activeDescription;
	public TextAsset upgradeDescription;
	public TextAsset passiveDescription;
	
	public float cooldown;
	public bool canActivate = true;
	public float effectSize;
	public Sprite icon;
	public System.Action<Transform> activeFunc;

	public List<Improvement> improvements;

	private float origCooldown;
	private float origEffectSize;

	public void Start()
	{
		activeFunc = activeEffect;
		origCooldown = cooldown;
		origEffectSize = effectSize;
	}

	public void triggerActive(Transform player)
	{
		if(canActivate) {
			canActivate = false;
			activeFunc(player);
			StartCoroutine(Timers.Countdown(cooldown,enableFire));
		}
	}

	public void enableFire()
	{
		canActivate = true;
	}

	public void getUpgradedBy(Ability ab)
	{
		ab.upgradeOtherAbility(this);
	}

	public void resetAbility()
	{
		cooldown = origCooldown;
		effectSize = origEffectSize;
		activeFunc = activeEffect;
		reset();
	}

	public void improveAttribute(int improvementIndex)
	{
		Improvement imp = improvements[improvementIndex];
		if(imp.pointsAllocated < imp.maxPoints) {
			string fieldName = imp.fieldName;
			float amount = imp.pointValue;
			FieldInfo info = this.GetType().GetField(fieldName, BindingFlags.Public |
			                                         BindingFlags.NonPublic | BindingFlags.SetField |
			                                         BindingFlags.Instance);
			//we only improve numerical values
			if(info.GetValue(this).GetType() == typeof(float)) {
				float value = (float)info.GetValue(this);
				info.SetValue(this,value+amount);
			} else if(info.GetValue(this).GetType() == typeof(int)) {
				int value = (int)info.GetValue(this);
				print(abilityName + ": the value of " + info.Name + " is " + value);
				info.SetValue(this,(int)(value+amount));
				print(abilityName + ": setting " + info.Name + " to " + (int)(value+amount));
			}

			imp.pointsAllocated++;
		}

	}

	public abstract void activeEffect(Transform player);

	protected abstract void upgradeOtherAbility(Ability other);

	protected abstract void reset();

	public abstract void applyPassive(Transform player);

	public abstract void undoPassive(Transform player);



}


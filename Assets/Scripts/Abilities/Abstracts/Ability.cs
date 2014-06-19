using UnityEngine;
using System.Collections;

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
	public Ability upgrade;
	public System.Action<Transform> activeFunc;

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

	public void upgradeAbility(Ability ab)
	{
		upgrade = ab;
		ab.upgradeOtherAbility(this);
	}

	public void removeUpgrade()
	{
		upgrade = null;
		cooldown = origCooldown;
		effectSize = origEffectSize;
		activeFunc = activeEffect;
		reset();
	}
	

	public abstract void activeEffect(Transform player);

	protected abstract void upgradeOtherAbility(Ability other);

	protected abstract void reset();

	public abstract void passiveEffect(Transform player);

}


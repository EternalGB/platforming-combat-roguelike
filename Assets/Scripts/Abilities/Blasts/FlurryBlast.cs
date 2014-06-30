using UnityEngine;
using System.Collections;

public class FlurryBlast : CloseBlast
{

	public Bounds hitArea;

	override public void burstEffect(Transform blast, Transform target)
	{
		GameActor actor;
		if(actor = target.GetComponent<GameActor>()) {
			actor.Damage(effectSize);
		}
	}

	protected override void createBlast(Transform location, Transform player)
	{
		GameObject blast = blastPool.getPooled();
		blast.SetActive(true);
		blast.SendMessage ("StartDelay",blastDelay);
		blast.SendMessage("SetBlastEffect",new UpgradeAction(onHitByBurst,burstTargets));
		float dir = Mathf.Sign (player.localScale.x);
		blast.transform.position = location.position + new Vector3(dir*hitArea.center.x, hitArea.center.y) +
			new Vector3(dir*Random.Range (-hitArea.extents.x,hitArea.extents.x), Random.Range (-hitArea.extents.y,hitArea.extents.y));
		blast.transform.right = player.right*dir;
		if(attachedToFirer)
			blast.SendMessage("SetOwner",player.GetComponent<GameActor>());
	}

}


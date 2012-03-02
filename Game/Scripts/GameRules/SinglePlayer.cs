using CryEngine;
using CryGameCode.AngryBoids;

/// <summary>
/// The campaign game mode is the base game mode
/// </summary>
namespace CryGameCode
{
	[DefaultGamemodeAttribute]
	public class SinglePlayer : BaseGameRules
	{
		//This is called, contrary to what you'd expect, just once, as the player persists between test sessions in the editor (ctrl+g)
		public override void OnClientConnect(int channelId, bool isReset = false, string playerName = "")
		{
			GameRules.SpawnPlayer<CameraProxy>(channelId, "Player", new Vec3(0, 0, 0), new Vec3(0, 0, 0));
		}

		public override void OnRevive(EntityId actorId, Vec3 pos, Vec3 rot, int teamId)
		{
			Debug.LogAlways("Entering game:");

			var cameraProxy = GameRules.GetPlayer(actorId) as CameraProxy;

			if(cameraProxy == null)
			{
				Debug.LogAlways("[SinglePlayer.OnRevive] Failed to get the player proxy. Check the log for errors.");
				return;
			}

			Debug.LogAlways("Initialising camera proxy...");

			cameraProxy.Init();

			Debug.LogAlways("Camera proxy initialised!");
		}
	}
}
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
			var player = GameRules.SpawnPlayer<CameraProxy>(channelId, "Player", new Vec3(0, 0, 0), new Vec3(0, 0, 0));
			if(player == null)
				Debug.LogAlways("[SinglePlayer.OnClientConnect] Failed to spawn the player. Check the log for errors.");
		}

		public override void OnClientDisconnect(int channelId)
		{
			Actor.Remove(channelId);
		}

		public override void OnRevive(EntityId actorId, Vec3 pos, Vec3 rot, int teamId)
		{
			var cameraProxy = Actor.Get(actorId) as CameraProxy;

			if(cameraProxy == null)
			{
				Debug.Log("[SinglePlayer.OnRevive] Failed to get the player proxy. Check the log for errors.");
				return;
			}

			cameraProxy.Init();
		}
	}
}
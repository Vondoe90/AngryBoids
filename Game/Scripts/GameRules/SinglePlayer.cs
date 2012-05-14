using CryEngine;
using CryGameCode.AngryBoids;
using CryGameCode.Entities;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The campaign game mode is the base game mode
/// </summary>
namespace CryGameCode
{
	[GameRules(Default = true)]
	public class SinglePlayer : GameRules
	{
		//This is called, contrary to what you'd expect, just once, as the player persists between test sessions in the editor (ctrl+g)
		public override void OnClientConnect(int channelId, bool isReset = false, string playerName = "")
		{
			var player = Actor.Create<PlayerCamera>(channelId, "Player");

			var spawnPoints = Entity.GetEntities("SpawnPoint");
			if(spawnPoints != null && spawnPoints.Count() > 0)
			{
				var spawnPoint = spawnPoints.First();
				if(spawnPoint != null)
				{
					player.Position = spawnPoint.Position;
					player.Rotation = spawnPoint.Rotation;
				}
			}
		}

		public override void OnClientDisconnect(int channelId)
		{
			Actor.Remove(channelId);
		}

		public override void OnRevive(EntityId actorId, Vec3 pos, Vec3 rot, int teamId)
		{
			var cameraProxy = Actor.Get(actorId) as PlayerCamera;

			if(cameraProxy == null)
			{
				Debug.Log("[SinglePlayer.OnRevive] Failed to get the player proxy. Check the log for errors.");
				return;
			}

			cameraProxy.Init();
		}
	}
}
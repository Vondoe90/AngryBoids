using System.Threading.Tasks;
using CryEngine;
using CryEngine.Async;
using CryGameCode.AngryBoids;
using CryGameCode.Entities;

using System.Collections.Generic;
using System.Linq;
using System;
using CryGameCode.Entities.AngryBoids;

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
		}

		public override void OnClientDisconnect(int channelId)
		{
			Actor.Remove(channelId);
		}

		public override void OnClientEnteredGame(int channelId, EntityId playerId, bool reset, bool loadingSaveGame)
		{
			var actor = Actor.Get(playerId);

			OnRevive(playerId, actor.Position, (Vec3)actor.Rotation, 0);
		}

		public override void OnRevive(EntityId actorId, Vec3 pos, Vec3 rot, int teamId)
		{
			var cameraProxy = Actor.Get<PlayerCamera>(actorId);
			if(cameraProxy == null)
			{
				Debug.Log("[SinglePlayer.OnRevive] Failed to get the player proxy. Check the log for errors.");
				return;
			}

            Debug.Log("Current = " + Environment.Version.ToString());
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                Debug.Log("Loaded " + item.FullName);
            }

		    StartMovingTargets();

			cameraProxy.Init();
		}

        private async void StartMovingTargets()
        {
            var enemy1 = Entity.Find("Enemy1");
            var enemy2 = Entity.Find("Enemy2");
            var enemy3 = Entity.Find("Enemy3");

            var pos1 = enemy1.Position;
            var pos2 = enemy2.Position;
            var pos3 = enemy3.Position;

            await DoAnimationLoop(enemy1, enemy2, enemy3, pos1, pos2, pos3);
        }

        private async Task DoAnimationLoop(EntityBase enemy1, EntityBase enemy2, EntityBase enemy3, Vec3 pos1, Vec3 pos2, Vec3 pos3)
        {

            var speed = TimeSpan.FromMilliseconds(800);
            
            MoveTo(enemy1, pos2, speed);
            MoveTo(enemy2, pos3, speed);
            await MoveTo(enemy3, pos1, speed);

            MoveTo(enemy1, pos3, speed);
            MoveTo(enemy2, pos1, speed);
            await MoveTo(enemy3, pos2, speed);

            MoveTo(enemy1, pos1, speed);
            MoveTo(enemy2, pos2, speed);
            await MoveTo(enemy3, pos3, speed);

            await DoAnimationLoop(enemy1, enemy2, enemy3, pos1, pos2, pos3);
        }

        public Task MoveTo(EntityBase ent, Vec3 position, TimeSpan duration)
        {
            var job = new MoveToJob(ent, position, duration);
            job.Task.ConfigureAwait(false).GetAwaiter();
            Awaiter.Instance.Jobs.Add(job);
            return job.Task;
        }

	}
}
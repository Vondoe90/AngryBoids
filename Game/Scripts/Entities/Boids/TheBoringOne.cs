using System;
using System.Threading.Tasks;
using CryEngine;
using CryEngine.Async;

namespace CryGameCode.Entities.AngryBoids
{
	[Entity(Category = "AngryBoids")]
	public class TheBoringOne : AngryBoid
	{
		public TheBoringOne()
			: base()
		{
		}

        protected override void OnReset(bool enteringGame)
        {
            if (enteringGame)
                Test();
            base.OnReset(enteringGame);
        }

        public async void Test()
        {
            Debug.Log("Hello there");
            await Delay.TimeDelay(TimeSpan.FromSeconds(30));
            Debug.LogAlways("Are you there?");
            await Delay.TimeDelay(TimeSpan.FromSeconds(20));

            await LoopMode();
        }

	    public async Task LoopMode()
	    {
            var originalPosition = this.Position;
            //Debug.Log("Starting to move");
            //await MoveTo(new Vec3(900, 900, 100), TimeSpan.FromSeconds(1));
            //Debug.LogAlways("Arrived at destination, moving back");
            //await MoveTo(originalPosition, TimeSpan.FromSeconds(1));
            //await LoopMode();
	        
	    }

        public Task MoveTo(Vec3 position, TimeSpan duration)
        {
            var job = new MoveToJob(this, position, duration);
            job.Task.ConfigureAwait(false).GetAwaiter();
            Awaiter.Instance.Jobs.Add(job);
            return job.Task;
        }
	}

    public class MoveToJob : AsyncJob<bool>
    {

        // TODO: Refactor to use decent timing instead of DateTime

        public float DelayInMilliseconds { get; set; }

        private DateTime beginTime;
        private DateTime endTime;
        private EntityBase entity;
        private Vec3 beginPosition;
        private Vec3 endPosition;

        public MoveToJob(float milliseconds)
            : base()
        {
            DelayInMilliseconds = milliseconds;
            beginTime = DateTime.Now;

            if (milliseconds <= 0)
            {
                source.TrySetResult(false);
                IsFinished = true;
            }
        }

        public MoveToJob(EntityBase ent, Vec3 position, TimeSpan delay)
        {
            DelayInMilliseconds = (float)delay.TotalMilliseconds;
            beginTime = DateTime.Now;
            endTime = DateTime.Now + delay;
            beginPosition = ent.Position;
            endPosition = position;
            entity = ent;
        }

        public override bool Update(float frameTime)
        {
            var now = DateTime.Now;
            if (!IsFinished)
            {
                float percentageElapsed = System.Math.Min(((float)(now - beginTime).TotalMilliseconds )/DelayInMilliseconds, 1);

                var diff = endPosition - beginPosition;
                entity.Position = new Vec3(beginPosition.X + diff.X * percentageElapsed, beginPosition.Y + diff.Y * percentageElapsed, beginPosition.Z + diff.Z * percentageElapsed);
                if (now >= endTime)
                {
                    source.TrySetResult(true);
                    IsFinished = true;
                }
            }
            return IsFinished;
        }
    }
}
// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public class AbilityExecutionContext : IAbilityExecutionContext, IAbilityTaskContainer
{

	public AbilityExecutionContext(IAbilityScheduler scheduler, IAbilityDefinition definition, ActiveAbilityGuid guid = default)
	{
		Guid = guid.IsValid ? guid : scheduler.GenerateNextActiveAbilityGuid();
		Scheduler = scheduler;
		Definition = definition;
	}
	
	public void RegisterTask(IAbilityTask task)
	{
		if (_cleared)
		{
			return;
		}
		
		if (task.Owner is not null)
		{
			return;
		}

		if (!_activeTasks.Add(task))
		{
			return;
		}

		task.Owner = this;
		task.Activate();
	}

	public void CancelTask(IAbilityTask task)
	{
		if (_cleared)
		{
			return;
		}
		
		if (task.Owner != this)
		{
			return;
		}

		if (!_activeTasks.Contains(task))
		{
			return;
		}
		
		task.Cancel();
		_activeTasks.Remove(task);
		task.Owner = null;
	}

	void IAbilityTaskContainer.CompleteTask(IAbilityTask task)
	{
		if (_cleared)
		{
			return;
		}
		
		_activeTasks.Remove(task);
	}

	void IAbilityTaskContainer.Clear()
	{
		_cleared = true;
		foreach (var task in _activeTasks)
		{
			task.Cancel();
		}
		
		_activeTasks.Clear();
	}

	void IAbilityTaskContainer.Tick(float deltaSeconds)
	{
		_capturedTasks.Clear();
		foreach (var task in _activeTasks)
		{
			_capturedTasks.Add(task);
		}

		foreach (var task in _capturedTasks)
		{
			task.Tick(deltaSeconds);
			if (_cleared)
			{
				return;
			}
		}
	}
	
	public ActiveAbilityGuid Guid { get; }
	public IAbilityScheduler Scheduler { get; }
	public IAbilityDefinition Definition { get; }

	public EAbilityExecutionState ExecutionState { get; private set; }
	EAbilityExecutionState IAbilityExecutionContext.ExecutionState { get => ExecutionState; set => ExecutionState = value; }
	IAbilityCancellationRequest? IAbilityExecutionContext.PendingCancellationRequest { get; set; }

	private readonly HashSet<IAbilityTask> _activeTasks = new();
	private readonly List<IAbilityTask> _capturedTasks = new();
	private bool _cleared;

}



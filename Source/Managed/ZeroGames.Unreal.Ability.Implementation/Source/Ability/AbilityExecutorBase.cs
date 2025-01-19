// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public abstract class AbilityExecutorBase : IAbilityExecutor
{
	
	protected virtual void HandleActivate(IAbilityExecutionContext activatedContext, IAbilityActivationRequest request){}
	void IAbilityExecutor.HandleActivate(IAbilityExecutionContext activatedContext, IAbilityActivationRequest request)
		=> HandleActivate(activatedContext, request);

	protected virtual void HandleCancel(IAbilityExecutionContext canceledContext, IAbilityCancellationRequest request){}
	void IAbilityExecutor.HandleCancel(IAbilityExecutionContext canceledContext, IAbilityCancellationRequest request)
	{
		if (canceledContext is IAbilityTaskContainer container)
		{
			container.Clear();
		}
		
		HandleCancel(canceledContext, request);
	}

	protected virtual void HandleEnd(IAbilityExecutionContext completedContext){}
	void IAbilityExecutor.HandleEnd(IAbilityExecutionContext completedContext)
	{
		if (completedContext is IAbilityTaskContainer container)
		{
			container.Clear();
		}
		
		HandleEnd(completedContext);
	}

	protected virtual void Tick(IAbilityExecutionContext context, float deltaSeconds){}
	void IAbilityExecutor.Tick(IAbilityExecutionContext context, float deltaSeconds)
	{
		if (context is IAbilityTaskContainer container)
		{
			container.Tick(deltaSeconds);
		}
		
		Tick(context, deltaSeconds);
	}

	protected virtual bool CanActivate(IAbilityScheduler scheduler, IAbilityActivationRequest request) => true;
	bool IAbilityExecutor.CanActivate(IAbilityScheduler scheduler, IAbilityActivationRequest request)
		=> CanActivate(scheduler, request);

	protected virtual bool CanActivate(IAbilityScheduler scheduler, IAbilityActivationRequest request, out IAbilityActivationTestResult? details)
	{
		details = null;
		return true;
	}
	bool IAbilityExecutor.CanActivate(IAbilityScheduler scheduler, IAbilityActivationRequest request, out IAbilityActivationTestResult? details)
		=> CanActivate(scheduler, request, out details);

	protected bool Cancel(IAbilityExecutionContext context)
	{
		if (context.ExecutionState <= EAbilityExecutionState.Active)
		{
			IAbilityCancellationRequest request = IAbilitySystemAllocator.Instance.AllocateCancellationRequest(context.Definition.ActivationGroup, context.Guid);
			return context.Scheduler.TryCancelAbility(request, out _);
		}

		return false;
	}

	protected bool NotifyCompletion(IAbilityExecutionContext context)
	{
		if (context.ExecutionState <= EAbilityExecutionState.Active)
		{
			context.Scheduler.EndAbility(context);
			return true;
		}

		return false;
	}

}



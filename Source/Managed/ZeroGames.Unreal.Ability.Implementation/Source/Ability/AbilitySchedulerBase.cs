// Copyright Zero Games. All Rights Reserved.

using System.Diagnostics.CodeAnalysis;

namespace ZeroGames.Unreal.Ability;

public abstract class AbilitySchedulerBase : IAbilityScheduler
{
	
	public void BeginPlay()
	{
		try
		{
			_state = EState.Playing;
			InternalBeginPlay();
		}
		catch (Exception ex)
		{
			UnhandledExceptionHelper.Guard(ex);
			_state = EState.Error;
		}
	}

	public void EndPlay()
	{
		try
		{
			InternalEndPlay();
			_state = EState.Dead;
		}
		catch (Exception ex)
		{
			UnhandledExceptionHelper.Guard(ex);
			_state = EState.Error;
		}
	}

	public void Tick(float deltaSeconds)
	{
		try
		{
			// Newly activated abilities during iteration are not ticked this time.
			_capturedGroups.Clear();
			foreach (var pair in _activeAbilityMap)
			{
				_capturedGroups.Add(pair.Key);
			}

			foreach (var group in _capturedGroups)
			{
				if (!_activeAbilityMap.TryGetValue(group, out var activeContext))
				{
					// Ability has already ended.
					continue;
				}
				
				activeContext.Definition.Executor.Tick(activeContext, deltaSeconds);
				// Executor can kill us.
				if (!IsValid)
				{
					return;
				}
			}

			InternalTick(deltaSeconds);
		}
		catch (Exception ex)
		{
			UnhandledExceptionHelper.Guard(ex);
		}
	}

	public bool TryActivateAbility(IAbilityActivationRequest request, [NotNullWhen(true)] out IAbilityExecutionContext? activatedContext)
	{
		activatedContext = null;

		if (!IsValid)
		{
			return false;
		}

		if (Avatar.IsExpired)
		{
			return false;
		}
		
		if (!CanActivateAbility(request))
		{
			return false;
		}

		try
		{
			activatedContext = InternalActivateAbility(request, default);
			return true;
		}
		catch (Exception ex)
		{
			UnhandledExceptionHelper.Guard(ex);
			return false;
		}
	}

	public bool TryCancelAbility(IAbilityCancellationRequest request, [NotNullWhen(true)] out IAbilityExecutionContext? canceledContext)
	{
		canceledContext = null;
		
		if (!IsValid)
		{
			return false;
		}
		
		if (Avatar.IsExpired)
		{
			return false;
		}
		
		if (!_activeAbilityMap.TryGetValue(request.ActivationGroup, out canceledContext))
		{
			return false;
		}

		if (request.Guid.IsValid)
		{
			if (canceledContext.Guid != request.Guid)
			{
				return false;
			}

			if (Avatar.GetNetMode() == ENetMode.NM_Client && request.Guid.IsAllocatedByAuthority)
			{
				return false;
			}
		}

		InternalCancelAbility(request, canceledContext);
		return true;
	}

	public bool TryGetActiveAbility(AbilityActivationGroup group, [NotNullWhen(true)] out IAbilityExecutionContext? activeContext)
		=> _activeAbilityMap.TryGetValue(group, out activeContext);

	public bool CanActivateAbility(IAbilityActivationRequest request)
	{
		if (!CanActivateAbilityShared(request))
		{
			return false;
		}
		
		try
		{
			return request.Definition.Executor.CanActivate(this, request);
		}
		catch (Exception ex)
		{
			UnhandledExceptionHelper.Guard(ex);
			return false;
		}
	}

	public bool CanActivateAbility(IAbilityActivationRequest request, out IAbilityActivationTestResult? details)
	{
		details = null;
		
		if (!CanActivateAbilityShared(request))
		{
			return false;
		}
		
		try
		{
			return request.Definition.Executor.CanActivate(this, request, out details);
		}
		catch (Exception ex)
		{
			UnhandledExceptionHelper.Guard(ex);
			return false;
		}
	}

	ActiveAbilityGuid IAbilityScheduler.GenerateNextActiveAbilityGuid()
	{
		if (_currentGuid == 0)
		{
			_currentGuid = Avatar.HasAuthority() ? 2ul : 1ul;
		}

		ActiveAbilityGuid result = new(_currentGuid);
		_currentGuid += 2;
		return result;
	}

	public Actor Avatar { get; }

	public bool IsValid => _state == EState.Playing;
	
	public event Action<IAbilityExecutionContext>? OnAbilityActivated;
	public event Action<IAbilityExecutionContext>? OnAbilityCancelled;
	public event Action<IAbilityExecutionContext>? OnAbilityCompleted;

	void IAbilityScheduler.EndAbility(IAbilityExecutionContext context)
	{
		if (!IsValid)
		{
			return;
		}
		
		if (Avatar.IsExpired)
		{
			return;
		}
		
		if (!_activeAbilityMap.TryGetValue(context.Definition.ActivationGroup, out var activeContext))
		{
			return;
		}

		if (activeContext != context)
		{
			return;
		}
		
		InternalEndAbility(context);
	}

	protected AbilitySchedulerBase(Actor avatar)
	{
		Avatar = avatar;
	}

	protected virtual void InternalBeginPlay(){}
	protected virtual void InternalEndPlay(){}
	protected virtual void InternalTick(float deltaSeconds){}
	
	protected virtual void HandleActivateAbility(IAbilityActivationRequest request, IAbilityExecutionContext activatedContext){}
	protected virtual void HandleCancelAbility(IAbilityCancellationRequest request, IAbilityExecutionContext canceledContext){}
	protected virtual void HandleEndAbility(IAbilityExecutionContext completedContext){}
	
	protected IAbilityExecutionContext InternalActivateAbility(IAbilityActivationRequest request, ActiveAbilityGuid guid)
	{
		try
		{
			AbilityActivationGroup group = request.Definition.ActivationGroup;
			if (_activeAbilityMap.TryGetValue(group, out var activeContext))
			{
				IAbilityCancellationRequest cancellationRequest = IAbilitySystemAllocator.Instance.AllocateCancellationRequest(group);
				InternalCancelAbility(cancellationRequest, activeContext);
			}

			IAbilityExecutionContext activatedContext = IAbilitySystemAllocator.Instance.AllocateExecutionContext(this, request.Definition, guid);
			activatedContext.ExecutionState = EAbilityExecutionState.PendingActivation;
			_activeAbilityMap[group] = activatedContext;
			
			HandleActivateAbility(request, activatedContext);
			activatedContext.Definition.Executor.HandleActivate(activatedContext, request);
			OnAbilityActivated?.Invoke(activatedContext);

			// Support End/Cancel ability during activation.
			EAbilityExecutionState currentExecutionState = activatedContext.ExecutionState;
			if (currentExecutionState == EAbilityExecutionState.PendingCompletion)
			{
				InternalEndAbility(activatedContext);
			}
			else if (currentExecutionState == EAbilityExecutionState.PendingCancellation)
			{
				InternalCancelAbility(activatedContext.PendingCancellationRequest!, activatedContext);
			}
			else
			{
				ensure(currentExecutionState == EAbilityExecutionState.PendingActivation);
				activatedContext.ExecutionState = EAbilityExecutionState.Active;
			}
			
			return activatedContext;
		}
		catch (Exception)
		{
			_activeAbilityMap.Remove(request.Definition.ActivationGroup);
			throw;
		}
	}

	protected void InternalEndAbility(IAbilityExecutionContext completedContext)
	{
		try
		{
			// Support cancel ability during activation.
			if (completedContext.ExecutionState == EAbilityExecutionState.PendingActivation)
			{
				completedContext.ExecutionState = EAbilityExecutionState.PendingCompletion;
			}
			else
			{
				HandleEndAbility(completedContext);
				completedContext.Definition.Executor.HandleEnd(completedContext);
				completedContext.ExecutionState = EAbilityExecutionState.Completed;
				OnAbilityCompleted?.Invoke(completedContext);
			}
		}
		catch (Exception ex)
		{
			UnhandledExceptionHelper.Guard(ex);
		}
		finally
		{
			ensure(_activeAbilityMap.Remove(completedContext.Definition.ActivationGroup));
		}
	}

	private bool CanActivateAbilityShared(IAbilityActivationRequest request)
	{
		if (!IsValid)
		{
			return false;
		}

		if (Avatar.IsExpired)
		{
			return false;
		}
		
		EAbilityNetPolicy netPolicy = request.Definition.NetPolicy;
		ENetMode netMode = Avatar.GetNetMode();
		if (netPolicy != EAbilityNetPolicy.LocalOnly && netMode == ENetMode.NM_Client)
		{
			return false;
		}

		return true;
	}
	
	private void InternalCancelAbility(IAbilityCancellationRequest request, IAbilityExecutionContext canceledContext)
	{
		try
		{
			// Support cancel ability during activation.
			if (canceledContext.ExecutionState == EAbilityExecutionState.PendingActivation)
			{
				canceledContext.ExecutionState = EAbilityExecutionState.PendingCancellation;
				canceledContext.PendingCancellationRequest = request;
			}
			else
			{
				HandleCancelAbility(request, canceledContext);
				canceledContext.Definition.Executor.HandleCancel(canceledContext, request);
				canceledContext.ExecutionState = EAbilityExecutionState.Canceled;
				OnAbilityCancelled?.Invoke(canceledContext);
			}
		}
		catch (Exception ex)
		{
			UnhandledExceptionHelper.Guard(ex);
		}
		finally
		{
			ensure(_activeAbilityMap.Remove(request.ActivationGroup));
		}
	}

	private enum EState : uint8
	{
		Initialized,
		Playing,
		Dead,
		Error,
	}

	private EState _state;
	
	private readonly Dictionary<AbilityActivationGroup, IAbilityExecutionContext> _activeAbilityMap = new();
	private readonly List<AbilityActivationGroup> _capturedGroups = new();

	private uint64 _currentGuid;

}



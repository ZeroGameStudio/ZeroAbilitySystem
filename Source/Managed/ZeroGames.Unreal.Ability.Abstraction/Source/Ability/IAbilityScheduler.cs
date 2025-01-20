// Copyright Zero Games. All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using ZeroGames.ZSharp.UnrealEngine.Engine;

namespace ZeroGames.Unreal.Ability;

public interface IAbilityScheduler
{
	void BeginPlay();
	void EndPlay();

	void Tick(float deltaSeconds);
	
	bool TryActivateAbility(IAbilityActivationRequest request, [NotNullWhen(true)] out IAbilityExecutionContext? activatedContext);
	bool TryCancelAbility(IAbilityCancellationRequest request, [NotNullWhen(true)] out IAbilityExecutionContext? canceledContext);
	bool TryGetActiveAbility(AbilityActivationGroup group, [NotNullWhen(true)] out IAbilityExecutionContext? activeContext);

	bool CanActivateAbility(IAbilityActivationRequest request);
	bool CanActivateAbility(IAbilityActivationRequest request, out IAbilityActivationTestResult? details);
	
	void EndAbility(IAbilityExecutionContext context);

	void DispatchAbilityEvent(AbilityEventKey key, AbilityActivationGroup group, ActiveAbilityGuid guid = default, IAbilityEventPayload? payload = null);
	
	ActiveAbilityGuid GenerateNextActiveAbilityGuid();
	
	Actor Avatar { get; }
	
	bool IsValid { get; }
	
	event Action<IAbilityExecutionContext>? OnAbilityActivated;
	event Action<IAbilityExecutionContext>? OnAbilityCancelled;
	event Action<IAbilityExecutionContext>? OnAbilityCompleted;
}

public static class AbilitySchedulerExtensions
{
	public static void DispatchAbilityEvent(this IAbilityScheduler @this, AbilityEventKey key, IAbilityExecutionContext context, IAbilityEventPayload? payload = null)
		=> @this.DispatchAbilityEvent(key, context.Definition.ActivationGroup, context.Guid, payload);
}



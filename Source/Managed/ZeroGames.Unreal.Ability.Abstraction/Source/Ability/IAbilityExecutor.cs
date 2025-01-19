// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityExecutor
{
	void HandleActivate(IAbilityExecutionContext activatedContext, IAbilityActivationRequest request);
	void HandleCancel(IAbilityExecutionContext canceledContext, IAbilityCancellationRequest request);
	void HandleEnd(IAbilityExecutionContext completedContext);
	
	void Tick(IAbilityExecutionContext context, float deltaSeconds);

	bool CanActivate(IAbilityScheduler scheduler, IAbilityActivationRequest request);
	bool CanActivate(IAbilityScheduler scheduler, IAbilityActivationRequest request, out IAbilityActivationTestResult? details);
}



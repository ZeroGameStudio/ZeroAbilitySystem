// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public class AbilityScheduler_Proxy : AbilitySchedulerBase, IAbilityScheduler_Proxy
{

	public AbilityScheduler_Proxy(Actor avatar) : base(avatar)
	{
		
	}

	public void ReceiveAbilityActivation(IAbilityActivationRequest request)
	{
		InternalActivateAbility(request);
	}

	public void ReceiveAbilityCancellation(IAbilityCancellationRequest request)
	{
		TryCancelAbility(request, out _);
	}

	public void ReceiveAbilityCompletion(AbilityActivationGroup group, ActiveAbilityGuid guid)
	{
		if (TryGetActiveAbility(group, out var activeContext) && activeContext.Guid == guid)
		{
			InternalEndAbility(activeContext);
		}
	}
	
}
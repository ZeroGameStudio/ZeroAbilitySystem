// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public class AbilityScheduler_Proxy : AbilitySchedulerBase, IAbilityScheduler_Proxy
{

	public AbilityScheduler_Proxy(Actor avatar) : base(avatar)
	{
		
	}

	public void NetSerializeAbilityActivation(IAbilityActivationRequest request, ActiveAbilityGuid guid)
	{
		InternalActivateAbility(request, guid);
	}

	public void NetSerializeAbilityCancellation(IAbilityCancellationRequest request)
	{
		TryCancelAbility(request, out _);
	}

	public void NetSerializeAbilityCompletion(AbilityActivationGroup group, ActiveAbilityGuid guid)
	{
		if (TryGetActiveAbility(group, out var activeContext) && activeContext.Guid == guid)
		{
			InternalEndAbility(activeContext);
		}
	}

	public void NetSerializeAbilityEvent(AbilityEventKey key, AbilityActivationGroup group, ActiveAbilityGuid guid, IAbilityEventPayload? payload)
	{
		InternalDispatchAbilityEvent(key, group, guid, payload, false);
	}
	
}
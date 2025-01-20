// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public class AbilityScheduler_Authority : AbilitySchedulerBase, IAbilityScheduler_Authority
{
	
	public AbilityScheduler_Authority(Actor avatar, IAbilityNetReplicator? replicator) : base(avatar)
	{
		Replicator = replicator;
	}
	
	public IAbilityNetReplicator? Replicator { get; }

	protected override void HandleActivateAbility(IAbilityActivationRequest request, IAbilityExecutionContext activatedContext)
	{
		base.HandleActivateAbility(request, activatedContext);

		if (activatedContext.Definition.NetPolicy == EAbilityNetPolicy.Replicated)
		{
			Replicator?.NetSerializeAbilityActivation(request, activatedContext.Guid);
		}
	}

	protected override void HandleCancelAbility(IAbilityCancellationRequest request, IAbilityExecutionContext canceledContext)
	{
		if (canceledContext.Definition.NetPolicy == EAbilityNetPolicy.Replicated)
		{
			Replicator?.NetSerializeAbilityCancellation(request);
		}
		
		base.HandleCancelAbility(request, canceledContext);
	}

	protected override void HandleEndAbility(IAbilityExecutionContext completedContext)
	{
		if (completedContext.Definition.NetPolicy == EAbilityNetPolicy.Replicated)
		{
			Replicator?.NetSerializeAbilityCompletion(completedContext.Definition.ActivationGroup, completedContext.Guid);
		}
		
		base.HandleEndAbility(completedContext);
	}

	protected override void HandleDispatchAbilityEvent(IAbilityEventDefinition definition, AbilityEventKey key, AbilityActivationGroup group, ActiveAbilityGuid guid, IAbilityEventPayload? payload)
	{
		base.HandleDispatchAbilityEvent(definition, key, group, guid, payload);

		if (definition.NetPolicy == EAbilityEventNetPolicy.Replicated)
		{
			Replicator?.NetSerializeAbilityEvent(key, group, guid, payload);
		}
	}
	
}



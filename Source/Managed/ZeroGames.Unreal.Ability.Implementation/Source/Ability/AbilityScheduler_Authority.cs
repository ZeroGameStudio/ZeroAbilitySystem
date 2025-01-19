// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public class AbilityScheduler_Authority : AbilitySchedulerBase, IAbilityScheduler_Authority
{
	
	public AbilityScheduler_Authority(Actor avatar, IAbilityReplicationProxy? replicationProxy) : base(avatar)
	{
		ReplicationProxy = replicationProxy;
	}
	
	public IAbilityReplicationProxy? ReplicationProxy { get; }

	protected override void HandleActivateAbility(IAbilityActivationRequest request, IAbilityExecutionContext activatedContext)
	{
		base.HandleActivateAbility(request, activatedContext);

		if (activatedContext.Definition.NetPolicy == EAbilityNetPolicy.Replicated)
		{
			ReplicationProxy?.ReplicateAbilityActivation(request, activatedContext.Guid);
		}
	}

	protected override void HandleCancelAbility(IAbilityCancellationRequest request, IAbilityExecutionContext canceledContext)
	{
		if (canceledContext.Definition.NetPolicy == EAbilityNetPolicy.Replicated)
		{
			ReplicationProxy?.ReplicateAbilityCancellation(request);
		}
		
		base.HandleCancelAbility(request, canceledContext);
	}

	protected override void HandleEndAbility(IAbilityExecutionContext completedContext)
	{
		if (completedContext.Definition.NetPolicy == EAbilityNetPolicy.Replicated)
		{
			ReplicationProxy?.ReplicateAbilityCompletion(completedContext.Definition.ActivationGroup, completedContext.Guid);
		}
		
		base.HandleEndAbility(completedContext);
	}
}



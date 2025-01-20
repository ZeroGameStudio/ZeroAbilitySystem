// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityEventExecutor
{
	void ReceiveAbilityEvent(IAbilityExecutionContext context, IAbilityEventDefinition definition, IAbilityEventPayload? payload);
}



// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityTaskContainer
{
	void RegisterTask(IAbilityTask task);
	void CancelTask(IAbilityTask task);
	void CompleteTask(IAbilityTask task);
	void Clear();
	void Tick(float deltaSeconds);
}



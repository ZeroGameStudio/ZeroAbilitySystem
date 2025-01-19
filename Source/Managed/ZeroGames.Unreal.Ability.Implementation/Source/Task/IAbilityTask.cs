// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityTask
{
	void Activate();
	void Cancel();
	void Tick(float deltaSeconds);
	
	IAbilityTaskContainer? Owner { get; set; }
}



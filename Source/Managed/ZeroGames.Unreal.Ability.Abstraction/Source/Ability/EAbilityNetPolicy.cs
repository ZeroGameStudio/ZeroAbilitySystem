// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public enum EAbilityNetPolicy
{
	/// <summary>
	/// Ability must be activated by authority and will be replicated to proxies.
	/// </summary>
	Replicated,
	
	/// <summary>
	/// Ability must be activated by authority and will not be replicated to proxies.
	/// </summary>
	AuthorityOnly,
	
	/// <summary>
	/// Ability can be activated by either authority or proxy but will not be replicated.
	/// </summary>
	LocalOnly,
}



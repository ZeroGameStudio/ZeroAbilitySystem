// Copyright Zero Games. All Rights Reserved.

using System.Reflection;
using System.Runtime.Loader;

namespace ZeroGames.Unreal.Ability;

public interface IAbilitySystemAllocator
{
	public static IAbilitySystemAllocator Instance
	{
		get
		{
			if (_instance is null)
			{
				string allocatorTypeName = AbilitySystemSettings.AllocatorType;
				Type allocatorType = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly())!.Assemblies
					.SelectMany(asm => asm.GetTypes())
					.First(type => type.IsAssignableTo(typeof(IAbilitySystemAllocator)) && type.FullName == allocatorTypeName);
				
				_instance = (IAbilitySystemAllocator)Activator.CreateInstance(allocatorType)!;
			}
			
			return _instance;
		}
	}

	IAbilityActivationRequest AllocateActivationRequest(IAbilityDefinition definition);
	IAbilityCancellationRequest AllocateCancellationRequest(AbilityActivationGroup group, ActiveAbilityGuid guid = default);
	IAbilityExecutionContext AllocateExecutionContext(IAbilityScheduler scheduler, IAbilityDefinition definition, ActiveAbilityGuid guid = default);

	private static IAbilitySystemAllocator? _instance;
}



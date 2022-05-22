using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Entities;
using GPUInstance;

[assembly: RegisterGenericComponentType(typeof(InstanceData<GPUInstance.InstanceProperties>))]
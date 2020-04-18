using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MovementData : IComponentData {
    public float gravityRate;
    public float speed;
    public float3 direction;
}
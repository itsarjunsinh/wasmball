using Unity.Entities;
using Unity.Tiny.Input;

class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var input = World.GetOrCreateSystem<InputSystem>();
        bool isTapped = input.GetMouseButtonUp(0);

        Entities.ForEach((ref MovementData moveData) => {
            if (isTapped) {
                moveData.direction.y = 4f;
            }
        }).ScheduleParallel();
    }
}
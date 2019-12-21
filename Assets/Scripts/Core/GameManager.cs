using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Collections; 
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
//using Unity.Physics;

public class GameManager : MonoBehaviour
{
    EntityManager entityManager;
    [SerializeField] private Mesh quadMesh;
    [SerializeField] private Material agentMaterial;
    [SerializeField] private Material playerMaterial;

    void Start()
    {
        entityManager = World.Active.EntityManager;

        EntityArchetype playerArchetype = entityManager.CreateArchetype(
            typeof(PlayerTag),
            typeof(AABB),
            typeof(HungerComponent),
            typeof(LocalToWorld),
            typeof(MoveSpeedComponent),
            typeof(RenderMesh),
            typeof(TargetComponent),
            typeof(Translation)
            );

        EntityArchetype agentArchetype = entityManager.CreateArchetype(
            typeof(HungerComponent),
            typeof(AABB),
            typeof(LocalToWorld),
            typeof(MoveSpeedComponent),
            typeof(RenderMesh),
            typeof(TargetComponent),
            typeof(Translation)
            );

        Entity player = entityManager.CreateEntity(playerArchetype);

        entityManager.SetComponentData(player, new HungerComponent { Value = 10 });
        entityManager.SetComponentData(player, new MoveSpeedComponent { Value = 4f });
        entityManager.SetComponentData(player, new Translation { Value = new float3(0,4,0)  });

        Translation position = entityManager.GetComponentData<Translation>(player);

        entityManager.SetComponentData(player, new AABB { Min = position.Value - 0.5f, Max = position.Value + 0.5f });


        entityManager.SetSharedComponentData(player, new RenderMesh { mesh = quadMesh, material = playerMaterial });

        NativeArray<Entity> agents = new NativeArray<Entity>(1, Allocator.Temp);

        entityManager.CreateEntity(agentArchetype, agents);

        for (int i = 0; i < agents.Length; i++)
        {
            Entity agent = agents[i];
            entityManager.SetComponentData(agent, new HungerComponent { Value = 10 });
            entityManager.SetComponentData(agent, new MoveSpeedComponent { Value = 4f });
            entityManager.SetComponentData(agent, new TargetComponent { Entity = player });
            entityManager.SetComponentData(agent, new Translation { Value = new float3(UnityEngine.Random.Range(-8,8), 0, 0) });

            entityManager.SetSharedComponentData(agent, new RenderMesh { mesh = quadMesh, material = agentMaterial });
        }

        agents.Dispose();
        
    }

    void Update()
    {
        
    }
}

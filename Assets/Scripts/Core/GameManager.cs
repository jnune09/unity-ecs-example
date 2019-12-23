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
    [SerializeField] private Material actorMaterial;
    [SerializeField] private Material playerMaterial;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        SpawnPlayer();

        for (int i = 0; i < 1; i++)
        {
            //SpawnPlayer();
            SpawnActor();
        }

    }

    public void SpawnPlayer()
    {
        Entity player = entityManager.CreateEntity(
            typeof(PlayerTag),
            typeof(AABB),
            typeof(Hunger),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(Translation),
            typeof(Velocity)
            );

        entityManager.SetComponentData(player, new Hunger { Value = 10 });
        entityManager.SetComponentData(player, new Velocity { Speed = 6f });
        entityManager.SetComponentData(player, new Translation { Value = new float3(UnityEngine.Random.Range(-10, 10), 5, 0) });
        entityManager.SetSharedComponentData(player, new RenderMesh { mesh = quadMesh, material = playerMaterial });
    }

    public void SpawnActor()
    {
        Entity actor = entityManager.CreateEntity(
            typeof(ActorTag),
            typeof(AABB),
            typeof(Hunger),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(State),
            typeof(Translation),
            typeof(Velocity)
            );

        entityManager.SetComponentData(actor, new Hunger { Value = 10 });
        entityManager.SetComponentData(actor, new Velocity { Speed = 4f });
        entityManager.SetComponentData(actor, new Translation { Value = new float3(UnityEngine.Random.Range(-10, 10), 0, 0) });
        entityManager.SetSharedComponentData(actor, new RenderMesh { mesh = quadMesh, material = actorMaterial });
    }

    void Update()
    {
        
    }
}

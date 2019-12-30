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
    [SerializeField] private Material vendorMaterial;

    void Start()
    {
        Vector3[] characterSpriteVertices = new Vector3[4];

        characterSpriteVertices[0] = new Vector3(-8, -16);
        characterSpriteVertices[1] = new Vector3(8, -16);
        characterSpriteVertices[2] = new Vector3(-8, 16);
        characterSpriteVertices[3] = new Vector3(8, 16);

        quadMesh.vertices = characterSpriteVertices;

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        SpawnPlayer();
        SpawnVendor();
        SpawnActor();

    }

    public void SpawnPlayer()
    {
        Entity player = entityManager.CreateEntity(
            typeof(PlayerTag),
            typeof(AABB),
            typeof(Collision),
            typeof(Direction),
            typeof(HitBox),
            typeof(Hunger),
            typeof(InteractBox),
            typeof(Inventory),
            typeof(LocalToWorld),
            typeof(PlayerInput),
            typeof(RenderMesh),
            typeof(Speed),
            typeof(Translation),
            typeof(Velocity)
            );

        //Entity item = entityManager.CreateEntity();
        //entityManager.GetBuffer<Inventory>(player).Add(new Inventory { Item = item, Count = 1 });

        entityManager.SetComponentData(player, new AABB { Position = new float3(-8, -16, 0), Size = new float3(16, 8, 0) });
        entityManager.SetComponentData(player, new HitBox { Position = new float3(-8, -16, 0), Size = new float3(16, 32, 0) });
        entityManager.SetComponentData(player, new Hunger { Value = 10 });
        entityManager.SetComponentData(player, new InteractBox { Offset = -4f, Distance = 16f, Size = new float3(8, 8, 0) });
        entityManager.SetComponentData(player, new Speed { Value = 80f });
        entityManager.SetComponentData(player, new Translation { Value = new float3(UnityEngine.Random.Range(100, 100), 5, 0) });
        entityManager.SetSharedComponentData(player, new RenderMesh { mesh = quadMesh, material = playerMaterial });
    }

    public void SpawnActor()
    {
        Entity actor = entityManager.CreateEntity(
            typeof(ActorTag),
            typeof(AgentTag),
            typeof(AABB),
            typeof(Collision),
            typeof(Direction),
            typeof(HitBox),
            typeof(Hunger),
            typeof(Inventory),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(Speed),
            typeof(Translation),
            typeof(Velocity)
            );

        entityManager.SetComponentData(actor, new AABB { Position = new float3(-8, -16, 0), Size = new float3(16, 8, 0) });
        entityManager.SetComponentData(actor, new HitBox { Position = new float3(-8, -16, 0), Size = new float3(16, 32, 0) });
        entityManager.SetComponentData(actor, new Hunger { Value = 10 });
        entityManager.SetComponentData(actor, new Speed { Value = 60f });
        entityManager.SetComponentData(actor, new Translation { Value = new float3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), 0) });
        entityManager.SetSharedComponentData(actor, new RenderMesh { mesh = quadMesh, material = actorMaterial });
    }

    public void SpawnVendor()
    {
        Entity vendor = entityManager.CreateEntity(
            typeof(AABB),
            typeof(Collision),
            typeof(Direction),
            typeof(Hunger),
            typeof(Inventory),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(Speed),
            typeof(Translation),
            typeof(Velocity)
            );

        entityManager.SetComponentData(vendor, new AABB { Position = new float3(-8, -16, 0), Size = new float3(16, 8, 0) });
        entityManager.SetComponentData(vendor, new Speed { Value = 60f });
        entityManager.SetComponentData(vendor, new Translation { Value = new float3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), 0) });
        entityManager.SetSharedComponentData(vendor, new RenderMesh { mesh = quadMesh, material = vendorMaterial });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
        }
    }
}

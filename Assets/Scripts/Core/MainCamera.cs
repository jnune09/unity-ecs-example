using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class MainCamera : MonoBehaviour
{
    EntityManager entityManager;
    new Entity camera;

    Vector3 position;
    Vector3 direction;
    float distance;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        camera = entityManager.CreateEntity(
                typeof(Camera)
            );

        entityManager.SetComponentData<Camera>(camera, new Camera { Position = new float3(0, 0, -10) });

        speed = 5f;

    }

    // Update is called once per frame
    void Update()
    {
        position = entityManager.GetComponentData<Camera>(camera).Position;

        direction = (position - transform.position).normalized;
        distance = Vector3.Distance(position, transform.position);

        transform.position += direction * distance * speed * Time.deltaTime;
    }
}

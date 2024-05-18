using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private Rigidbody _prefab;

    private static float _separationChance;

    private void Start()
    {
        _separationChance = 100;
    }

    private void OnMouseUpAsButton()
    {
        Destroy(gameObject);
        SpawnNewObjects();        
    }

    private void Explode(List<Rigidbody> objects, float explosionForce, float explosionRaduis)
    {
        foreach (Rigidbody explodableObject in objects)
        {
            explodableObject.AddExplosionForce(explosionForce, transform.position, explosionRaduis);
        }
    }

    private void SpawnNewObjects()
    {
        if (Random.Range(0, 101) <= _separationChance)
        {
            int randomSpawnCubes = Random.Range(2, 7);
            Vector3 currentPosition = transform.position;

            List<Rigidbody> spawnedObjects = new();

            _prefab.transform.localScale = new Vector3(_prefab.transform.localScale.x / 2, _prefab.transform.localScale.y / 2, 
                _prefab.transform.localScale.z / 2);

            for (int i = 0; i < randomSpawnCubes; i++)
            {
                _prefab.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                spawnedObjects.Add(Instantiate(_prefab, currentPosition, Quaternion.identity));
                currentPosition += new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f));
            }

            _separationChance /= 2;

            Explode(spawnedObjects, _explosionForce, _explosionRadius);
        }
        else
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius / _prefab.transform.localScale.x);

            List<Rigidbody> cubes = new();

            foreach (Collider hit in hits)
            {
                if (hit.attachedRigidbody != null)
                    cubes.Add(hit.attachedRigidbody);
            }

            Explode(cubes, _explosionForce / _prefab.transform.localScale.x, _explosionRadius / _prefab.transform.localScale.x);
        }
    }
}

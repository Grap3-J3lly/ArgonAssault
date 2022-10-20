using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float respawnDelay = 2f;
    bool isTransitioning = false;

    [SerializeField] ParticleSystem explosionParticles;

    private void OnTriggerEnter(Collider other) 
    {
        if(isTransitioning) {return;}

        StartCrashSequence();
    }

    private void StartCrashSequence()
    {
        isTransitioning = true;

        explosionParticles.Play();
        GetComponent<PlayerControls>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Invoke("Respawn", respawnDelay);
    }

    private void Respawn()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
    }

}

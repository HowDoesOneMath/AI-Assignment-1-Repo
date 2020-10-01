using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Logic behind the exploding beans
[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class BeanScript : MonoBehaviour
{
    //Every material, particle system and sound that a bean can make.
    //The bean will assume one or more of each depending on its HOT/NOISY/SAFE characteristics
    public Material normalBean;
    public Material scaredBean;
    public Material deadBean;
    public Material happyBean;
    public ParticleSystem fuse;
    public ParticleSystem explosion;
    public ParticleSystem confetti;
    public AudioSource nonono;
    public AudioSource kaboom;
    public AudioSource yaaay;
    public bool isSafe;
    bool isUsed = false;
    public float rotSpeed = 360f;
    public float noticeDistance = 8f;

    BeanChances myChances = null;

    Rigidbody rb;
    MeshRenderer mr;

    AudioSource ohno = null;
    ParticleSystem smoky = null;
    AudioSource boomboom = null;
    ParticleSystem explodey = null;
    AudioSource yay = null;
    ParticleSystem party = null;

    //A large amount of initialization
    private void Awake()
    {
        //Grab the HOT/NOISY/SAFE data from the static variable set in SpawnTheBeanz.cs
        myChances = SpawnTheBeanz.worldChance;

        mr = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        //Prevent movement by rigidbody
        rb.constraints = RigidbodyConstraints.FreezeAll;

        //Assign unique materials and particle effects depending on initial HOT/NOISY parameters
        if (myChances.HOT)
        {
            smoky = Instantiate(fuse, transform, false);
            smoky.transform.localPosition = Vector3.up;
        }

        if (myChances.NOISY)
        {
            ohno = Instantiate(nonono, transform, false);
            ohno.transform.localPosition = Vector3.up * 0.5f;
            mr.material = Instantiate(scaredBean);
            mr.material.SetFloat("RandomOffset", Random.Range(0, 2.0f * Mathf.PI));
        }
        else
        {
            mr.material = Instantiate(normalBean);
            mr.material.SetFloat("RandomOffset", Random.Range(0, 2.0f * Mathf.PI));
        }

        isSafe = myChances.SAFE;

        //mr.material = Instantiate(mr.material);
        //
        //mr.material.SetFloat("RandomOffset", Random.Range(0, Mathf.PI));
    }

    //Beans are set to face the player if they're within a certain distance from them
    //They're also set to no longer do this if they become 'used', when the player explodes them.
    private void Update()
    {
        if (isUsed)
            return;

        if (PlayerScript.mainPlayer != null)
        {
            Vector3 playerDir = PlayerScript.mainPlayer.transform.position - transform.position;
            playerDir = Vector3.ProjectOnPlane(playerDir, Vector3.up);

            if (playerDir.sqrMagnitude < noticeDistance * noticeDistance)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.left, playerDir.normalized), rotSpeed * Time.deltaTime);
            }
        }
    }

    //This causes the bean to explode in either fire or confetti.
    //Only ever called from the player.
    public void PushButton()
    {
        if (!isUsed)
        {
            isUsed = true;

            StartCoroutine(MakeStuffHappen());
        }
    }

    //The flashy effects that follow
    IEnumerator MakeStuffHappen()
    {
        //Destroy smoke/'no' sound effect immediately
        if (ohno != null)
        {
            Destroy(ohno.gameObject);
            ohno = null;
        }

        if (smoky != null)
        {
            Destroy(smoky.gameObject);
            smoky = null;
        }

        //If the player is safe, spawn confetti
        //Otherwise, create an explosion
        if (isSafe)
        {
            mr.material = Instantiate(happyBean);
            mr.material.SetFloat("RandomOffset", Random.Range(0, 2.0f * Mathf.PI));

            //Save references to these effects, so they may be deleted later.
            yay = Instantiate(yaaay, transform.position, Quaternion.identity);
            party = Instantiate(confetti, transform.position, confetti.transform.rotation);
        }
        else
        {
            mr.material = Instantiate(deadBean);

            //Causes them to tumble once they go boom
            rb.constraints = RigidbodyConstraints.None;

            float launch = Random.Range(10f, 30f);
            float horizontal = Random.Range(0f, 10f);
            float angle = Random.Range(0, Mathf.PI * 2f);

            //Set them moving and spinning to make it dramatic
            rb.velocity = new Vector3(horizontal * Mathf.Sin(angle), launch, horizontal * Mathf.Cos(angle));
            rb.angularVelocity = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));

            //Save references to these effects, so they may be deleted later.
            boomboom = Instantiate(kaboom, transform.position, Quaternion.identity);
            explodey = Instantiate(explosion, transform.position, explosion.transform.rotation);
        }

        //Wait for a little bit
        yield return new WaitForSeconds(8f);

        //Destroy the object
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    //Destroys any lingering effects before removing the object itself.
    private void OnDestroy()
    {
        if (ohno != null)
        {
            Destroy(ohno.gameObject);
            ohno = null;
        }

        if (smoky != null)
        {
            Destroy(smoky.gameObject);
            smoky = null;
        }

        if (yay != null)
        {
            Destroy(yay.gameObject);
            yay = null;
        }

        if (party != null)
        {
            Destroy(party.gameObject);
            party = null;
        }

        if (boomboom != null)
        {
            Destroy(boomboom.gameObject);
            boomboom = null;
        }

        if (explodey != null)
        {
            Destroy(explodey.gameObject);
            explodey = null;
        }
    }
}

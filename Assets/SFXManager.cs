using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using UnityEngine.VFX;
using JetBrains.Annotations;

public class SFXManager : MonoBehaviour
{
    public GameObject prefab;
    public AudioSource sourcePrefab;
    public AudioClip smallZoom;
    public AudioClip bigZoom;
    public AudioClip cities;
    public AudioClip[] farm;
    public AudioClip[] fuel;
    public AudioClip[] metal;
    public AudioClip[] minerals;
    public AudioClip[] boat;
    public AudioClip[] air;
    public AudioClip[] train;
    public AudioClip[] truck;
    public AudioClip[] OST;
    public NodeManager nodeManager;
    AudioSource newSource;
    int index;

    public void SmallZoomSoundEffect(Transform parent)
    {
        GameObject Prefab = Instantiate(prefab);
        AudioSource source = Prefab.GetComponent<AudioSource>();
        source.clip = smallZoom;
        source.transform.parent = parent;
        source.transform.position = parent.position;
        source.Play();
    }
    public AudioSource BigZoomSoundEffect(Transform parent)
    {
        GameObject Prefab = Instantiate(prefab);
        AudioSource source = Prefab.GetComponent<AudioSource>();
        source.clip = bigZoom;
        source.transform.parent = parent;
        source.transform.position = parent.position;
        source.Play();
        return source;
    }
    public AudioSource citySFX(Transform parent)
    {
        GameObject Prefab = Instantiate(prefab);
        AudioSource source = Prefab.GetComponent<AudioSource>();
        source.clip = cities;
        source.transform.parent = parent;
        source.transform.position = parent.position;
        source.Play();
        return source;
    }
    public AudioSource farmSFX(int index, Transform parent)
    {
        GameObject Prefab = Instantiate(prefab);
        AudioSource source = Prefab.GetComponent<AudioSource>();
        source.clip = farm[index];
        source.transform.parent = parent;
        source.transform.position = parent.position;
        source.Play();
        return source;
    }
    public AudioSource fuelSFX(int index, Transform parent)
    {
        GameObject Prefab = Instantiate(prefab);
        AudioSource source = Prefab.GetComponent<AudioSource>();
        source.clip = fuel[index];
        source.transform.parent = parent;
        source.transform.position = parent.position;
        source.Play();
        return source;
    }
    public AudioSource metalSFX(int index, Transform parent)
    {
        GameObject Prefab = Instantiate(prefab);
        AudioSource source = Prefab.GetComponent<AudioSource>();
        source.clip = metal[index];
        source.transform.parent = parent;
        source.transform.position = parent.position;
        source.Play();
        return source;
    }
    public AudioSource mineralsSFX(int index, Transform parent)
    {
        GameObject Prefab = Instantiate(prefab);
        AudioSource source = Prefab.GetComponent<AudioSource>();
        source.clip = minerals[index];
        source.transform.parent = parent;
        source.transform.position = parent.position;
        source.Play();
        return source;
    }
    public AudioSource ost(int index, Transform parent)
    {

        GameObject Prefab = Instantiate(prefab);
        Prefab.GetComponent<CameraPlayer>().maxDistance = 100f;
        AudioSource source = Prefab.GetComponent<AudioSource>();
        source.clip = OST[index];
        source.transform.position = new Vector3(0, 0, 0);
        source.Play();
        return source;
    }
    public IEnumerator sfx()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        AudioSource source = new AudioSource();
        source = ost(nodeManager.CityEra, this.transform);
        source.Play();
        yield return new WaitUntil(() => nodeManager.CityEra == 1);
        source.Stop();
        source = ost(1, this.transform);
        source.Play();
        yield return new WaitUntil(() => nodeManager.CityEra == 2);
        source.Stop();
        source = ost(2, this.transform);
        source.Play();
    }
    private void Start()
    {
        StartCoroutine(sfx());
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class FarmNode : ResourceNode
{
	private void Awake()
	{
		resourceType = ResourceType.Food;
        StartCoroutine(Generation());
        prefabPathName = "Assets/Nodes Prefabs/Farm Node Prefab.prefab";
        StartCoroutine(sfx());


    }


    public IEnumerator sfx()
    {
        yield return new WaitUntil(() => used);
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        sfxmanager = manager.GetComponent<SFXManager>();
        AudioSource source = sfxmanager.farmSFX((int)City.CityEra, this.transform);
        source.Stop();
        source.Play();
        if ((int)City.CityEra == 1)
        {
            source.Stop();
            source.Play();
            yield return new WaitUntil(() => City.CityEra == Era.Modern);
            source = sfxmanager.farmSFX(1, this.transform);
            source.Stop();
            source.Play();
        }
        if ((int)City.CityEra == 2)
        {
            source.Stop();
            source.Play();
            yield return new WaitUntil(() => City.CityEra == Era.Futuristic);
            source = sfxmanager.farmSFX(2, this.transform);
            source.Stop();
            source.Play();
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(sfx());


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void Selected()
	{
		throw new System.NotImplementedException();
	}

	public override void Setup()
	{
		base.Setup();
		resourceType = ResourceType.Food;
	}
}

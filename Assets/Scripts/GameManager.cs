using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Settings Settings;
    private static GameManager instance = null;
    public GameObject TradeRoutesGO;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

	private void Awake()
	{
		instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

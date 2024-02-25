using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableEventTriggerer : MonoBehaviour
{
    [Space]
    public UnityEvent onEnable = new UnityEvent();


	private void OnEnable()
	{
		onEnable.Invoke();
	}
}

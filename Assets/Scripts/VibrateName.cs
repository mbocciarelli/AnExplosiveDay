using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateName : MonoBehaviour
{
    private bool _secondDone = true;
    private float _speed = 5.0f;
    [SerializeField] private Transform titleTransform;
    private Transform _firstTransform;
    private float _cooldown = 3;
    // Start is called before the first frame update
    void Start()
    {
        _firstTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_secondDone)
        {
            if (_cooldown != 0)
            {
                Vibrate();
            }
            else
            {
                Recalibrate();
            }
        }
        
    }


    public void Vibrate()
    {
        //titleTransform.Rotate(titleTransform * _speed * Time.deltaTime, 45);
        StartCoroutine(Cooldown());
        Debug.Log("Vibrate");
    }
    
    public void Recalibrate()
    {
        transform.rotation = _firstTransform.rotation;
        
    }
    IEnumerator Cooldown()
    {
        _secondDone = false;
        yield return new WaitForSeconds(0.5f);
        _secondDone = true;
    }
}

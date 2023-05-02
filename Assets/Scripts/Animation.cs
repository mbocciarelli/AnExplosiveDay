using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    private float speed = 650;
    private bool _isFirstMoveEnd = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < (1080 / 2))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else if (transform.position.y >= (1080 / 2) && transform.position.y < 2000)
        {
            StartCoroutine(CountChrono());
            if (_isFirstMoveEnd)
                {
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                }
        }
    }

    private void OnEnable()
    {
        _isFirstMoveEnd = false;
        transform.position = new Vector3(transform.position.x, -600, transform.position.z);
    }

    IEnumerator CountChrono()
    {
        //_isFirstMoveEnd = false;
        yield return new WaitForSeconds(0.5f);
        _isFirstMoveEnd = true;
    }
}
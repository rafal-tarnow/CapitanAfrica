using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplodeBomb : MonoBehaviour
{
    public GameObject explosion;

    void explode(Collision2D col)
    {
            Destroy(col.gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Layer layer = col.gameObject.GetComponent<Layer>();
        if (layer != null)
        {
            if (layer.checkLayer(Layer.Type.PLAYER))
            {
                    explode(col);
            }
        }
    }
}

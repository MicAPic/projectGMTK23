using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float distanceRay = 100;
    [SerializeField]
    private Transform laserFirePoint;
    public LineRenderer lineRenderer;

    public ParticleSystem laserEndParticles;

    private bool endParticlesPlaying;

    private void Start()
    {
        lineRenderer.material.color = Color.red;
    }

    private void Update()
    {
        ShootLaser();
    }



    private void ShootLaser()
    {
        RaycastHit2D[] hitAll = Physics2D.RaycastAll(laserFirePoint.position, transform.right);
        if (hitAll.Length > 1)
        {
            int index = 1;
            for(int i = 1; i < hitAll.Length; ++i)
            {
                if (!hitAll[i].collider.CompareTag("Button")){
                    index = i;
                    break;
                }
            }


            //Debug.Log(hitAll[index].collider);


            //if (hitAll[index].collider.CompareTag("Player"))
            //{
            //    // game over
            //}

            if (!endParticlesPlaying)
            {
                endParticlesPlaying = true;
                laserEndParticles.Play(true);
            }
            laserEndParticles.gameObject.transform.position = hitAll[index].point;
            float distance = ((Vector2)hitAll[index].point - (Vector2)transform.position).magnitude;
            lineRenderer.SetPosition(1, new Vector3(distance, 0, 0));
            Draw2DRay(laserFirePoint.position, hitAll[index].point);
        }
        else
        {
            Draw2DRay(laserFirePoint.position, laserFirePoint.transform.right * distanceRay);
        }
    }

    private void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    public void DisableParticle()
    {
        Destroy(laserEndParticles);
    }
}

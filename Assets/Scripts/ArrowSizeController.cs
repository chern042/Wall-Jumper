using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSizeController : MonoBehaviour
{
    public Transform[] arrows;


    public void MakeArrowsBigger(float distanceDelta)
    {


            for(int i=0;i<arrows.Length;i++)
            {

                if (distanceDelta >= 0.6f && distanceDelta <= 1.65f)
                {
                    arrows[i].localScale = new Vector3(distanceDelta , distanceDelta , arrows[i].localScale.z);


                }



            }

        }
       

    
}

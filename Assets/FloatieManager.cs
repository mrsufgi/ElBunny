using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Random = UnityEngine.Random;

public class FloatieManager : MonoBehaviour
{

    private Floaties m_PassiveFloatie;
    private Floaties m_ActiveFloatie;
    private Floaties m_OffFloatie;

    public List<Floaties> Floties;

    private int currentFloatieState = 0;
    private const float c_OffScreenPositionX = 15f;
    private float width = Screen.width;
    public Player Bunny;

    void Next()
    {

        m_ActiveFloatie = Floties[currentFloatieState % 3];
        m_ActiveFloatie.transform.name = "Active";
        m_PassiveFloatie = Floties[(currentFloatieState + 2) % 3];
        m_PassiveFloatie.transform.name = "Passive";
        m_OffFloatie = Floties[(currentFloatieState + 1) % 3];
        m_OffFloatie.transform.name = "Off";

        OnFloatie = m_PassiveFloatie;
        currentFloatieState++;
    }

    Floaties lastFloatie;

    float xRange;
    private int size = 1;
    float yRange;
    float yStandard;

 
    // Use this for initialization
    void Start()
    {

        Next();

        m_PassiveFloatie.Spawn(size);
        
        yStandard = m_PassiveFloatie.transform.position.y;

        refreshRandom();
  
        m_ActiveFloatie.Spawn(size);

        m_ActiveFloatie.transform.position = new Vector3(m_PassiveFloatie.xEnd + xRange, yStandard + yRange, 0);

        refreshRandom();

        m_OffFloatie.Spawn(size);

        m_OffFloatie.transform.position = new Vector3(m_PassiveFloatie.xEnd + 15f, yStandard + yRange, 0);

        refreshRandom();
    }


      void OnTriggerExit2D(Collider2D other)
    {

            print("HHHHIII");
          if (other.gameObject.CompareTag("Edge"))
          {

            Floaties floatie = other.gameObject.transform.parent.GetComponent<Floaties>();
            floatie.DespawnFloatie();

            refreshRandom();
          
            floatie.Spawn(size);
            
            floatie.transform.position = new Vector3(m_PassiveFloatie.xEnd + 15f, yStandard + yRange, 0);
        }
      }

      void refreshRandom()
      {

      //   var cam = Camera.main.ScreenToWorldPoint(new Vector3(width, 0, 10));
      //  print(cam);
          size = Random.Range(1, 7);
          xRange = Random.Range(1f, 9);
          yRange = Random.Range(-3f, 3f);


        
        /*   if (xRange < 2.5f)
           {
               yRange = Random.Range(0, 0.3f);
           }
           else if (xRange < 3.3f)
           {
               yRange = Random.Range(0, 1.7f);
           }
           else
           {
               yRange = Random.Range(0, 3.4f);
           }*/
    }

    public Floaties OnFloatie { get; private set; }

    public void Reposition()
    {
       Next();
        refreshRandom();
        m_ActiveFloatie.transform.DOMoveX(m_PassiveFloatie.xEnd + xRange, 3f).OnComplete(() =>
        {
            Bunny.angle = getPassiveActiveVector();
        });
    }
    
    public float getPassiveActiveVector()
    {
        print(m_ActiveFloatie.topLeft);
        print(m_PassiveFloatie.topRight);
        Vector2 diference = m_ActiveFloatie.topLeft - m_PassiveFloatie.topRight;
        print(diference.normalized);
        float sign = (m_ActiveFloatie.topLeft.y < m_PassiveFloatie.topRight.y) ? -1.0f : 1.0f;
        print(Vector2.Angle(Vector2.right, diference) * sign);
        return Vector2.Angle(Vector2.right, diference) * sign;
    }
}
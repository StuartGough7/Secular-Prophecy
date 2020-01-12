using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralaxer : MonoBehaviour
{
    public Transform[] backgrounds;                                                                                                 // list of backgrounds and foregrounds to be parralaxed
    public float smoothing = 1f;                                                                                                    // How to smooth the parralaxing is going to be 


    private float[] parallaxScales;                                                                                                 // proportion of the camera to move the backgrounds by
    private Transform cam;
    private Vector3 previousCameraPosition;

    void Awake()                                                                                                                    // called before Start but after all objects are setup (great for asigning references)
    {
        cam = Camera.main.transform;                                                                                                // refernce to maing camera, could have made a public adn dragged it in but this is neater
    }

    void Start()
    {
        
        previousCameraPosition = cam.position;                                                                                      // Previousframe had current frames position
        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;                                                                     // note the -ve sign
        }
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)                                                                                // parralax is the opposite of the camera movement because of the previous frame minus multiplied by scale
        {
            float parallax = (previousCameraPosition.x - cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;                                                      //Set target position which is the current position plus the parralax
            Vector3 backgroundtargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundtargetPos, smoothing * Time.deltaTime);
        }
        previousCameraPosition = cam.position;
    }
}


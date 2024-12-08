using System.Collections.Generic;
using extOSC;
using UnityEngine;

public class CoordinateInput : MonoBehaviour
{
    [Header("OSC Receiver")]
    public OSCReceiver oSCReceiver;

    [Header("Configuration")]
    public List<GameObject> SoundObjects; // Assign your 8 GameObjects in the Inspector
    public string BaseAddress = "/track"; // Base address for OSC messages

    void Start()
    {
        // Bind each GameObject to its corresponding OSC address
        for (int i = 0; i < SoundObjects.Count; i++)
        {
            int index = i + 1; // OSC tracks start from 1
            string addressX = $"{BaseAddress}/{index}/x";
            string addressY = $"{BaseAddress}/{index}/y";
            string addressZ = $"{BaseAddress}/{index}/z";

            oSCReceiver.Bind(addressX, message => UpdatePosition(message, index - 1, 'x'));
            oSCReceiver.Bind(addressY, message => UpdatePosition(message, index - 1, 'y'));
            oSCReceiver.Bind(addressZ, message => UpdatePosition(message, index - 1, 'z'));
        }
    }

    void UpdatePosition(OSCMessage oSCMessage, int objectIndex, char axis)
    {
        if (objectIndex < 0 || objectIndex >= SoundObjects.Count) return;

        GameObject soundObject = SoundObjects[objectIndex];
        if (soundObject == null) return;

        Vector3 position = soundObject.transform.position;

        if (oSCMessage.ToFloat(out var value))
        {
            // Update the corresponding axis
            switch (axis)
            {
                case 'x': position.x = value; break;
                case 'y': position.y = value; break;
                case 'z': position.z = value; break;
            }

            soundObject.transform.position = position;
        }
    }
}


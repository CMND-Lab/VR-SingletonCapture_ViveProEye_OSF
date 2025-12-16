using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;


namespace VRSingletonSearch
{
    public class TargetController : MonoBehaviour
    {
        [SerializeField] GameObject sphereMesh;
        [SerializeField] GameObject cubeMesh;
        
        private void Awake()
        {
        }

        public void ShowTarget(bool show) {
            gameObject.SetActive(show);
        }

        public void SetColour(Color colour) {
            sphereMesh.GetComponent<MeshRenderer>().material.color = colour;
            cubeMesh.GetComponent<MeshRenderer>().material.color = colour;
        }

        public void SetMesh(TargetShape shape) {
            if (shape == TargetShape.Apple) {
                sphereMesh.SetActive(true);
                cubeMesh.SetActive(false);
            } 
            else if (shape == TargetShape.Pear) {
                sphereMesh.SetActive(false);
                cubeMesh.SetActive(true);    
            }
            else {
                sphereMesh.SetActive(false);
                cubeMesh.SetActive(false);
            }
        }

        public void SetMaterial(Material mat) {
            sphereMesh.GetComponent<MeshRenderer>().material = mat;
            cubeMesh.GetComponent<MeshRenderer>().material = mat;
        }
    }

    public enum TargetShape {
        Apple = 0,
        Pear = 1
    }
}
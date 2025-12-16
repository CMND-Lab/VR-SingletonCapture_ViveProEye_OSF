using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using Tobii.G2OM;
using UXF;

namespace VRSingletonSearch
{
    public class GazeFocusDetector : MonoBehaviour, IGazeFocusable
    {
        public Color HighlightColor = Color.red;
        public float AnimationTime = 0.1f;

        private Renderer _renderer;
        private Color _originalColor;
        private Color _targetColor;
        
        public bool focus = false;

        public bool highlight;
        
        public void GazeFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                focus = true;
                _targetColor = HighlightColor;
            }
            else
            {
                focus = false;
                _targetColor = _originalColor;
            }
        }

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            if (_renderer != null) {
                _originalColor = _renderer.material.color;
                _targetColor = _originalColor;
            }
        }
        

        private void Update()
        {
            if (highlight && _renderer != null)
            {
                //This lerp will fade the color of the object
                if (_renderer.material.HasProperty(Shader.PropertyToID("_BaseColor"))) // new rendering pipeline (lightweight, hd, universal...)
                {
                    _renderer.material.SetColor("_BaseColor",
                        Color.Lerp(_renderer.material.GetColor("_BaseColor"), _targetColor,
                            Time.deltaTime * (1 / AnimationTime)));
                }
                else // old standard rendering pipline
                {
                    _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor,
                        Time.deltaTime * (1 / AnimationTime));
                }
            }
        }
    }
}


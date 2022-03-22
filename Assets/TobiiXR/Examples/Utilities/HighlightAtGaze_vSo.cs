// Copyright © 2018 – Property of Tobii AB (publ) - All Rights Reserved

using Tobii.G2OM;
using UnityEngine;
using HutongGames.PlayMaker;

namespace Tobii.XR.Examples
{
//Monobehaviour which implements the "IGazeFocusable" interface, meaning it will be called on when the object receives focus
    public class HighlightAtGaze_vSo : MonoBehaviour, IGazeFocusable
    {
        //public Color HighlightColor = Color.clear;
        //public float AnimationTime = 0.1f;
        public PlayMakerFSM GoFsm;

        //private Renderer _renderer;
        //private Color _originalColor;
        //private Color _targetColor;
        //The method of the "IGazeFocusable" interface, which will be called when this object receives or loses focus
        public void GazeFocusChanged(bool hasFocus)
        {
            
            //If this object received focus, fade the object's color to highlight color
            if (hasFocus)
            {
                GoFsm.gameObject.GetComponent<PlayMakerFSM>();
                FsmGameObject thisGameObject = GoFsm.FsmVariables.GetFsmGameObject("sendByO_g");
                thisGameObject.Value = this.gameObject;

                if (this.gameObject.tag == "Necessary")
                {
                    GoFsm.SendEvent("Watching");
                }

                else if (this.gameObject.tag == "WallCollider")
                {
                    GoFsm.SendEvent("Watching_W");
                }
                

                //if(this._renderer != null)
                //{
                //    _targetColor = HighlightColor;
                //}
            }
            //If this object lost focus, fade the object's color to it's original color
            else
            {
                GoFsm.gameObject.GetComponent<PlayMakerFSM>();
                GoFsm.SendEvent("Not Watching");
                //if(this._renderer != null)
                //{
                //    _targetColor = _originalColor;
                //}
            }
        }
   
        private void Start()
        {
            //if (GetComponent<Renderer>())
            //{
            //    if (GetComponent<Renderer>().material.HasProperty("_Color") == true)
            //    {
            //        _renderer = GetComponent<Renderer>();
            //        _originalColor = _renderer.material.color;
            //        _targetColor = _originalColor;
            //    }
                
            //}            
        }

        private void Update()
        {
            //if (GetComponent<Renderer>())
            //{
            //    if (GetComponent<Renderer>().material.HasProperty("_Color") == true)
            //    {
            //        //This lerp will fade the color of the object
            //        if (_renderer.material.HasProperty(Shader.PropertyToID("_BaseColor"))) // new rendering pipeline (lightweight, hd, universal...)
            //        {
            //            _renderer.material.SetColor("_BaseColor", Color.Lerp(_renderer.material.GetColor("_BaseColor"), _targetColor, Time.deltaTime * (1 / AnimationTime)));
            //        }
            //        else // old standard rendering pipline
            //        {
            //            _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, Time.deltaTime * (1 / AnimationTime));
            //        }
            //    }
                    
            //}
            //else return;
        }
    }
}

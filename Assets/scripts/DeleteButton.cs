using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class ButtonClass : UIBehaviour
    {
        [SerializeField]
        private Button _button = null;

        private void Awake()
        {
            _button.onClick.AddListener(() => Destroy(GameObjectClass.CurrentlySelectedGameObject));
        }

    private void Destroy(object currentlySelectedGameObject)
    {
        throw new NotImplementedException();
    }

    private void OnDestroy()
        { 
            _button.onClick.RemoveAllListeners();
        }
    }


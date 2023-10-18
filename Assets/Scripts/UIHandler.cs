using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIHandler : MonoBehaviour
{

}

public interface IUIHandler
{
    public void Hide();
    public void Show();
}

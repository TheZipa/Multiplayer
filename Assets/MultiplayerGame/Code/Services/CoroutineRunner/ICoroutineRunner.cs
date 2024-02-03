using System.Collections;
using UnityEngine;

namespace MultiplayerGame.Code.Services.CoroutineRunner
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}
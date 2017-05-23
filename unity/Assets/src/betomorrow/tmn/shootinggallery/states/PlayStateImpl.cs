using UnityEngine;
using System;

using ksubaka.sdk;
using ksubaka.states;

namespace betomorrow.tmn.shootinggallery
{
    public class PlayStateImpl : MonoBehaviour, PlayState
    {

        public event GameOverEvent gameOver;

        GameManager _gameManager;
        TargetPool _targetPool;
        static PlayStateImpl instance;
        
        public static PlayStateImpl Instance {
            get { return instance; }
        }
        
        void Awake ()
        {
            instance = this;
        }

        public void OnStart (Context context)
        {
            _gameManager = FindObjectOfType<GameManager> ();
            _targetPool = FindObjectOfType<TargetPool> ();
            //  _targetPool.targetDestroyed += OnTargetDestroyed;
        }

        public void OnStop ()
        {
//      _targetPool.targetDestroyed -= OnTargetDestroyed;
        }
//
//  void OnTargetDestroyed (TargetController target) {
//      if (_targetPool.liveTargetCount <=16) {
//          _gameManager.NotifyWin();
//          if (gameOver != null) { gameOver(); }
//      }
//  }

        public   void OnOutOfSkittles ()
        {

            _gameManager.NotifyWin ();
            if (gameOver != null) {
                gameOver ();
            }

        }

    }
}

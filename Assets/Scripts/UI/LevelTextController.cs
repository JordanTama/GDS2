﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chapterText;
    private RectTransform _chapterTextTransform;
    private RectTransform _levelTextTransform;
    [SerializeField] private TextMeshProUGUI levelText;
    private Tween _chapterMoveTween;
    private Tween _levelMoveTween;
    private Tween _chapterColorTween;
    private Tween _levelColorTween;

    [SerializeField] private float moveLerpTime = 2f;
    [SerializeField] private float colorLerpTime = 2f;
    [SerializeField] private float colorDelayTime = 1f;
    [SerializeField] private float chapterXPos;
    [SerializeField] private float levelXPos;

    private GameLoopManager _gameLoopManager;


    // Start is called before the first frame update
    private void Awake()
    {
        _chapterTextTransform = chapterText.rectTransform;
        _levelTextTransform = levelText.rectTransform;
        chapterXPos = -1f * (_chapterTextTransform.anchoredPosition.x + _chapterTextTransform.rect.width);
        levelXPos = -1f * (_levelTextTransform.anchoredPosition.x + _levelTextTransform.rect.width);

        chapterText.text = "Chapter " + SceneManager.GetActiveScene().name.Split('-')[0].Split('l')[1];
        levelText.text = "Part " + SceneManager.GetActiveScene().name.Split('-')[1];

        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _gameLoopManager.OnRestart += AnimateText;
    }

    void Start()
    {
        // AnimateText();
    
    }

    private void ShowText()
    {
        if(_chapterColorTween.IsPlaying())_chapterColorTween.Complete();
        if(_chapterMoveTween.IsPlaying())_chapterMoveTween.Complete();
        if(_levelMoveTween.IsPlaying())_levelMoveTween.Complete();
        if(_levelColorTween.IsPlaying())_levelColorTween.Complete();
        
        _chapterColorTween = chapterText.DOFade(1f, 0f);
        _levelColorTween = levelText.DOFade(1f, 0f);
    }

    private void AnimateText()
    {
        _chapterMoveTween = _chapterTextTransform.DOLocalMoveX(chapterXPos, moveLerpTime).From().SetEase(Ease.OutCubic);
        _levelMoveTween = _levelTextTransform.DOLocalMoveX(levelXPos,1.25f*moveLerpTime).From().SetEase(Ease.OutCubic).SetDelay(moveLerpTime*0.75f).OnComplete(() =>
        { 
            _chapterColorTween = chapterText.DOFade(0f, colorLerpTime).SetDelay(colorDelayTime);
            _levelColorTween = levelText.DOFade(0f, colorLerpTime).SetDelay(colorDelayTime);   
        });
    }

    private void OnDestroy()
    {
        _gameLoopManager.OnRestart -= AnimateText;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderChangeTeamColor : MonoBehaviour {
    public int teamColor;

    public SpriteRenderer leftHip, rightHip;
    public SpriteRenderer leftLeg, rightLeg;
    public SpriteRenderer head;

    public List<Sprite> hips, legs, heads;

    private void Start() {
        leftHip.sprite = hips[teamColor];
        rightHip.sprite = hips[teamColor];
        leftLeg.sprite = legs[teamColor];
        rightLeg.sprite = legs[teamColor];
        head.sprite = heads[teamColor];
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : TutorialState.cs
==============================
*/
public class TutorialState : State
{
    public override State RunCurrentState()
    {
        return this;
    }
}

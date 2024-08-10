using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UserRole
{
    Security = 1,       //경비원
    Patient = 2,        //환자
    Mental = 3          //정신병자
}

public enum UserEnding
{
    DeadEnding = 0,
    WinEnding = 1,
    LoseEnding = 2,
    NomalEnding = 3
}
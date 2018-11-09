using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainScript {
    private static int round=0, score=0;
    private static ArrayList[][] list;
    private static bool gameStart=false;
    private static bool startVid=false;
    private static float progress=0;
    private static bool done=false;
    private static float contentLength=1f;
    private static float curSize=0;

    public static int Round {
        get {
            return round;
        }

        set {
            round = value;
        }
    }

    public static ArrayList[][] List {
        get {
            return list;
        }

        set {
            list = value;
        }
    }

    public static int Score {
        get {
            return score;
        }

        set {
            score = value;
        }
    }

    public static bool GameStart {
        get {
            return gameStart;
        }

        set {
            gameStart = value;
        }
    }

    public static bool StartVid {
        get {
            return startVid;
        }

        set {
            startVid = value;
        }
    }

    public static float Progress {
        get {
            return progress;
        }

        set {
            progress = value;
        }
    }

    public static bool Done {
        get {
            return done;
        }

        set {
            done = value;
        }
    }

    public static float ContentLength {
        get {
            return contentLength;
        }

        set {
            contentLength = value;
        }
    }

    public static float CurSize {
        get {
            return curSize;
        }

        set {
            curSize = value;
        }
    }
}

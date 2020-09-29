state("A Slower Speed of Light") {
    // // we want values 8->16 bytes in, so everything is n*24 + 8 for last offset
    // // ulong orbNumber7 : 0x008D86E4, 0x4, 0x24, 0x94, 0x0, 0x10, 0x14, 0x2C0;
    // // ulong orbNumber22 : 0x008D86E4, 0x4, 0x24, 0x94, 0x0, 0x10, 0x14, 0x728;
    // // ulong orbNumber44 : 0x008D86E4, 0x4, 0x24, 0x94, 0x0, 0x10, 0x14, 0x410;
    // // ulong orbStartOfHallway : 0x008D86E4, 0x4, 0x24, 0x94, 0x0, 0x10, 0x14, 0x3C8;
    // // ulong orbEndOfHallway : 0x008D86E4, 0x4, 0x24, 0x94, 0x0, 0x10, 0x14, 0x4E8;
    // // ulong orbFirstInPark : 0x008D86E4, 0x4, 0x24, 0x94, 0x0, 0x10, 0x14, 0xB0;
    // // ulong orbEndPark : 0x008D86E4, 0x4, 0x24, 0x94, 0x0, 0x10, 0x14, 0xE0;
    // // ulong orbFinalRun : 0x008D86E4, 0x4, 0x24, 0x94, 0x0, 0x10, 0x14, 0x2A8;
    // // ulong orbLastInRun : 0x008D86E4, 0x4, 0x24, 0x94, 0x0, 0x10, 0x14, 0x8;
    // // offset: 0x8
    // ulong orbLastInRun : 0x008d4458, 0x44, 0x0, 0x28, 0x18, 0x1c, 0x14, 0x8;
    // // offset: 0xB0
    // ulong orbFirstInPark : 0x008d86ec, 0x50, 0x0, 0x94, 0x4, 0x10, 0x14, 0xB0;
    // // offset: 0xE0
    // // ulong orbEndPark : 0x008D86EC, 0x50, 0x0, 0x94, 0x4, 0x10, 0x14, 0xE0;
    // ulong orbEndPark : 0x008D86EC, 0x50, 0x0, 0x94, 0x0, 0x10, 0x14, 0xE0;
    // // ulong orbEndPark : 0x008D86EC, 0x50, 0x0, 0x94, 0x10, 0x50, 0x70, 0xE0;
    // // offset: 0x2A8
    // // ulong orbFinalRun : 0x008D86EC, 0x50, 0x0, 0x94, 0x0, 0x10, 0x14, 0x2A8;
    // ulong orbFinalRun : 0x008D86EC, 0x50, 0x0, 0x94, 0x4, 0x10, 0x14, 0x2A8;
    // // offset: 0x2C0
    // // ulong orbNumber7 : 0x008D86EC, 0x50, 0x0, 0x94, 0x0, 0x10, 0x14, 0x2c0;
    // ulong orbNumber7 : 0x008D86EC, 0x50, 0x0, 0x94, 0x4, 0x10, 0x14, 0x2c0;
    // // ulong orbNumber7 : 0x008D86EC, 0x50, 0x0, 0x94, 0x10, 0x50, 0x70, 0x2c0;
    // // offset: 0x3C8
    // // ulong orbStartOfHallway : 0x008D86EC, 0x50, 0x0, 0x94, 0x0, 0x10, 0x14, 0x3C8;
    // ulong orbStartOfHallway : 0x008D86EC, 0x50, 0x0, 0x94, 0x4, 0x10, 0x14, 0x3C8;
    // // ulong orbStartOfHallway : 0x008D86EC, 0x50, 0x0, 0x94, 0x10, 0x50, 0x70, 0x3C8;
    // // ulong orbStartOfHallway : 0x008D86EC, 0x44, 0x0, 0x28, 0x18, 0x1c, 0x14, 0x3C8;
    // // offset: 0x410
    // // ulong orbNumber44 : 0x008D86EC, 0x50, 0x0, 0x94, 0x0, 0x10, 0x14, 0x410;
    // ulong orbNumber44 : 0x008D86EC, 0x50, 0x0, 0x94, 0x4, 0x10, 0x14, 0x410;
    // // ulong orbNumber44 : 0x008D86EC, 0x50, 0x0, 0x94, 0x10, 0x50, 0x70, 0x410;
    // // offset: 0x4E8
    // // ulong orbEndOfHallway : 0x008D86EC, 0x50, 0x0, 0x94, 0x0, 0x10, 0x14, 0x4E8;
    // ulong orbEndOfHallway : 0x008D86EC, 0x50, 0x0, 0x94, 0x4, 0x10, 0x14, 0x4E8;
    // // ulong orbEndOfHallway : 0x008D4458, 0x44, 0x0, 0x28, 0x18, 0x1c, 0x14, 0x4E8;
    // // ulong orbEndOfHallway : 0x008D86EC, 0x50, 0x0, 0x94, 0x10, 0x50, 0x70, 0x4E8;
    // // offset: 0x728
    // ulong orbNumber22 : 0x008D86EC, 0x50, 0x0, 0x94, 0x0, 0x10, 0x14, 0x728;
    // // ulong orbNumber22 : 0x008D86EC, 0x50, 0x0, 0x94, 0x4, 0x10, 0x14, 0x728;
    // // ulong orbNumber22 : 0x008d4458, 0x44, 0x0, 0x28, 0x18, 0x1c, 0x14, 0x728;
    // // ulong orbNumber22 : 0x008d4458, 0x44, 0x0, 0x28, 0x3c, 0x3c, 0x14, 0x710;
    // // ulong orbNumber22 : 0x008D86EC, 0x50, 0x0, 0x94, 0x10, 0x50, 0x70, 0x728;

    // 70
    bool orbNumber7 : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x24, 86;
    // 23
    bool orbNumber22 : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x24, 39;
    // 56
    bool orbNumber44 : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x24, 72;
    // 59
    bool orbStartOfHallway : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x24, 75;
    // 47
    bool orbEndOfHallway : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x24, 63;
    // 92
    bool orbFirstInPark : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x24, 108;
    // 90
    bool orbEndPark : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x24, 106;
    // 71
    bool orbFinalRun : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x24, 87;
    // 99
    bool orbLastInRun : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x24, 117;

    double totalTimePlayer : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0xa0;
    bool movementFrozen : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x84;
    // 0 in intro-slides, null in loading, 0 when you get into the level
    // double totalTimePlayerNullWhenLoading : 0x008D8B4C, 0x14, 0x8, 0x14, 0x58, 0x98;

    bool gameWin : 0x00376C0C, 0x11c, 0xc0, 0x0, 0x70, 0x14, 0x86;

    uint orbCounter : 0x008D7CB4, 0x4, 0x8, 0x14, 0x70;

    // null in menu, 0 when loading, 0 in game default
    uint orbCounterNullInMenu : 0x008D7CB4, 0x4, 0x8, 0x50, 0x70, 0x70;
}

startup {
    vars.scanTarget = new SigScanTarget(14,
        "558BEC53575683EC0C8B7D088B05",
        "????????",
        "83EC086A0050E8",
        "????????",
        "83C41085C00F84DF0000008B05");

    settings.Add("enable", true, "Enable");
    settings.SetToolTip("enable", "Enable this autosplitter");
}

init {
    vars.inLevel = false;
    vars.inMenu = false;
    vars.split = 0;
    vars.tick = 0;
    vars.author = "XertroV";
    vars.finishedRun = false;
    vars.orbs = new Dictionary<string, bool>();
    vars.lastSplit = 0;
}

update {
    // in menu orbCounters don't match (mostly?)
    // in slides totalTimePlayer doesn't match
    // in loading totalTimePlayer doesn't match
    vars.inLevel = current.orbCounter == current.orbCounterNullInMenu;

    vars.wasInMenu = vars.inMenu;
    vars.inMenu = current.orbCounter != current.orbCounterNullInMenu || current.orbCounter > 100;

    vars.wasFinishedRun = vars.finishedRun;
    vars.finishedRun = current.gameWin && !old.gameWin;

    vars.checkOrbs = new[] { 
        new[] {current.orbNumber7, old.orbNumber7, "orbNumber7"}, 
        new[] {current.orbNumber22, old.orbNumber22, "orbNumber22"}, 
        new[] {current.orbNumber44, old.orbNumber44, "orbNumber44"}, 
        new[] {current.orbStartOfHallway, old.orbStartOfHallway, "orbStartOfHallway"}, 
        new[] {current.orbEndOfHallway, old.orbEndOfHallway, "orbEndOfHallway"}, 
        new[] {current.orbFirstInPark, old.orbFirstInPark, "orbFirstInPark"}, 
        new[] {current.orbEndPark, old.orbEndPark, "orbEndPark"}, 
        new[] {current.orbFinalRun, old.orbFinalRun, "orbFinalRun"}, 
        new[] {current.gameWin, old.gameWin, "gameWin"},
    };

    vars.tick += 1;
    if (vars.tick % 120 == 0) {
        print(
            "\n| totalTimePlayer: " + current.totalTimePlayer + 
            "\n| gameWin: " + current.gameWin + 
            "\n| orbCounter: " + current.orbCounter + 
            "\n| orbCounterNullInMenu: " + current.orbCounterNullInMenu +
            "\n| orb7: " + current.orbNumber7 +
            "\n| orb22: " + current.orbNumber22 +
            "\n| orb44: " + current.orbNumber44 +
            "\n| orbStartOfHallway: " + current.orbStartOfHallway +
            "\n| orbEndOfHallway: " + current.orbEndOfHallway +
            "\n| orbFirstInPark: " + current.orbFirstInPark +
            "\n| orbEndPark: " + current.orbEndPark +
            "\n| orbFinalRun: " + current.orbFinalRun +
            "\n| orbLastInRun: " + current.orbLastInRun +
            "\n| movementFrozen: " + current.movementFrozen +
            "\n| vars.tick: " + vars.tick +
            "\n| vars.author: " + vars.author +
            "\n| vars.split: " + vars.split +
            "\n| vars.inMenu: " + vars.inMenu +
            "\n| vars.inLevel: " + vars.inLevel +
            // "\n| vars.finishedRun: " + vars.finishedRun +
            ""
            );
    }

}

start {
    if (vars.inLevel) {
        if (vars.inLevel && !current.gameWin && current.totalTimePlayer > 0 && current.totalTimePlayer > old.totalTimePlayer) {
            // vars.split = 1;
            return true;
        }
    }
}

split {
    // if (vars.inLevel && vars.tick > (60 + vars.lastSplit)) {
    if (vars.inLevel) {
        foreach (var t in vars.checkOrbs) {
            var curr = t[0];
            var prev = t[1];
            var name = t[2];
            if (!vars.orbs.ContainsKey(name)) {
                vars.orbs[name] = false;
            }
            // print("checking: " + name + " > " + curr + " : " + prev + " -- " + vars.orbs[name]);
            if (curr == true && prev == false) {
                vars.orbs[name] = true;
                print("splitting: " + name + ": " + curr + " (was: " + prev + ")");
                vars.lastSplit = vars.tick;
                return true;
            }
        }
    }
}

reset {
    // clicking off the post-game time screen
    if (!current.gameWin && old.gameWin) {
        vars.orbs = new Dictionary<string, bool>();
        print("splitting on return to menu");
        return true;
    }
    if (vars.inMenu && !vars.wasInMenu) {
        // vars.split = 0;
        vars.orbs = new Dictionary<string, bool>();
        return true;
    }
}

isLoading {
    return vars.inMenu || current.movementFrozen;
}

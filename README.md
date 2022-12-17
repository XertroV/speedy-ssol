1. Project > Add Reference (Reference Manager)
2. Browse > Add "UnityEngine.dll" from your SSOL folder
3. Click "OK"

---

1. Load solution in Visual Studio (preferably the latest version)
2. Right click on the "Assembly-CSharp" project (on the right side) > Build
3. Build events > post build event will update ssol folder in ../../SSOL_PATCHED (modify anything here)

---

Alternatively, you can go to Build > Configuration Manager and set the project to Release, if you want it to be that way.
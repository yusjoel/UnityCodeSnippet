@echo off
for %%a in (*.unitypackage) do (echo [%%a]
UnityPackageRenamer.exe "%%a" AssetStore
echo ----------------------------------------)
echo on
pause
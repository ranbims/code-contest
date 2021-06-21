cd /d "C:\Code\CodeContest\CodeContest" &msbuild "CodeContest.csproj" /t:sdvViewer /p:configuration="Debug" /p:platform="x86" /p:SolutionDir="C:\Code\CodeContest" 
exit %errorlevel% 
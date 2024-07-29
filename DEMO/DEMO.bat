@echo off
start "" "DEMO_Server\MyServer.exe"
timeout /t 3 /nobreak >nul
start "" "DEMO_Client\MyClient2.exe"
start "" "DEMO_Client\MyClient2.exe"
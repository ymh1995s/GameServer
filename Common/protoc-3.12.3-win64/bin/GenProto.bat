protoc.exe -I=./ --csharp_out=./ ./Protocol.proto 
IF ERRORLEVEL 1 PAUSE

START ../../../MyServer/PacketGenerator/bin/Debug/net6.0/PacketGenerator.exe ./Protocol.proto
XCOPY /Y Protocol.cs "../../../MyClient/Assets/Scripts/Packet"
XCOPY /Y Protocol.cs "../../../MyServer/MyServer/Packet"
XCOPY /Y ClientPacketManager.cs "../../../MyClient/Assets/Scripts/Packet"
XCOPY /Y ServerPacketManager.cs "../../../MyServer/MyServer/Packet"
rm *.dll
rm *.exe

ls -l

mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:RicochetBall.dll RicochetBall.cs

mcs -r:System -r:System.Windows.Forms -r:RicochetBall.dll -out:ball.exe RicochetBallMain.cs

./ball.exe
using System;
using System.Windows.Forms;

public class RicochetBallMain
{
	public static void Main()
	{
		System.Console.WriteLine("Assigment 4(Ricochet Ball) has begun.");
		RicochetBall rb = new RicochetBall();
		Application.Run(rb);
		System.Console.WriteLine("Assignmet 4(Ricochet Ball) has ended.");
	}
}
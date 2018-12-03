using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

public class RicochetBall : Form
{
	private Label name = new Label();

	Button newball = new Button();
	Button reset = new Button();
	Button start = new Button();
	Button pause = new Button();
	Button exit = new Button();

	private	Label refreshratebox_name = new Label();
	private TextBox refreshratebox = new TextBox();	
	private Label speedbox_name = new Label();
	private TextBox speedbox = new TextBox();
	private Label degreebox_name = new Label();
	private TextBox degreebox = new TextBox();

	private double radius = 10.0;
	private double diameter;
	private SolidBrush BallBrush = new SolidBrush(Color.White);

	private double ball_location_x; // top left of the ball, the point used to draw the ball
	private double ball_location_y;
	private double delta_x; // the change in x and y
	private double delta_y;
	private double direction_angle;
	private double linear_speed; // user input speed
	private double speed; // speed used to calculate delta_x and delta_y, (pixel/tic)

	//variables for clocks
	private const double refresh_rate = 100.0;
	private const double time_converter = 1000.0;
	private double dot_update_rate;
	private const double delta = 1.0;

	private static System.Timers.Timer animation_clock = new System.Timers.Timer();
	private static System.Timers.Timer refresh_clock = new System.Timers.Timer();

	public RicochetBall()
	{
		Text = "Ricochet Ball";
		Size = new Size(1600, 900);

		ball_location_x = 800 - radius;
		ball_location_y = 410 - radius;

		diameter = 2*radius;

		name.Text = "Haowen Yong -- Ricochet Ball";
		name.Size = new Size(360, 30);
		name.Font = new Font("Arial", 18);
		name.Location = new Point(620, 15);
		name.ForeColor = Color.White;
		name.BackColor = Color.Black;

		newball.Text = "new";
		newball.Size = new Size(85, 25);
		newball.Location = new Point(50, 820);
		start.Text = "start";
		start.Size = new Size(85, 25);
		start.Location = new Point(150, 820);
		pause.Text = "pause";
		pause.Size = new Size(85, 25);
		pause.Location = new Point(250, 820);
		reset.Text = "reset";
		reset.Size = new Size(85, 25);
		reset.Location = new Point(350, 820);

		exit.Text = "exit";
		exit.Size = new Size(85, 25);
		exit.Location = new Point(1450, 770);

		refreshratebox_name.Text = "Refresh Rate (tic/sec) ";
		refreshratebox_name.Size = new Size(80, 25);
		refreshratebox_name.Location = new Point(50, 770);
		refreshratebox_name.ForeColor = Color.White;
		refreshratebox_name.BackColor = Color.Gray;
		refreshratebox.Size = new Size(80, 25);
		refreshratebox.Location = new Point(135, 770);

		speedbox_name.Text = "Speed (pixel/sec) ";
		speedbox_name.Size = new Size(70, 25);
		speedbox_name.Location = new Point(265, 770);
		speedbox_name.ForeColor = Color.White;
		speedbox_name.BackColor = Color.Gray;
		speedbox.Size = new Size(80, 25);
		speedbox.Location = new Point(340, 770);

		degreebox_name.Text = "Direction (degree) ";
		degreebox_name.Size = new Size (60, 25);
		degreebox_name.Location = new Point(470, 770);
		degreebox_name.ForeColor = Color.White;
		degreebox_name.BackColor = Color.Gray;
		degreebox.Size = new Size(80, 25);
		degreebox.Location = new Point(535, 770);

		Controls.Add(name);
		Controls.Add(newball);
		Controls.Add(reset);
		Controls.Add(start);
		Controls.Add(pause);
		Controls.Add(exit);
		Controls.Add(refreshratebox_name);
		Controls.Add(refreshratebox);
		Controls.Add(speedbox_name);
		Controls.Add(speedbox);
		Controls.Add(degreebox_name);
		Controls.Add(degreebox);

		newball.Click += new EventHandler(newball_click);
		reset.Click += new EventHandler(reset_click);
		start.Click += new EventHandler(start_click);
		pause.Click += new EventHandler(pause_click);
		exit.Click += new EventHandler(exit_click);

		animation_clock.Enabled = false;
		animation_clock.Elapsed += new ElapsedEventHandler(update_dot_position);

		refresh_clock.Enabled = false;
		refresh_clock.Elapsed += new ElapsedEventHandler(update_graphics);

		DoubleBuffered = true;
	}

	protected override void OnPaint(PaintEventArgs a)
	{
		Graphics board = a.Graphics;
		board.FillRectangle(Brushes.Black, 0, 0, 1600, 60);
		board.FillRectangle(Brushes.White, 0, 60, 1600, 700);
		board.FillRectangle(Brushes.Gray, 0, 760, 1600, 140);

		board.FillEllipse(BallBrush, (float)ball_location_x, (float)ball_location_y, (float)diameter, (float)diameter);

		base.OnPaint(a);
	}

	protected void start_refresh_clock(double refreshrate)
	{
		double elapsed_time_between_tics;
		if(refreshrate < 1.0)
			refreshrate = 1.0;
		elapsed_time_between_tics = time_converter/refreshrate;
		refresh_clock.Interval = (int)System.Math.Round(elapsed_time_between_tics);
		refresh_clock.Enabled = true;
		System.Console.WriteLine("method start_refresh_clock has terminated, refresh_clock has started");
	}

	protected void start_animation_clock(double updaterate)
	{
		double elapsed_time_between_coordinate_changes;
		if(updaterate < 1.0)
			updaterate = 1.0;
		elapsed_time_between_coordinate_changes = time_converter/updaterate;
		animation_clock.Interval = (int)System.Math.Round(elapsed_time_between_coordinate_changes);
		animation_clock.Enabled = true;
;		System.Console.WriteLine("method start_animation_clock has terminated, animation_clock has started.");
	}

	protected void update_graphics(System.Object sender, ElapsedEventArgs even)
	{
		Invalidate();
	}

	protected void update_dot_position(System.Object sender, ElapsedEventArgs even)
	{
		ball_location_x += delta_x;
		ball_location_y += delta_y;

		if(ball_location_x + diameter >= 1599)
		{
			delta_x = -delta_x;
		}
		if(ball_location_x <= 0)
		{
			delta_x = -delta_x;
		}
		if(ball_location_y + diameter >= 759)
		{
			delta_y = -delta_y;
		}
		if(ball_location_y <= 59)
		{
			delta_y = -delta_y;
		}
	}	

	protected void newball_click(Object sender, EventArgs events)
	{
		System.Console.WriteLine("you have clicked on the new button.");
		BallBrush.Color = Color.Green;
		ball_location_x = 800 - radius;
		ball_location_y = 410 - radius;
		refresh_clock.Enabled = false;
		animation_clock.Enabled = false;
		Invalidate();
	}

	protected void reset_click(Object sender, EventArgs events)
	{
		System.Console.WriteLine("you have clicked on the reset button.");
		refreshratebox.Text = String.Empty;
		speedbox.Text = String.Empty;
		degreebox.Text = String.Empty;
		BallBrush.Color = Color.White;
		ball_location_x = 800 - radius;
		ball_location_y = 410 - radius;
		if(pause.Text == "resume")
			pause.Text = "pause";
		refresh_clock.Enabled = false;
		animation_clock.Enabled = false;
		Invalidate();
	}

	protected void start_click(Object sender, EventArgs events)
	{
		double refresh_rate_user_input = Double.Parse(refreshratebox.Text);
		dot_update_rate = refresh_rate_user_input;
		double speed_input = Double.Parse(speedbox.Text);
		linear_speed = speed_input;
		double degree_input = Double.Parse(degreebox.Text);
		direction_angle = degree_input;
		direction_angle = -1 * direction_angle * (Math.PI / 180);
		speed = linear_speed/dot_update_rate;
		if(speed < 1.0)
			speed = 1.0;
		delta_x = speed * Math.Cos(direction_angle);
		delta_y = speed * Math.Sin(direction_angle);

		start_refresh_clock(refresh_rate);
		start_animation_clock(dot_update_rate);
		System.Console.WriteLine("you have clicked on the start button.");
	}

	protected void pause_click(Object sender, EventArgs events)
	{
		if(pause.Text == "pause")
		{
			refresh_clock.Enabled = false;
			animation_clock.Enabled = false;
			pause.Text = "resume";
			System.Console.WriteLine("you have clicked on the pause button.");
		}
		else
		{
			refresh_clock.Enabled = true;
			animation_clock.Enabled = true;
			pause.Text = "pause";
			System.Console.WriteLine("you have clicked on the resume button.");
		}
	}

	protected void exit_click(Object sender, EventArgs events)
	{
		System.Console.WriteLine("you have clicked on the exit button, the program will now exit.");
		Close();
	}
}